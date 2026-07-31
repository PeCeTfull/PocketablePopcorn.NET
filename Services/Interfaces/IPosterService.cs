using Pocketable_Popcorn.NET.Workers.Interfaces;

namespace Pocketable_Popcorn.NET.Services.Interfaces
{
	public interface IPosterService
	{
		void FetchPoster(IWorker worker);
	}
}
