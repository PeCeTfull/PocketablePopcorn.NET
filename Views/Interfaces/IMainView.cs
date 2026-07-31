using System;
using System.Collections;
using System.Windows.Forms;

namespace Pocketable_Popcorn.NET.Views.Interfaces
{
	public interface IMainView
	{
		event EventHandler OnAbout;
		event EventHandler OnFormLoad;
		event EventHandler OnMovieItemDoubleClick;
		event EventHandler OnSearch;
		event EventHandler OnWatchedMovieItemDoubleClick;
		event EventHandler OnWatchedMoviesClick;

		bool FormEnabled { get; set; }
		ListView MovieListView { get; }
		ArrayList Movies { get; }
		string SearchPhrase { get; }
		ListView WatchedMovieListView { get; }
		ArrayList WatchedMovies { get; }
		string WindowTitle { get; set; }

		void RestoreOriginalWindowTitle();
		void SetTemporaryWindowTitle(string tempWindowTitle);
	}
}
