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

            BackgroundImage = "retina_wood_1024.png";

            // REVIEW (0): add more comments explaining the code
            // REVIEW (1): all code to create main menu should go into a method (like Grid4x4_Test)
            // REVIEW (2): there should be some padding above the app title
            // REVIEW (3): Comic Sans MS is the worst font of all
            // REVIEW (4): header is not a good name for the title element ...
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

            // REVIEW (5): slider is not a good variable name
            Slider slider = new Slider
            {
                Minimum = 0,
                Maximum = 5,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            slider.ValueChanged += OnSliderValueChanged;

            // REVIEW (6): slider is not a good variable name
            // REVIEW (7): need to show difficulty level message instead of "Slider value is 0"
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

            // REVIEW (8): do we need this thing, what is the purpose ? 
            // Accomodate iPhone status bar.
            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
            
            // Build the page.
            bool testGrid = true;
            if (testGrid)
                Content = Grid4x4_Test();
            else
            {
                // REVIEW (1): all code to create main menu should go into a method (like Grid4x4_Test)
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

                // REVIEW (9): useless code, needs to be removed
                RelativeLayout relative = new RelativeLayout();
                ScrollView scroll = new ScrollView {
                    Content = {}
                };
            }
        }

        // REVIEW (7): need to show difficulty level message instead of "Slider value is 0"
        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            label.Text = String.Format("Slider value is {0:F1}", e.NewValue);
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
                    frame.OutlineColor = Color.Red; // only has effect on uwp
                    //frame.BackgroundColor = (((i + j)% 2) == 0) ? Color.White : Color.Red;

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

        //static void myPage_SizeChanged(object sender, EventArgs e)
        //{
        //    Debug.WriteLine(myPage.Width + " " + myPage.Height);
        //}
    }
}
