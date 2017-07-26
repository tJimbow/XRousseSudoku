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
        protected View _sudokuGridView;
        // Grid with change
        protected SudokuGridData _gridData;
        // Initial Grid with all value
        protected SudokuGridData _initialGridData;

        ///////////////////////////////////////////////////////////////////////
        // HEADER 
        ///////////////////////////////////////////////////////////////////////

        public View GenerateHeaderContent()
        {
            // title
            Label maintTitleGame = new Label
            {
                Margin = new Thickness(0, 40, 10, 0),
                Text = "In Game",
                FontSize = 40,
                FontFamily = "Verdana",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex("#000"),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            return maintTitleGame;
        }

        ///////////////////////////////////////////////////////////////////////
        // FOOTER
        ///////////////////////////////////////////////////////////////////////

        public StackLayout GenerateNumbersLayout()
        {
            StackLayout numbersLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            for (int i = 1; i < 10; i++)
            {
                Button btn = new Button();
                btn.Text = i.ToString();
                numbersLayout.Children.Add(btn);
            }

            return numbersLayout;
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

        async void GoBackToMainMenuPage(object sender, EventArgs ea)
        {
            await Navigation.PopModalAsync();
        }

        public View GenerateFooterContent()
        {
            StackLayout footer = new StackLayout();
            footer.Children.Add(GenerateNumbersLayout());
            footer.Children.Add(GenerateBackToMainMenuButton());
            return footer;
        }

        ///////////////////////////////////////////////////////////////////////
        // GAME PAGE
        ///////////////////////////////////////////////////////////////////////

        public GamePage()
        {
            // xamarin stuff
            InitializeComponent();

            // _gridData
            _gridData = new SudokuGridData(1);
            _gridData.RemoveGridValue();
            _initialGridData = _gridData;
            _gridData.Log();

            // set bg image
            BackgroundImage = "retina_wood_1024.png";

            //
            bool debug = false;

            // container for the header
            ContentView headerContainer = new ContentView
            {
                BackgroundColor = debug ? Color.Red : Color.Transparent,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = GenerateHeaderContent()
            };

            // container for the SudokuGridView (center container)
            Frame gridContainer = new Frame
            {
                BackgroundColor = debug ? Color.Yellow : Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0)
            };
            _sudokuGridView = new SudokuGridView(_gridData, gridContainer);
            gridContainer.Content = _sudokuGridView;
            
            // container for the footer
            ContentView footerContainer = new ContentView
            {
                BackgroundColor = debug ? Color.Blue : Color.Transparent,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = GenerateFooterContent()
            };
            
            // main stack layout
            StackLayout contentGame = new StackLayout
            {
                Children =
                {
                    headerContainer,
                    gridContainer,
                    footerContainer
                }
            };

            // Build the page
            Content = contentGame;
        }        
    }
}