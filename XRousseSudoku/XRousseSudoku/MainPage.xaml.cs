using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XRousseSudoku
{
    // Add class slider with Label Update Method with difficulty list
    public class SliderStep : Slider
    {
        Label label;
        private int StepValue;
        private Label sliderMessage;

        // Construct Slider
        public SliderStep(float inMin, float inMax, Label inMessage, int step=1)
        {
            // Margin for the slider thickness(left,top,right,bottom)
            Margin = new Thickness(100, 0, 100, 0);
            // Int : step value for the slide
            StepValue = step;
            // Label : message label object from constructor
            sliderMessage = inMessage;
            // Calculating the average between min and max slider value
            float moyenne = (inMin + inMax) / 2;
            // Add options for the slider
            // float Min, Max and default value for the slider
            Maximum = inMax;
            Minimum = inMin;
            Value   = moyenne;
            // Slider is Center and Expand with parent
            VerticalOptions = LayoutOptions.CenterAndExpand;
            UpdateText((int)Value);
        }

        // Creating List for the difficulties
        List<string> difficultyList = new List<string>()
        {
            "Facile",
            "Normal",
            "Difficile"
        };


        // Method to change the value of message slider with the difficulty list
        public void UpdateText(int value)
        {
            sliderMessage.Text = String.Format("Niveau de difficulté : {0:F1}", difficultyList[value - 1]);
        }

       // Method to change value from slide only to int value
        public void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            // Round value between the new value on Step Value
            var newStep = Math.Round(e.NewValue / StepValue);
            // Affect it to the slide value
            this.Value = newStep * StepValue;
            // Call method to display the new difficulty level
            UpdateText((int)Value);
        }

    }

    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            BackgroundImage = "retina_wood_1024.png";

            // REVIEW (0): add more comments explaining the code
            // REVIEW (1): all code to create main menu should go into a method (like Grid4x4_Test)
            // REVIEW (2): there should be some padding above the app title
            // REVIEW (3): Comic Sans MS is the worst font of all
            // REVIEW (4): header is not a good name for the title element ...
            // REVIEW (6): slider is not a good variable name
            // REVIEW (7): need to show difficulty level message instead of "Slider value is 0"
            
            // Build the page.
            bool testGrid = false;
            if (testGrid)
                // Display Grid
                Content = Grid4x4_Test();
            else
            {
                // Or display Menu
                Content = mainMenu();
            }
        }


        public View mainMenu()
        {
            // Value min and max to initiate Slider
            float minSlider = 1.0f;
            float maxSlider = 3.0f;
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
            Label difficultyMessage = new Label
            {
                // Add options for the difficulty message
                Margin = new Thickness(10, 0, 10, 10),
                Text     = "Niveau de difficulté : Normal",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions   = LayoutOptions.StartAndExpand,
            };

            // Create slider view
            SliderStep difficultySlider = new SliderStep(minSlider, maxSlider, difficultyMessage);
            // Change value from the difficultySlider method OnSliderValueChanged when slider is moved
            difficultySlider.ValueChanged += difficultySlider.OnSliderValueChanged;

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
                        difficultySlider,
                        // Add message for the slider
                        difficultyMessage,
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
