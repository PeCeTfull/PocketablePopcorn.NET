using Pocketable_Popcorn.NET.Models;
using System.Collections;

namespace Pocketable_Popcorn.NET.Services.Interfaces
{
	public interface IMovieService
	{
		ArrayList FetchMovies(string searchPhrase);
		bool FetchMovieDetails(Movie movie);
	}
}
