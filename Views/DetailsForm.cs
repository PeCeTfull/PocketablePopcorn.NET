using Pocketable_Popcorn.NET.Models;
using Pocketable_Popcorn.NET.Views.Interfaces;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Pocketable_Popcorn.NET.Views
{
	public class DetailsForm : Form, IDetailsView
	{
		private System.Windows.Forms.PictureBox pctPoster;
		private System.Windows.Forms.TextBox txtDetails;
		private System.Windows.Forms.Label lblPosterMessage;
		private System.Windows.Forms.NumericUpDown numUserRating;
		private System.Windows.Forms.Button btnAction;

		private bool _isNewRatingRequested;
		private bool _isRemovalRequested;
		private Movie _movie;
		private readonly string _movieDetailsInnerText;
		private readonly string _removeFromListButtonText;

		public event EventHandler OnClosing;
		public event EventHandler OnPosterRequest;
		public event EventHandler OnRateAndAdd;
		public event EventHandler OnRemove;

		public bool IsNewRatingRequested
		{
			get { return _isNewRatingRequested; }
			set { _isNewRatingRequested = value; }
		}

		public bool IsRemovalRequested
		{
			get { return _isRemovalRequested; }
			set { _isRemovalRequested = value; }
		}

		public Movie Movie
		{
			get { return _movie; }
		}

		public short UserRating
		{
			get { return Convert.ToInt16(numUserRating.Value); }
			set { numUserRating.Value = value; }
		}
	
		public DetailsForm(Movie movie, ArrayList watchedImdbIds, ArrayList userRatings,
			string movieDetailsInnerText, string removeFromListButtonText)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			numUserRating.KeyDown += new KeyEventHandler(Global_KeyDown);
			btnAction.KeyDown += new KeyEventHandler(Global_KeyDown);

			_movie = movie;
			_movieDetailsInnerText = movieDetailsInnerText;
			_removeFromListButtonText = removeFromListButtonText;
			SetMovieDetails(watchedImdbIds, userRatings);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pctPoster = new System.Windows.Forms.PictureBox();
			this.txtDetails = new System.Windows.Forms.TextBox();
			this.lblPosterMessage = new System.Windows.Forms.Label();
			this.numUserRating = new System.Windows.Forms.NumericUpDown();
			this.btnAction = new System.Windows.Forms.Button();
			// 
			// pctPoster
			// 
			this.pctPoster.Size = new System.Drawing.Size(128, 184);
			// 
			// txtDetails
			// 
			this.txtDetails.Location = new System.Drawing.Point(128, 0);
			this.txtDetails.Multiline = true;
			this.txtDetails.ReadOnly = true;
			this.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDetails.Size = new System.Drawing.Size(164, 164);
			this.txtDetails.Text = "Movie details";
			this.txtDetails.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Global_KeyDown);
			// 
			// lblPosterMessage
			// 
			this.lblPosterMessage.Location = new System.Drawing.Point(8, 8);
			this.lblPosterMessage.Size = new System.Drawing.Size(112, 168);
			this.lblPosterMessage.Text = "Loading poster...";
			this.lblPosterMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// numUserRating
			// 
			this.numUserRating.Location = new System.Drawing.Point(128, 164);
			this.numUserRating.Maximum = new System.Decimal(new int[] {
																		  10,
																		  0,
																		  0,
																		  0});
			this.numUserRating.Size = new System.Drawing.Size(44, 20);
			// 
			// btnAction
			// 
			this.btnAction.Location = new System.Drawing.Point(172, 164);
			this.btnAction.Size = new System.Drawing.Size(120, 20);
			this.btnAction.Text = "&Rate && Add to List";
			this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			// 
			// DetailsForm
			// 
			this.ClientSize = new System.Drawing.Size(290, 183);
			this.Controls.Add(this.lblPosterMessage);
			this.Controls.Add(this.pctPoster);
			this.Controls.Add(this.txtDetails);
			this.Controls.Add(this.numUserRating);
			this.Controls.Add(this.btnAction);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Text = "Movie";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.DetailsForm_Closing);
			this.Load += new System.EventHandler(this.DetailsForm_Load);

		}
		#endregion

		public void ApplyPosterToView(Bitmap newPoster)
		{
			Bitmap resizedPoster = ResizeBitmap(newPoster,
				pctPoster.Width, pctPoster.Height);
			pctPoster.Image = resizedPoster;

			lblPosterMessage.Visible = false;
		}

		public void RequestRatingAndClose()
		{
			IsNewRatingRequested = true;
			Close();
		}

		public void RequestRemovalAndClose()
		{
			if (!numUserRating.Enabled)
			{
				IsRemovalRequested = true;
				Close();
			}
		}

		public void SetPosterMessage(string newMessage)
		{
			lblPosterMessage.Text = newMessage;
		}

		#region Helper Functions

		private void CenterFormWindow()
		{
			Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;

			Left = (workingArea.Width - Width) / 2;
			Top = (workingArea.Height - Height) / 2;
		}

		private void ChangeActionButtonToRemove()
		{
			numUserRating.Enabled = false;
			btnAction.Text = _removeFromListButtonText;
		}

		private short GetUserRatingIfWatched(ArrayList watchedImdbIds, ArrayList userRatings)
		{
			short userRating = 0;

			for (int i = 0; i < watchedImdbIds.Count; i++)
			{
				if ((string)watchedImdbIds[i] == _movie.ImdbID)
				{
					userRating = (short)userRatings[i];
					break;
				}
			}

			return userRating;
		}

		private Bitmap ResizeBitmap(Bitmap sourceBitmap, int newWidth, int newHeight)
		{
			Bitmap targetBitmap = new Bitmap(newWidth, newHeight);

			using (Graphics graphics = Graphics.FromImage(targetBitmap))
			{
				Rectangle targetRect = new Rectangle(0, 0, newWidth, newHeight);
				Rectangle sourceRect = new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height);

				graphics.DrawImage(sourceBitmap, targetRect, sourceRect, GraphicsUnit.Pixel);
			}

			return targetBitmap;
		}

		private void SetMovieDetails(ArrayList watchedImdbIds, ArrayList userRatings)
		{
			UserRating = GetUserRatingIfWatched(watchedImdbIds, userRatings);
			if (UserRating > 0)
			{
				ChangeActionButtonToRemove();
			}

			string movieType = string.Format("{0}{1}",
				_movie.Type[0].ToString().ToUpper(), _movie.Type.Substring(1));

			Text = string.Format("{0}: {1}", movieType, _movie.Title);
			txtDetails.Text = string.Format(_movieDetailsInnerText,
				_movie.Title, _movie.Released, _movie.Runtime, _movie.Genre, _movie.Country,
				_movie.Plot, _movie.Actors, _movie.Writer, _movie.Director, _movie.ImdbRating);
		}

		#endregion Helper Functions

		#region Event Handlers

		private void btnAction_Click(object sender, System.EventArgs e)
		{
			if (numUserRating.Enabled)
			{
				if (OnRateAndAdd != null)
				{
					OnRateAndAdd(sender, e);
				}
			}
			else
			{
				if (OnRemove != null)
				{
					OnRemove(sender, e);
				}
			}
		}

		private void DetailsForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (OnClosing != null)
			{
				OnClosing(sender, e);
			}
		}

		private void DetailsForm_Load(object sender, System.EventArgs e)
		{
			CenterFormWindow();

			if (OnPosterRequest != null)
			{
				OnPosterRequest(sender, e);
			}

			txtDetails.Focus();
		}

		private void Global_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Insert:
					if (numUserRating.Enabled)
					{
						if (OnRateAndAdd != null)
						{
							OnRateAndAdd(sender, e);
							txtDetails.Focus();
						}
					}
					break;
				case Keys.Delete:
					if (!numUserRating.Enabled)
					{
						if (OnRemove != null)
						{
							OnRemove(sender, e);
							txtDetails.Focus();
						}
					}
					break;
				case Keys.Escape:
					Close();
					break;
				default:
					base.OnKeyDown(e);
					break;
			}
		}

		#endregion Event Handlers
	}
}
