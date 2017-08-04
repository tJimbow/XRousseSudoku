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
        protected Label _messageHeader;
        ///////////////////////////////////////////////////////////////////////
        // HEADER 
        ///////////////////////////////////////////////////////////////////////

        public View GenerateHeaderContent()
        {
            StackLayout header = new StackLayout();
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
            Label logHeader = new Label
            {
                Margin = new Thickness(0, 40, 10, 0),
                Text = "Select a case",
                FontSize = 20,
                FontFamily = "Verdana",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex("#000"),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            _messageHeader = logHeader;
            header.Children.Add(maintTitleGame);
            header.Children.Add(logHeader);
            return header;
        }

        ///////////////////////////////////////////////////////////////////////
        // FOOTER
        ///////////////////////////////////////////////////////////////////////

        public StackLayout GenerateNumbersLayout(SudokuGridData sudokuGridData, SudokuGridView GridView)
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
                btn.Clicked += (object sender, EventArgs e) => {
                    String message = sudokuGridData.SetCellValue(sudokuGridData.GetSelectedCell(), int.Parse(btn.Text));
                    _messageHeader.Text = message;
                    GridView.Update();
                };
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

        public View GenerateFooterContent(SudokuGridData GridData, SudokuGridView GridView)
        {
            StackLayout footer = new StackLayout();
            footer.Children.Add(GenerateNumbersLayout(GridData, GridView));
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

            // remove some values
            _gridData.RemoveGridValue(48);            
            _gridData.GetNullCells();

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
            _sudokuGridView = new SudokuGridView(_gridData, gridContainer, _messageHeader);
            gridContainer.Content = _sudokuGridView;
            
            // container for the footer
            ContentView footerContainer = new ContentView
            {
                BackgroundColor = debug ? Color.Blue : Color.Transparent,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = GenerateFooterContent(_gridData, (SudokuGridView) _sudokuGridView)
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