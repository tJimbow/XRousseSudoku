using System;
using System.Collections.Generic;
using System.Text;
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
            int step = 1)
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
}
