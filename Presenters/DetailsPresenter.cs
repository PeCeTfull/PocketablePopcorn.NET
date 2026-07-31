using Pocketable_Popcorn.NET.GlobalDelegates;
using Pocketable_Popcorn.NET.Services.Interfaces;
using Pocketable_Popcorn.NET.Views.Interfaces;
using Pocketable_Popcorn.NET.Workers;
using Pocketable_Popcorn.NET.Workers.Interfaces;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace Pocketable_Popcorn.NET.Presenters
{
	public class DetailsPresenter
	{
		private readonly ResourceManager _rm;
		private readonly IDetailsView _view;
		private readonly IPosterService _posterService;

		public DetailsPresenter(ResourceManager rm, IDetailsView view, IPosterService posterService)
		{
			_rm = rm;
			_view = view;
			_posterService = posterService;

			_view.OnPosterRequest += new EventHandler(OnPosterRequest);
			_view.OnRateAndAdd += new EventHandler(OnRateAndAdd);
			_view.OnRemove += new EventHandler(OnRemove);
		}

		#region Service Job Runners

		private void RunServiceJob_FetchPoster()
		{
			IWorker worker = new FetchPosterWorker(_rm, _view.Movie.PosterURL,
				new BitmapTakingVoidDelegate(_view.ApplyPosterToView),
				new StringTakingVoidDelegate(_view.SetPosterMessage));

			_posterService.FetchPoster(worker);
		}

		#endregion Service Job Runners

		#region Event Handlers

		private void OnPosterRequest(object sender, EventArgs e)
		{
			RunServiceJob_FetchPoster();
		}

		private void OnRateAndAdd(object sender, EventArgs e)
		{
			if (_view.UserRating < 1)
			{
				MessageBox.Show(_rm.GetString("RateMovieFirstMessage"), _rm.GetString("Warning"),
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
					MessageBoxDefaultButton.Button1);
			}
			else
			{
				_view.RequestRatingAndClose();
			}
		}

		private void OnRemove(object sender, EventArgs e)
		{
			DialogResult questionResult = MessageBox.Show(
				_rm.GetString("RemoveFromWatchedMoviesListQuestion"), _rm.GetString("Question"),
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

			if (questionResult == DialogResult.Yes)
			{
				_view.RequestRemovalAndClose();
			}
		}

		#endregion Event Handlers
	}
}
