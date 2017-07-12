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
        Label label;

        public MainPage()
        {
            InitializeComponent();

            Label header = new Label
            {
                Text = "XRousse Sudoku",
                FontSize = 40,
                FontFamily = "Comic Sans MS",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex("#FFF"),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            Slider slider = new Slider
            {
                Minimum = 0,
                Maximum = 5,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            slider.ValueChanged += OnSliderValueChanged;

            label = new Label
            {
                Text = "Slider value is 0",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Button newGameBtn = new Button
            {
                Text = "Nouvelle partie",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth = 1,
                BorderRadius = 15,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            // Accomodate iPhone status bar.
            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // Build the page.
            Content = new StackLayout
            {
                Children =
                {
                    header,
                    slider,
                    label,
                    newGameBtn
                }
            };

            RelativeLayout relative = new RelativeLayout();

            ScrollView scroll = new ScrollView {
                Content = {}
            };


        }
        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            label.Text = String.Format("Slider value is {0:F1}", e.NewValue);
        }
    }
}
