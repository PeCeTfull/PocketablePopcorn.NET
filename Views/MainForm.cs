using Pocketable_Popcorn.NET.Models;
using Pocketable_Popcorn.NET.Views.Interfaces;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Pocketable_Popcorn.NET.Views
{
	public class MainForm : Form, IMainView
	{
		private System.Windows.Forms.TextBox txtInputMovie;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Button btnMore;
		private System.Windows.Forms.ListView lstFoundMovies;
		private System.Windows.Forms.ListView lstWatchedMovies;
		private System.Windows.Forms.ContextMenu ctmMore;
		private System.Windows.Forms.MenuItem mniWatchedMovies;
		private System.Windows.Forms.MenuItem mniSeparator;
		private System.Windows.Forms.MenuItem mniAbout;
		private System.Windows.Forms.MenuItem mniExit;

		private readonly ArrayList _movies = new ArrayList();
		private string _originalWindowTitle;
		private readonly ArrayList _watchedMovies = new ArrayList();

		public event EventHandler OnAbout;
		public event EventHandler OnFormLoad;
		public event EventHandler OnMovieItemDoubleClick;
		public event EventHandler OnSearch;
		public event EventHandler OnWatchedMovieItemDoubleClick;
		public event EventHandler OnWatchedMoviesClick;

		public bool FormEnabled
		{
			get { return Enabled; }
			set { Enabled = value; }
		}

		public ListView MovieListView
		{
			get { return lstFoundMovies; }
		}

		public ArrayList Movies
		{
			get { return _movies; }
		}

		public string OriginalWindowTitle
		{
			get { return _originalWindowTitle; }
			set { _originalWindowTitle = value; }
		}

		public string SearchPhrase
		{
			get { return txtInputMovie.Text; }
		}

		public ListView WatchedMovieListView
		{
			get { return lstWatchedMovies; }
		}

		public ArrayList WatchedMovies
		{
			get { return _watchedMovies; }
		}

		public string WindowTitle
		{
			get { return Text; }
			set { Text = value; }
		}
	
		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			EstablishLocationAndSize();
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
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem();
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.txtInputMovie = new System.Windows.Forms.TextBox();
			this.btnSearch = new System.Windows.Forms.Button();
			this.btnMore = new System.Windows.Forms.Button();
			this.ctmMore = new System.Windows.Forms.ContextMenu();
			this.mniWatchedMovies = new System.Windows.Forms.MenuItem();
			this.mniSeparator = new System.Windows.Forms.MenuItem();
			this.mniAbout = new System.Windows.Forms.MenuItem();
			this.mniExit = new System.Windows.Forms.MenuItem();
			this.lstFoundMovies = new System.Windows.Forms.ListView();
			this.lstWatchedMovies = new System.Windows.Forms.ListView();
			// 
			// txtInputMovie
			// 
			this.txtInputMovie.MaxLength = 167;
			this.txtInputMovie.Size = new System.Drawing.Size(268, 20);
			this.txtInputMovie.Text = "";
			this.txtInputMovie.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInputMovie_KeyDown);
			this.txtInputMovie.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInputMovie_KeyPress);
			// 
			// btnSearch
			// 
			this.btnSearch.Location = new System.Drawing.Point(268, 0);
			this.btnSearch.Size = new System.Drawing.Size(64, 20);
			this.btnSearch.Text = "&Search";
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnMore
			// 
			this.btnMore.Location = new System.Drawing.Point(332, 0);
			this.btnMore.Size = new System.Drawing.Size(24, 20);
			this.btnMore.Text = "&...";
			this.btnMore.Click += new System.EventHandler(this.btnMore_Click);
			// 
			// ctmMore
			// 
			this.ctmMore.MenuItems.Add(this.mniWatchedMovies);
			this.ctmMore.MenuItems.Add(this.mniSeparator);
			this.ctmMore.MenuItems.Add(this.mniAbout);
			this.ctmMore.MenuItems.Add(this.mniExit);
			// 
			// mniWatchedMovies
			// 
			this.mniWatchedMovies.Text = "Movies you &watched";
			this.mniWatchedMovies.Click += new System.EventHandler(this.mniWatchedMovies_Click);
			// 
			// mniSeparator
			// 
			this.mniSeparator.Text = "-";
			// 
			// mniAbout
			// 
			this.mniAbout.Text = "&About...";
			this.mniAbout.Click += new System.EventHandler(this.mniAbout_Click);
			// 
			// mniExit
			// 
			this.mniExit.Text = "E&xit";
			this.mniExit.Click += new System.EventHandler(this.mniExit_Click);
			// 
			// lstFoundMovies
			// 
			listViewItem1.Text = "The search results will appear here.";
			this.lstFoundMovies.Items.Add(listViewItem1);
			this.lstFoundMovies.Location = new System.Drawing.Point(0, 20);
			this.lstFoundMovies.Size = new System.Drawing.Size(104, 96);
			this.lstFoundMovies.View = System.Windows.Forms.View.List;
			this.lstFoundMovies.ItemActivate += new System.EventHandler(this.lstFoundMovies_ItemActivate);
			// 
			// lstWatchedMovies
			// 
			listViewItem2.Text = "Your watched movies will appear here.";
			this.lstWatchedMovies.Items.Add(listViewItem2);
			this.lstWatchedMovies.Location = new System.Drawing.Point(104, 20);
			this.lstWatchedMovies.Size = new System.Drawing.Size(104, 96);
			this.lstWatchedMovies.View = System.Windows.Forms.View.List;
			this.lstWatchedMovies.ItemActivate += new System.EventHandler(this.lstWatchedMovies_ItemActivate);
			// 
			// MainForm
			// 
			this.ClientSize = new System.Drawing.Size(354, 181);
			this.Controls.Add(this.txtInputMovie);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.btnMore);
			this.Controls.Add(this.lstFoundMovies);
			this.Controls.Add(this.lstWatchedMovies);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Text = "Pocketable Popcorn.NET";
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.Load += new System.EventHandler(this.MainForm_Load);

		}
		#endregion

		public void RestoreOriginalWindowTitle()
		{
			Text = _originalWindowTitle;
			_originalWindowTitle = null;
		}

		public void SetTemporaryWindowTitle(string tempWindowTitle)
		{
			_originalWindowTitle = Text;
			Text = tempWindowTitle;
		}

		#region Helper Functions

		private void EstablishLocationAndSize()
		{
			Location = new Point(16, 16);
			Width = Screen.PrimaryScreen.WorkingArea.Width - 32;
			Height = Screen.PrimaryScreen.WorkingArea.Height - 32;
		}

		#endregion Helper Functions

		#region Event Handlers

		private void btnMore_Click(object sender, System.EventArgs e)
		{
			ctmMore.Show(this, new Point(btnMore.Left, btnMore.Top + btnMore.Height));
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			if (OnSearch != null)
			{
				OnSearch(sender, e);
			}
		}

		private void lstFoundMovies_ItemActivate(object sender, System.EventArgs e)
		{
			if (OnMovieItemDoubleClick != null)
			{
				OnMovieItemDoubleClick(sender, e);
			}
		}

		private void lstWatchedMovies_ItemActivate(object sender, System.EventArgs e)
		{
			if (OnWatchedMovieItemDoubleClick != null)
			{
				OnWatchedMovieItemDoubleClick(sender, e);
			}
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			EstablishLocationAndSize();

			if (OnFormLoad != null)
			{
				OnFormLoad(sender, e);
			}

			txtInputMovie.Focus();
		}

		private void MainForm_Resize(object sender, System.EventArgs e)
		{
			btnMore.Left = this.ClientSize.Width - btnMore.Width;
			btnSearch.Left = this.ClientSize.Width - btnMore.Width - btnSearch.Width;

			txtInputMovie.Width = this.ClientSize.Width - btnMore.Width - btnSearch.Width;

			lstFoundMovies.Width = this.ClientSize.Width / 2;
			lstFoundMovies.Height = this.ClientSize.Height - txtInputMovie.Height;

			lstWatchedMovies.Left = this.ClientSize.Width / 2;
			lstWatchedMovies.Width = this.ClientSize.Width / 2;
			lstWatchedMovies.Height = this.ClientSize.Height - txtInputMovie.Height;
		}

		private void mniAbout_Click(object sender, System.EventArgs e)
		{
			if (OnAbout != null)
			{
				OnAbout(sender, e);
			}
		}

		private void mniExit_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void mniWatchedMovies_Click(object sender, System.EventArgs e)
		{
			if (OnWatchedMoviesClick != null)
			{
				OnWatchedMoviesClick(sender, e);
			}
		}

		private void txtInputMovie_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && OnSearch != null)
			{
				OnSearch(sender, e);
			}
		}

		private void txtInputMovie_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				e.Handled = true;
			}
		}

		#endregion Event Handlers
	}
}
