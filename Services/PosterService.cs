using Pocketable_Popcorn.NET.Services.Interfaces;
using Pocketable_Popcorn.NET.Workers.Interfaces;
using System.Threading;

namespace Pocketable_Popcorn.NET.Services
{
	public class PosterService : IPosterService
	{
		private Thread _fetchPosterThread;

		public void FetchPoster(IWorker worker)
		{
			_fetchPosterThread = new Thread(new ThreadStart(worker.Run));
			_fetchPosterThread.Start();
		}
	}
}
