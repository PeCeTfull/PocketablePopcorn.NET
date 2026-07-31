using Pocketable_Popcorn.NET.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pocketable_Popcorn.NET.Views.Interfaces
{
	public interface IDetailsView
	{
		event EventHandler OnClosing;
		event EventHandler OnPosterRequest;
		event EventHandler OnRateAndAdd;
		event EventHandler OnRemove;

		bool IsNewRatingRequested { get; set; }
		bool IsRemovalRequested { get; set; }
		Movie Movie { get; }
		short UserRating { get; set; }

		void ApplyPosterToView(Bitmap newPoster);
		void RequestRatingAndClose();
		void RequestRemovalAndClose();
		void SetPosterMessage(string newMessage);
	}
}
