using Pocketable_Popcorn.NET.GlobalDelegates;
using Pocketable_Popcorn.NET.Workers.Interfaces;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Resources;

namespace Pocketable_Popcorn.NET.Workers
{
	public class FetchPosterWorker : IWorker
	{
		private readonly ResourceManager _rm;
		private readonly string _posterURL;
		private readonly BitmapTakingVoidDelegate _posterServicePostProcess;
		private readonly StringTakingVoidDelegate _posterServiceCatchProcess;

		public FetchPosterWorker(ResourceManager rm, string posterURL,
			BitmapTakingVoidDelegate posterServicePostProcess,
			StringTakingVoidDelegate posterServiceCatchProcess)
		{
			_rm = rm;
			_posterURL = posterURL;
			_posterServicePostProcess = posterServicePostProcess;
			_posterServiceCatchProcess = posterServiceCatchProcess;
		}

		private void InvokePosterServiceCatchProcess()
		{
			try
			{
				_posterServiceCatchProcess(_rm.GetString("PosterCurrentlyUnavailable"));
			}
			catch (ObjectDisposedException)
			{
			}
		}

		private void InvokePosterServicePostProcess(Bitmap bitmap)
		{
			try
			{
				_posterServicePostProcess(bitmap);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		public void Run()
		{
			HttpWebResponse response = null;

			try
			{
				string fullURL = "http://web.archive.org/web/9999if_/" + _posterURL;

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullURL);
				request.Method = "GET";
				response = (HttpWebResponse)request.GetResponse();

				Bitmap bitmap = new Bitmap(response.GetResponseStream());

				InvokePosterServicePostProcess(bitmap);
			}
			catch (WebException ex)
			{
				if (ex.Response != null)
				{
					ex.Response.Close();
				}

				InvokePosterServiceCatchProcess();
			}
			catch (Exception)
			{
				InvokePosterServiceCatchProcess();
			}
			finally
			{
				if (response != null)
				{
					response.Close();
				}
			}
		}
	}
}
