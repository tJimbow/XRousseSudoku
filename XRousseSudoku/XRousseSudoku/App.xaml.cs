using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace XRousseSudoku
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
            MainPage = new XRousseSudoku.MainPage();
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        public static void ComputeUWCWindowSize(int initialH, out int w, out int h)
        {
            double ratio = 768.0 / 1280.0;
            h = initialH;
            w = (int)(initialH * ratio);
        }
    }
}
