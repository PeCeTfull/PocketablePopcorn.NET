using Pocketable_Popcorn.NET.Presenters;
using Pocketable_Popcorn.NET.Services;
using Pocketable_Popcorn.NET.Views;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;

namespace Pocketable_Popcorn.NET
{
	public class Startup
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static void Main()
		{
			ResourceManager rm = new ResourceManager("Pocketable_Popcorn.NET.AppResources",
				Assembly.GetExecutingAssembly());

			if (GlobalConstants.EncryptedOmdbApiKey == "CHANGE_ME")
			{
				MessageBox.Show(rm.GetString("InvalidApiKeyWillTerminateMessage"),
					rm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Hand,
					MessageBoxDefaultButton.Button1);
				return;
			}

			MainForm view = new MainForm();
			MovieService movieService = new MovieService();
			UserDataService userDataService = new UserDataService();
			MainPresenter presenter = new MainPresenter(rm, view, movieService, userDataService);

			Application.Run(view);
		}
	}
}
