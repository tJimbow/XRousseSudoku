using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Diagnostics;

namespace XRousseSudoku
{
    public partial class GamePage : ContentPage
    {
        public GamePage()
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

            for (int i = 1; i < 10; i++)
            {
                Button btn = new Button();
                btn.Text = i.ToString();
                numbersLayout.Children.Add(btn);
            }

            return numbersLayout;
        }

        // structure of the sudoku grid:
        // Frame (centered, padding 0)
        //  -> Grid (fillExpand)
        //      -> ContentView (fillExpand, padding 0)
        //          -> Label (center & expand)
        public View GridTest()
        {
            // width / height of the grid
            int N = 9;

            // create the container frame
            Frame container = new Frame
            {
                Padding = new Thickness(0),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Blue,
                HasShadow = false
            };

            // force container to be proportional to window size and square
            this.SizeChanged += (object sender, EventArgs e) =>
            {
                double iSize = this.Height * 0.75;
                if (iSize >= this.Width)
                    iSize = this.Width - 32;
                container.WidthRequest = iSize;
                container.HeightRequest = iSize;
            };

            // create a grid
            Grid grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 0
            };

            // add N lines and N columns to the grid
            // (do not interleave RowDefinitions.Add & ColumnDefinitions.Add calls, this fails !)
            for (int i = 0; i < N; i++)
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            for (int j = 0; j < N; j++)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // grid cells
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    var contentViewCell = new ContentView
                    {
                        BackgroundColor = (((i + j) % 2) == 0) ? Color.White : Color.LightGray,
                        Padding = new Thickness(0),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                    };

                    contentViewCell.Content = new Label
                    {
                        Text = "99",
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                    };

                    grid.Children.Add(contentViewCell, i, j);
                }
            }

            // add grid to the frame
            container.Content = grid;

            return container;
        }

        async void GoBackToMainMenuPage(object sender, EventArgs ea)
        {
            await Navigation.PopModalAsync();
        }
    }
}