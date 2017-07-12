using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XRousseSudoku
{
    // SliderStep: slider with custom step integer value + custom Label + associating a name to each value
    public class SliderStep : StackLayout
    {
        private Label _label;
        private Slider _slider;
        private int _stepValue;
        private List<string> _valueNames;
        private String _valueNamePrefix;

        public SliderStep(
            float inMin,
            float inMax,
            List<string> valueNames,
            string valueNamePrefix,
            int step = 1)
        {
            // Instantiate new slider
            _slider = new Slider();
            _slider.Maximum = inMax;
            _slider.Minimum = inMin;

            // Instantiate new label
            _label = new Label
            {
                // Add options for the difficulty message
                Margin = new Thickness(10, 0, 10, 10),
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
            };

            // init members
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
            UpdateText((int)_slider.Value);

            //
            _slider.ValueChanged += OnSliderValueChanged;

            // Add content to Layout
            this.Children.Add(_slider);
            this.Children.Add(_label);
        }

        public void SetDefaultValue()
        {
            // Calculating the average between min and max slider value
            double average = (_slider.Minimum + _slider.Maximum) / 2.0;
            _slider.Value = (int)average;
        }

        // Method to change the value of message slider with the difficulty list
        public void UpdateText(int value)
        {
            _label.Text = _valueNamePrefix + " " + _valueNames[value - 1];
        }

        // Method to change value from slide only to int value
        public void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            // Round value between the new value on Step Value
            var newStep = Math.Round(e.NewValue / _stepValue);

            // Affect it to the slide value
            _slider.Value = newStep * _stepValue;

            // Call method to display the new difficulty level
            UpdateText((int)_slider.Value);
        }
    }
}
