using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XRousseSudoku
{
    // SliderStep: slider with custom step integer value + custom Label + associating a name to each value
    public class SliderStep : Slider
    {
        private Label _label;
        private int _stepValue;
        private List<string> _valueNames;
        private String _valueNamePrefix;

        public Label Label { get => _label; }

        public SliderStep(
            float inMin, 
            float inMax, 
            Label label,
            List<string> valueNames,
            string valueNamePrefix,
            int step=1)
        {
            // init members
            Maximum = inMax;
            Minimum = inMin;
            _label = label;
            _stepValue = step;
            _valueNames = valueNames;
            _valueNamePrefix = valueNamePrefix;

            // Margin for the slider thickness(left,top,right,bottom)
            Margin = new Thickness(100, 0, 100, 0);

            // Slider is Centered and Expands with parent
            VerticalOptions = LayoutOptions.CenterAndExpand;

            // set slider min / max & value
            SetDefaultValue();
            
            // 
            UpdateText((int)Value);

            //
            ValueChanged += OnSliderValueChanged;
        }

        public void SetDefaultValue()
        {
            // Calculating the average between min and max slider value
            double average = (Minimum + Maximum) / 2.0;
            Value = (int)average;
        }

        // Method to change the value of message slider with the difficulty list
        public void UpdateText(int value)
        {
            Label.Text = _valueNamePrefix + " " + _valueNames[value - 1];
        }

       // Method to change value from slide only to int value
        public void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            // Round value between the new value on Step Value
            var newStep = Math.Round(e.NewValue / _stepValue);

            // Affect it to the slide value
            this.Value = newStep * _stepValue;

            // Call method to display the new difficulty level
            UpdateText((int)Value);
        }
    }

    public partial class MainPage : ContentPage
    {
        private SliderStep _difficultySlider;

        public MainPage()
        {
            // must stay first
            InitializeComponent();

            // set background image
            BackgroundImage = "retina_wood_1024.png";
        
            // Build the page.
            bool testGrid = false;
            if (testGrid)
            {
                // Display Grid
                Content = Grid4x4_Test();
            }
            else
            {
                // Or display Menu
                Content = MainMenu();
            }
        }        

        protected override void OnAppearing()
        {
            // must set slider value here because of Xamarin bug when slider.Minimum != 0
            _difficultySlider.SetDefaultValue();
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

            // Create the difficulty message view
            Label difficultyLabel = new Label
            {
                // Add options for the difficulty message
                Margin = new Thickness(10, 0, 10, 10),
                Text     = "Niveau de difficulté : Normal",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions   = LayoutOptions.StartAndExpand,
            };

            // Create slider view
            float minSlider = 1.0f;
            float maxSlider = 3.0f;
            List<string> difficultyNames = new List<string>()
            {
                "Facile",
                "Normal",
                "Difficile"
            };
            string prefix = "La difficulté est :";
            _difficultySlider = new SliderStep(minSlider, maxSlider, difficultyLabel, difficultyNames, prefix);
            
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
                        _difficultySlider,
                        // Add message for the slider
                        _difficultySlider.Label,
                        // Add button to create new game
                        newGameBtn
                    }
            };

            // Return a View of the content menu
            return contentMenu;
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
                    frame.VerticalOptions   = LayoutOptions.FillAndExpand;
                    frame.HorizontalOptions = LayoutOptions.FillAndExpand;
                    //frame.OutlineColor = Color.Red; // only has effect on uwp
                    frame.BackgroundColor = (((i + j)% 2) == 0) ? Color.White : Color.Red;

                    // the frame contains a centered label
                    string label_text = Char.ConvertFromUtf32(65 + i + j * 2);
                    frame.Content = new Label
                    {
                        Text = label_text,
                        VerticalOptions   = LayoutOptions.Center,
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
