using System.Collections;

namespace Pocketable_Popcorn.NET.Services.Interfaces
{
	public interface IUserDataService
	{
		bool LoadUserData(ArrayList watchedMovies);
		void SaveUserData(ArrayList watchedMovies);
	}
}
