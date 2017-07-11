using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XRousseSudoku
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {

            InitializeComponent();
            BackgroundColor = Color.FromHex("#e5c15e");

            Content = new Label
            {
                Text = "XRousse Sudoku",
                FontSize = 40,
                FontFamily = "Comic Sans MS",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex("#FFF"),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
        }  
	}
}
