using System;

namespace Pocketable_Popcorn.NET.Models
{
	public class WatchedMovie : Movie
	{
		protected short _userRating;

		public short UserRating
		{
			get { return _userRating; }
			set { _userRating = value; }
		}

		public WatchedMovie()
		{
		}

		public WatchedMovie(string imdbID, string title, string year, string type, string posterURL,
			string runtime, string imdbRating, short userRating)
		{
			_imdbID = imdbID;
			_title = title;
			_year = year;
			_type = type;
			_posterURL = posterURL;
			_runtime = runtime;
			_imdbRating = imdbRating;
			_userRating = userRating;
		}

		public WatchedMovie(string imdbID, string title, string year, string type, string posterURL,
			string runtime, string imdbRating, string plot, string released, string actors,
			string writer, string director, string genre, string country, short userRating)
		{
			_imdbID = imdbID;
			_title = title;
			_year = year;
			_type = type;
			_posterURL = posterURL;
			_runtime = runtime;
			_imdbRating = imdbRating;
			_plot = plot;
			_released = released;
			_actors = actors;
			_writer = writer;
			_director = director;
			_genre = genre;
			_country = country;
			_userRating = userRating;
		}
	}
}
