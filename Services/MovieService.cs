using Pocketable_Popcorn.NET.Models;
using Pocketable_Popcorn.NET.Services.Interfaces;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace Pocketable_Popcorn.NET.Services
{
	public class MovieService : IMovieService
	{
		#region Helper Functions

		private string DecryptOmdbApiKey()
		{
			StringBuilder output = new StringBuilder();

			for (int i = 0; i < GlobalConstants.EncryptedOmdbApiKey.Length; ++i)
			{
				output.Append((char)
					(GlobalConstants.EncryptedOmdbApiKey[i]
					^ GlobalConstants.EncryptionKey[i % GlobalConstants.EncryptionKey.Length]));
			}

			return output.ToString();
		}

		private HttpWebResponse FetchIdResponse(string movieId)
		{
			HttpWebRequest request =
				(HttpWebRequest)WebRequest.Create("http://www.omdbapi.com/?r=xml&apikey="
				+ DecryptOmdbApiKey()
				+ "&i="
				+ movieId);
			request.Method = "GET";

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			return response;
		}

		private HttpWebResponse FetchSearchResponse(string searchPhrase)
		{
			HttpWebRequest request =
				(HttpWebRequest)WebRequest.Create("http://www.omdbapi.com/?r=xml&apikey="
				+ DecryptOmdbApiKey()
				+ "&s="
				+ searchPhrase);
			request.Method = "GET";

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			return response;
		}

		private void IterateSearchResults(ArrayList movies, XmlNodeList results)
		{
			foreach (XmlElement result in results)
			{
				string title = result.Attributes["title"].InnerText;
				string imdbID = result.Attributes["imdbID"].InnerText;
				string year = result.Attributes["year"].InnerText;
				string type = result.Attributes["type"].InnerText;
				string posterURL = result.Attributes["poster"] != null
					? result.Attributes["poster"].InnerText
					: string.Empty;

				Movie movie = new Movie(imdbID, title, year, type, posterURL);
				movies.Add(movie);
			}
		}

		private XmlNodeList ParseMovies(string xml)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);

			XmlNodeList movies = doc.GetElementsByTagName("movie");

			return movies;
		}

		private XmlNodeList ParseResults(string xml)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);

			XmlNodeList results = doc.GetElementsByTagName("result");

			return results;
		}

		private string ReadContents(HttpWebResponse response)
		{
			string contents;

			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				contents = reader.ReadToEnd();
			}

			response.Close();

			return contents;
		}

		private void UpdateMovieItem(Movie movie, XmlNodeList movies)
		{
			if (movies != null && movies.Count > 0)
			{
				movie.Runtime = movies[0].Attributes["runtime"].InnerText;
				movie.ImdbRating = movies[0].Attributes["imdbRating"].InnerText;
				movie.Plot = movies[0].Attributes["plot"].InnerText;
				movie.Released = movies[0].Attributes["released"].InnerText;
				movie.Actors = movies[0].Attributes["actors"].InnerText;
				movie.Writer = movies[0].Attributes["writer"].InnerText;
				movie.Director = movies[0].Attributes["director"].InnerText;
				movie.Genre = movies[0].Attributes["genre"].InnerText;
				movie.Country = movies[0].Attributes["country"].InnerText;
			}
		}

		#endregion Helper Functions

		public bool FetchMovieDetails(Movie movie)
		{
			HttpWebResponse response = FetchIdResponse(movie.ImdbID);
			string xml = ReadContents(response);
			XmlNodeList movies = ParseMovies(xml);
			UpdateMovieItem(movie, movies);

			return true;
		}

		public ArrayList FetchMovies(string searchPhrase)
		{
			ArrayList movies = new ArrayList();

			HttpWebResponse response = FetchSearchResponse(searchPhrase);
			string xml = ReadContents(response);
			XmlNodeList results = ParseResults(xml);
			IterateSearchResults(movies, results);

			return movies;
		}
	}
}
