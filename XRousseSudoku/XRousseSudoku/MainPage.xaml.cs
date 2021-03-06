﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XRousseSudoku
{
    public partial class MainPage : ContentPage
    {
        private SliderStep _sliderLayout;

        public MainPage()
        {
            // must stay first
            InitializeComponent();

            // set background image
            BackgroundImage = "retina_wood_1024.png";
        
            // Build the page
            Content = MainMenu();
        }        

        protected override void OnAppearing()
        {
            // must set slider value here because of Xamarin bug when slider.Minimum != 0
            _sliderLayout.SetDefaultValue();
        }

        public View MainMenu()
        {
            // Create title view
            Label mainTitleView = new Label
            {
                // Add options for the title view
                Margin = new Thickness(0, 40, 10, 0),
                Text           = "XRousse Sudoku",
                FontSize       = 40,
                FontFamily     = "Verdana",
                FontAttributes = FontAttributes.Bold,
                TextColor      = Color.FromHex("#000"),
                VerticalOptions   = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            // Create slider view
            int minSlider = 1;
            int maxSlider = 3;
            List<string> difficultyNames = new List<string>()
            {
                "Facile",
                "Normal",
                "Difficile"
            };
            string prefix = "La difficulté est :";

            // Create new Layout for the slider
            _sliderLayout = new SliderStep(minSlider, maxSlider, difficultyNames, prefix);
            
            // Create new button view to create a new game
            Button newGameBtn = new Button
            {
                Margin = new Thickness(10, 0, 10, 0),
                Text = "Nouvelle partie",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth  = 1,
                BorderRadius = 15,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions   = LayoutOptions.CenterAndExpand
            };

            // call OnNewGameButton() when button clicked
            newGameBtn.Clicked += OnNewGameButton;

            // Create new Layout with all the Views
            StackLayout contentMenu = new StackLayout
            {
                // Add padding to the Menu Content
                Padding = new Thickness(0, 20, 0, 20),
                // Contenu of the main menu
                Children =
                    {
                        // Add title
                        mainTitleView,
                        // Add slider
                        _sliderLayout,
                        // Add button to create new game
                        newGameBtn
                    }
            };

            // Return a View of the content menu
            return contentMenu;
        }

        async void OnNewGameButton(object sender, EventArgs ea)
        {
            var modalPage = new GamePage();            
            await Navigation.PushModalAsync(modalPage);
        }
    }
}
