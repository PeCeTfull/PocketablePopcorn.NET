using System;

namespace Pocketable_Popcorn.NET.Models
{
	public class Movie
	{
		protected string _imdbID;
		protected string _title;
		protected string _year;
		protected string _type;
		protected string _posterURL;
		protected string _runtime;
		protected string _imdbRating;
		protected string _plot;
		protected string _released;
		protected string _actors;
		protected string _writer;
		protected string _director;
		protected string _genre;
		protected string _country;

		public string ImdbID
		{
			get { return _imdbID; }
			set { _imdbID = value; }
		}
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}
		public string Year
		{
			get { return _year; }
			set { _year = value; }
		}
		public string Type
		{
			get { return _type; }
			set { _type = value; }
		}
		public string PosterURL
		{
			get { return _posterURL; }
			set { _posterURL = value; }
		}
		public string Runtime
		{
			get { return _runtime; }
			set { _runtime = value; }
		}
		public string ImdbRating
		{
			get { return _imdbRating; }
			set { _imdbRating = value; }
		}
		public string Plot
		{
			get { return _plot; }
			set { _plot = value; }
		}
		public string Released
		{
			get { return _released; }
			set { _released = value; }
		}
		public string Actors
		{
			get { return _actors; }
			set { _actors = value; }
		}
		public string Writer
		{
			get { return _writer; }
			set { _writer = value; }
		}
		public string Director
		{
			get { return _director; }
			set { _director = value; }
		}
		public string Genre
		{
			get { return _genre; }
			set { _genre = value; }
		}
		public string Country
		{
			get { return _country; }
			set { _country = value; }
		}

		public Movie()
		{
		}

		public Movie(string imdbID, string title, string year, string type, string posterURL)
		{
			_imdbID = imdbID;
			_title = title;
			_year = year;
			_type = type;
			_posterURL = posterURL;
		}

		public bool AreDetailsAvailable()
		{
			return _plot != null;
		}
	}
}
