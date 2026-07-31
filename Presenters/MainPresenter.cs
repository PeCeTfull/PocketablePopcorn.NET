using Pocketable_Popcorn.NET.Models;
using Pocketable_Popcorn.NET.Services;
using Pocketable_Popcorn.NET.Services.Interfaces;
using Pocketable_Popcorn.NET.Views;
using Pocketable_Popcorn.NET.Views.Interfaces;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pocketable_Popcorn.NET.Presenters
{
	public class MainPresenter
	{
		private readonly ResourceManager _rm;
		private readonly IMainView _view;
		private readonly IMovieService _movieService;
		private readonly IUserDataService _userDataService;

		public MainPresenter(ResourceManager rm, IMainView view, IMovieService movieService,
			IUserDataService userDataService)
		{
			_rm = rm;
			_view = view;
			_movieService = movieService;
			_userDataService = userDataService;

			_view.OnAbout += new EventHandler(OnAbout);
			_view.OnFormLoad += new EventHandler(OnFormLoad);
			_view.OnMovieItemDoubleClick += new EventHandler(OnMovieItemDoubleClick);
			_view.OnSearch += new EventHandler(OnSearch);
			_view.OnWatchedMovieItemDoubleClick += new EventHandler(OnWatchedMovieItemDoubleClick);
			_view.OnWatchedMoviesClick += new EventHandler(OnWatchedMoviesClick);
		}

		#region Helper Functions

		private void AddWatchedMovieWithImmediateSort(WatchedMovie movie)
		{
			int i;
			string currentTitle;
			string previousTitle;

			for (i = 0; i < _view.WatchedMovies.Count; i++)
			{
				currentTitle = ((WatchedMovie)_view.WatchedMovies[i]).Title;
				if (currentTitle == movie.Title)
				{
					continue;
				}

				previousTitle = i - 1 != -1 ? ((WatchedMovie)_view.WatchedMovies[i - 1]).Title
					: string.Empty;

				string[] titles = new string[3] {previousTitle, movie.Title, currentTitle};
				string[] sortedTitles = new string[3] {previousTitle, movie.Title, currentTitle};
				Array.Sort(sortedTitles);

				if (titles[0] == sortedTitles[0]
					&& titles[1] == sortedTitles[1]
					&& titles[2] == sortedTitles[2])
				{
					break;
				}
			}

			_view.WatchedMovies.Insert(i, movie);
		}

		private void GetUserRatingsOfWatchedMovies(ArrayList imdbIds, ArrayList userRatings)
		{
			imdbIds.Clear();
			userRatings.Clear();

			foreach (WatchedMovie movie in _view.WatchedMovies)
			{
				imdbIds.Add(movie.ImdbID);
				userRatings.Add(movie.UserRating);
			}
		}

		private void HandlePostRequests(DetailsForm detailsView, Movie movie)
		{
			if (detailsView.IsNewRatingRequested)
			{
				WatchedMovie newMovie = new WatchedMovie(movie.ImdbID, movie.Title, movie.Year,
					movie.Type, movie.PosterURL, movie.Runtime, movie.ImdbRating, movie.Plot,
					movie.Released, movie.Actors, movie.Writer, movie.Director, movie.Genre,
					movie.Country, detailsView.UserRating);
				AddWatchedMovieWithImmediateSort(newMovie);

				RefreshWatchedMovieList();
				RunServiceJob_SaveUserData();
			}
			else if (detailsView.IsRemovalRequested)
			{
				foreach (WatchedMovie watchedMovie in _view.WatchedMovies)
				{
					if (watchedMovie.ImdbID == movie.ImdbID)
					{
						_view.WatchedMovies.Remove(watchedMovie);
						break;
					}
				}

				RefreshWatchedMovieList();
				RunServiceJob_SaveUserData();
			}
		}

		private void OverrideMovieList(ArrayList newMovies)
		{
			_view.Movies.Clear();
			_view.MovieListView.Clear();

			_view.Movies.AddRange(newMovies);

			foreach (Movie movie in _view.Movies)
			{
				_view.MovieListView.Items.Add(
					new ListViewItem(new string[] { movie.Title, movie.ImdbID }));
			}

			_view.WindowTitle = _view.Movies.Count == 1
				? _rm.GetString("AppNameWithOneResultWindowTitle")
				: string.Format(_rm.GetString("AppNameWithResultsWindowTitle"), _view.Movies.Count);
		}

		private void RefreshWatchedMovieList()
		{
			_view.WatchedMovieListView.Clear();

			foreach (WatchedMovie movie in _view.WatchedMovies)
			{
				string visibleName = string.Format(_rm.GetString("WatchedMovieVisibleName"),
					movie.Title, movie.ImdbRating, movie.UserRating, movie.Runtime);

				_view.WatchedMovieListView.Items.Add(
					new ListViewItem(new string[] { visibleName, movie.ImdbID }));
			}
		}

		private DateTime RetrieveLinkerTimestamp()
		{
			string filePath = Assembly.GetCallingAssembly().GetName().CodeBase
				.Replace("file:///", "");
			const int c_PeHeaderOffset = 60;
			const int c_LinkerTimestampOffset = 8;
			byte[] b = new byte[2048];
			Stream s = null;

			try
			{
				s = new FileStream(filePath, FileMode.Open, FileAccess.Read);
				s.Read(b, 0, 2048);
			}
			finally
			{
				if (s != null)
				{
					s.Close();
				}
			}

			int i = BitConverter.ToInt32(b, c_PeHeaderOffset);
			int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);

			DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
			dt = dt.AddSeconds(secondsSince1970);
			dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);

			return dt;
		}

		private void ReturnWatchedMoviesAvgStats(ref decimal averageImdbRating,
			ref decimal averageUserRating, ref decimal averageRuntime, int watchedMoviesCount)
		{
			if (watchedMoviesCount > 0)
			{
				averageImdbRating = averageImdbRating / watchedMoviesCount;
				averageUserRating = averageUserRating / watchedMoviesCount;
				averageRuntime = Math.Round(averageRuntime / watchedMoviesCount);
			}
		}

		private void ReturnWatchedMoviesSumStats(out decimal totalImdbRating,
			out decimal totalUserRating, out decimal totalRuntime, out int imdbRatingMoviesCount,
			out int userRatingMoviesCount, out int runtimeMoviesCount)
		{
			totalImdbRating = 0;
			totalUserRating = 0;
			totalRuntime = 0;
			imdbRatingMoviesCount = 0;
			userRatingMoviesCount = 0;
			runtimeMoviesCount = 0;

			NumberFormatInfo apiNumberFormatInfo = new NumberFormatInfo();
			apiNumberFormatInfo.NumberDecimalSeparator = ".";

			foreach (WatchedMovie movie in _view.WatchedMovies)
			{
				try
				{
					totalImdbRating += decimal.Parse(movie.ImdbRating, apiNumberFormatInfo);
					imdbRatingMoviesCount++;
				}
				catch (FormatException)
				{
				}

				try
				{
					totalUserRating += Convert.ToDecimal(movie.UserRating);
					userRatingMoviesCount++;
				}
				catch (FormatException)
				{
				}

				try
				{
					totalRuntime += decimal.Parse(movie.Runtime.Split(' ')[0]);
					runtimeMoviesCount++;
				}
				catch (FormatException)
				{
				}
			}
		}

		private void ShowDetailsDialog(Movie movie)
		{
			ArrayList watchedImdbIds = new ArrayList();
			ArrayList userRatings = new ArrayList();
			GetUserRatingsOfWatchedMovies(watchedImdbIds, userRatings);
			string movieDetailsInnerText = _rm.GetString("MovieDetailsInnerText");
			string removeFromListButtonText = _rm.GetString("RemoveFromListButton");

			DetailsForm detailsView = new DetailsForm(movie, watchedImdbIds, userRatings,
				movieDetailsInnerText, removeFromListButtonText);
			PosterService posterService = new PosterService();
			DetailsPresenter detailsPresenter = new DetailsPresenter(_rm, detailsView, posterService);

			detailsView.OnClosing += new EventHandler(OnDetailsViewClosing);
			_view.FormEnabled = false;
			detailsView.Show();
		}

		private string ValidateSearchPhrase(string searchPhrase)
		{
			string validationMessageString = string.Empty;

			if (searchPhrase.Length == 0)
			{
				validationMessageString = "EnterSearchPhraseMessage";
			}
			else if (searchPhrase.Length < 3)
			{
				validationMessageString = "SearchPhraseTooShortMessage";
			}
			else if (searchPhrase.IndexOf('&') > -1
				|| searchPhrase.IndexOf('?') > -1
				|| searchPhrase.IndexOf('%') > -1
				|| searchPhrase.IndexOf(';') > -1
				|| searchPhrase.IndexOf('\\') > -1
				|| searchPhrase.IndexOf('/') > -1)
			{
				validationMessageString = "SearchPhraseWithInvalidCharsMessage";
			}

			return validationMessageString;
		}

		#endregion Helper Functions

		#region Service Job Runners

		private bool RunServiceJob_FetchMovieDetails(Movie movie)
		{
			bool isFetched = false;

			_view.SetTemporaryWindowTitle(_rm.GetString("AppNameWithDownloadingMovieDetailsWindowTitle"));
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				isFetched = _movieService.FetchMovieDetails(movie);
			}
			catch (Exception ex)
			{
				string errorMessage = string.Format(_rm.GetString("ErrorFetchingMovieDetailsMessage"),
					ex.ToString());
				MessageBox.Show(errorMessage, _rm.GetString("Error"), MessageBoxButtons.OK,
					MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
			}
			finally
			{
				_view.RestoreOriginalWindowTitle();
				Cursor.Current = Cursors.Default;
			}

			return isFetched;
		}

		private ArrayList RunServiceJob_FetchMovies(string searchPhrase)
		{
			ArrayList fetchedMovies = null;

			_view.SetTemporaryWindowTitle(_rm.GetString("AppNameWithSearchingWindowTitle"));
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				fetchedMovies = _movieService.FetchMovies(searchPhrase);
			}
			catch (Exception ex)
			{
				string errorMessage = string.Format(_rm.GetString("ErrorFetchingMoviesMessage"),
					ex.ToString());
				MessageBox.Show(errorMessage, _rm.GetString("Error"), MessageBoxButtons.OK,
					MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
			}
			finally
			{
				_view.RestoreOriginalWindowTitle();
				Cursor.Current = Cursors.Default;
			}

			return fetchedMovies;
		}

		private bool RunServiceJob_LoadUserData()
		{
			bool isLoaded = false;

			try
			{
				if (File.Exists(GlobalHelper.CombineWithAppDirectoryPath(
					GlobalConstants.UserData_WatchedMoviesFileName)))
				{
					isLoaded = _userDataService.LoadUserData(_view.WatchedMovies);
				}
			}
			catch (Exception ex)
			{
				string errorMessage = string.Format(_rm.GetString("ErrorLoadingUserDataMessage"),
					ex.ToString());
				MessageBox.Show(errorMessage, _rm.GetString("Error"), MessageBoxButtons.OK,
					MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
			}

			return isLoaded;
		}

		private void RunServiceJob_SaveUserData()
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				_userDataService.SaveUserData(_view.WatchedMovies);
			}
			catch (Exception ex)
			{
				string errorMessage = string.Format(_rm.GetString("ErrorSavingUserDataMessage"),
					GlobalConstants.UserData_WatchedMoviesFileName, ex.ToString());
				MessageBox.Show(errorMessage, _rm.GetString("Error"), MessageBoxButtons.OK,
					MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		#endregion Service Job Runners

		#region Event Handlers

		private void OnAbout(object sender, EventArgs e)
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;

			MessageBox.Show(string.Format(_rm.GetString("AboutMessage"), version,
				GlobalConstants.Copyright, string.Format("{0} UTC",
				RetrieveLinkerTimestamp().ToUniversalTime())),
				_rm.GetString("AboutProgram"));
		}

		private void OnDetailsViewClosing(object sender, EventArgs e)
		{
			_view.FormEnabled = true;
			DetailsForm detailsView = (DetailsForm)sender;
			Movie movie = detailsView.Movie;
			HandlePostRequests(detailsView, movie);
		}

		private void OnFormLoad(object sender, EventArgs e)
		{
			if (RunServiceJob_LoadUserData())
			{
				RefreshWatchedMovieList();
			}
		}

		private void OnMovieItemDoubleClick(object sender, EventArgs e)
		{
			if (_view.Movies.Count == _view.MovieListView.Items.Count)
			{
				Movie selectedMovie = (Movie)_view.Movies[_view.MovieListView.SelectedIndices[0]];

				if (selectedMovie.AreDetailsAvailable()
					|| RunServiceJob_FetchMovieDetails(selectedMovie))
				{
					ShowDetailsDialog(selectedMovie);
				}
			}
		}

		private void OnSearch(object sender, EventArgs e)
		{
			string trimmedSearchPhrase = _view.SearchPhrase.Trim();
			string validationMessageString = ValidateSearchPhrase(trimmedSearchPhrase);

			if (validationMessageString == string.Empty)
			{
				ArrayList fetchedMovies = RunServiceJob_FetchMovies(trimmedSearchPhrase);

				if (fetchedMovies != null)
				{
					OverrideMovieList(fetchedMovies);
				}
			}
			else
			{
				MessageBox.Show(_rm.GetString(validationMessageString), _rm.GetString("Warning"),
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
					MessageBoxDefaultButton.Button1);
			}
		}

		private void OnWatchedMovieItemDoubleClick(object sender, EventArgs e)
		{
			if (_view.WatchedMovies.Count == _view.WatchedMovieListView.Items.Count)
			{
				Movie selectedMovie =
					(Movie)_view.WatchedMovies[_view.WatchedMovieListView.SelectedIndices[0]];

				if (selectedMovie.AreDetailsAvailable()
					|| RunServiceJob_FetchMovieDetails(selectedMovie))
				{
					ShowDetailsDialog(selectedMovie);
				}
			}
		}

		private void OnWatchedMoviesClick(object sender, EventArgs e)
		{
			int watchedMoviesCount = _view.WatchedMovies.Count;

			decimal averageImdbRating;
			decimal averageUserRating;
			decimal averageRuntime;
			int imdbRatingMoviesCount;
			int userRatingMoviesCount;
			int runtimeMoviesCount;

			ReturnWatchedMoviesSumStats(out averageImdbRating, out averageUserRating,
				out averageRuntime, out imdbRatingMoviesCount, out userRatingMoviesCount,
				out runtimeMoviesCount);

			ReturnWatchedMoviesAvgStats(ref averageImdbRating, ref averageUserRating,
				ref averageRuntime, watchedMoviesCount);

			MessageBox.Show(string.Format(_rm.GetString("WatchedMovieStatsMessage"),
				watchedMoviesCount, averageImdbRating.ToString("0.00"), imdbRatingMoviesCount,
				averageUserRating.ToString("0.00"), userRatingMoviesCount,
				averageRuntime, runtimeMoviesCount),
				_rm.GetString("WatchedMovieStats"), MessageBoxButtons.OK,
				MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
		}

		#endregion Event Handlers
	}
}
