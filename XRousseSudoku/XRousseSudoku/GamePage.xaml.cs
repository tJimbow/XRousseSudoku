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

            // set bg image
            BackgroundImage = "retina_wood_1024.png";

            // title
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

            // main stack layout
            StackLayout contentGame = new StackLayout
            {
                Children =
                {
                    maintTitleGame,
                    GridTest(),
                    generateNumbersLayout(),
                    GenerateBackToMainMenuButton()
                }
            };

            // Build the page
            Content = contentGame;
        }

        public View GenerateBackToMainMenuButton()
        {
            Button backToMainMenuBtn = new Button
            {
                //Margin = new Thickness(10, 0, 10, 0),
                Text = "Abandonner",
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                BorderWidth = 1,
                BorderRadius = 15,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            backToMainMenuBtn.Clicked += GoBackToMainMenuPage;
            return backToMainMenuBtn;
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

        public View GridTest()
        {
            int N = 9;

            // create a grid
            Grid grid = new Grid();

            // add N lines and N columns 
            // (do not interleave RowDefinitions.Add & ColumnDefinitions.Add calls, this fails !)
            for (int i = 0; i < N; i++)
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            for (int j = 0; j < N; j++)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // set content of each cell
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    string text = "99"; // (i + j * N).ToString();

                    bool useButton = false;
                    if (useButton)
                    {
                        Button button = new Button
                        {
                            Text = text,
                            Font = Font.SystemFontOfSize(NamedSize.Micro),
                            BorderWidth = 1,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                        };
                        grid.Children.Add(button, i, j);
                    }
                    else
                    {
                        // each cell contains a layout frame(in expand all mode)
                        Frame frame = new Frame
                        {
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            BackgroundColor = (((i + j) % 2) == 0) ? Color.White : Color.LightGray
                        };

                        // the frame contains a centered label
                        frame.Content = new Label
                        {
                            Text = text,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label))
                        };

                        // add frame to the grid
                        grid.Children.Add(frame, i, j);
                    }
                }
            }
            return grid;
        }
    
        async void GoBackToMainMenuPage(object sender, EventArgs ea)
        {
            await Navigation.PopModalAsync();
        }
    }
}