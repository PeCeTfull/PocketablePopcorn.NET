using Pocketable_Popcorn.NET.Models;
using Pocketable_Popcorn.NET.Services.Interfaces;
using System;
using System.Collections;
using System.Text;
using System.Xml;

namespace Pocketable_Popcorn.NET.Services
{
	public class UserDataService : IUserDataService
	{
		private const string strWatchedMovies = "WatchedMovies";
		private const string strMovie = "Movie";

		private const string strTitle = "Title";
		private const string strImdbID = "ImdbID";
		private const string strYear = "Year";
		private const string strType = "Type";
		private const string strPosterURL = "PosterURL";
		private const string strRuntime = "Runtime";
		private const string strImdbRating = "ImdbRating";
		private const string strUserRating = "UserRating";

		#region Helper Functions

		private void InitXmlElementValues(out string title, out string imdbID, out string year,
			out string type, out string posterURL, out string runtime, out string imdbRating,
			out short userRating)
		{
			title = string.Empty;
			imdbID = string.Empty;
			year = string.Empty;
			type = string.Empty;
			posterURL = string.Empty;
			runtime = string.Empty;
			imdbRating = string.Empty;
			userRating = 0;
		}

		private void ReadXml(ArrayList watchedMovies)
		{
			XmlTextReader reader = new XmlTextReader(GlobalHelper.CombineWithAppDirectoryPath(
				GlobalConstants.UserData_WatchedMoviesFileName));
			
			string title;
			string imdbID;
			string year;
			string type;
			string posterURL;
			string runtime;
			string imdbRating;
			short userRating;

			InitXmlElementValues(out title, out imdbID, out year, out type, out posterURL,
				out runtime, out imdbRating, out userRating);
			
			while (reader.Read())
			{
				ReadXmlElement(reader, ref title, ref imdbID, ref year, ref type, ref posterURL,
					ref runtime, ref imdbRating, ref userRating);

				if (reader.NodeType == XmlNodeType.EndElement && reader.Name == strMovie)
				{
					WatchedMovie movie = new WatchedMovie(imdbID, title, year, type, posterURL,
						runtime, imdbRating, userRating);
					watchedMovies.Add(movie);

					InitXmlElementValues(out title, out imdbID, out year, out type, out posterURL,
						out runtime, out imdbRating, out userRating);
				}
			}

			reader.Close();
		}

		private void ReadXmlElement(XmlTextReader reader, ref string title, ref string imdbID,
			ref string year, ref string type, ref string posterURL, ref string runtime,
			ref string imdbRating, ref short userRating)
		{
			if (reader.NodeType == XmlNodeType.Element)
			{
				switch (reader.Name)
				{
					case strTitle:
						title = reader.ReadString();
						break;
					case strImdbID:
						imdbID = reader.ReadString();
						break;
					case strYear:
						year = reader.ReadString();
						break;
					case strType:
						type = reader.ReadString();
						break;
					case strPosterURL:
						posterURL = reader.ReadString();
						break;
					case strRuntime:
						runtime = reader.ReadString();
						break;
					case strImdbRating:
						imdbRating = reader.ReadString();
						break;
					case strUserRating:
						userRating = Convert.ToInt16(reader.ReadString());
						break;
				}
			}
		}

		private void WriteMovieElements(XmlTextWriter writer, ArrayList watchedMovies)
		{
			foreach (WatchedMovie movie in watchedMovies)
			{
				writer.WriteStartElement(strMovie);

				writer.WriteElementString(strTitle, movie.Title);
				writer.WriteElementString(strImdbID, movie.ImdbID);
				writer.WriteElementString(strYear, movie.Year);
				writer.WriteElementString(strType, movie.Type);
				writer.WriteElementString(strPosterURL, movie.PosterURL);
				writer.WriteElementString(strRuntime, movie.Runtime);
				writer.WriteElementString(strImdbRating, movie.ImdbRating);
				writer.WriteElementString(strUserRating, movie.UserRating.ToString());

				writer.WriteEndElement();
			}
		}

		private void WriteXml(ArrayList watchedMovies)
		{
			XmlTextWriter writer = new XmlTextWriter(
				GlobalHelper.CombineWithAppDirectoryPath(
				GlobalConstants.UserData_WatchedMoviesFileName),
				Encoding.UTF8);

			writer.WriteStartDocument();
			writer.WriteStartElement(strWatchedMovies);

			WriteMovieElements(writer, watchedMovies);

			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Flush();
			writer.Close();
		}

		#endregion Helper Functions

		public bool LoadUserData(ArrayList watchedMovies)
		{
			ReadXml(watchedMovies);

			return true;
		}

		public void SaveUserData(ArrayList watchedMovies)
		{
			WriteXml(watchedMovies);
		}
	}
}
