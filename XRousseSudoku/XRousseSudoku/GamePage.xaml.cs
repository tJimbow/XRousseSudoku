using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XRousseSudoku
{
	public partial class GamePage : ContentPage
	{
		public GamePage ()
        {
            InitializeComponent();
            Label maintTitleGame = new Label
            {
                // Add options for the title view
                Margin = new Thickness(0, 40, 10, 0),
                Text = "In Game",
                FontSize = 40,
                FontFamily = "Verdana",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex("#000"),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            StackLayout contentGame = new StackLayout
            {
                Children =
                {
                    maintTitleGame,
                    Grid4x4_Test(),
                    generateNumbersLayout(),
                }
            };
            // Build the page
            Content = contentGame;
        }

        public StackLayout generateNumbersLayout()
        {
            StackLayout numbersLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
            };

            for(int i = 1; i<10; i++)
            {
                Button btn = new Button();
                btn.Text = i.ToString();
                numbersLayout.Children.Add(btn);
            }


            return numbersLayout;
        }

        public View Grid4x4_Test()
        {
            // create a grid
            Grid grid = new Grid();

            // add 2 lines and 2 columns 
            // (do not interleave RowDefinitions.Add & ColumnDefinitions.Add calls, this fails !)
            for (int i = 0; i < 2; i++)
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            for (int j = 0; j < 2; j++)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // set content of each cell
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    // each cell contains a layout frame (in expand all mode)
                    Frame frame = new Frame();
                    frame.VerticalOptions = LayoutOptions.FillAndExpand;
                    frame.HorizontalOptions = LayoutOptions.FillAndExpand;
                    //frame.OutlineColor = Color.Red; // only has effect on uwp
                    frame.BackgroundColor = (((i + j) % 2) == 0) ? Color.White : Color.Red;

                    // the frame contains a centered label
                    string label_text = Char.ConvertFromUtf32(65 + i + j * 2);
                    frame.Content = new Label
                    {
                        Text = label_text,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
                    };

                    // add frame to the grid
                    grid.Children.Add(frame, i, j);
                }
            }
            return grid;
        }
    }
}