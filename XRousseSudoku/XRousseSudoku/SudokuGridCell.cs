using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace XRousseSudoku
{
    public class SudokuGridCell
    {
        private int _value;
        private int _coordX;
        private int _coordY;
        private List<int> _possibleValue = new List<int>();

        public int Value { get => _value; set => this._value = value; }
        public List<int> PossibleValue { get => _possibleValue; set => _possibleValue = value; }
        public int GetCoordX { get => _coordX; }
        public int GetCoordY { get => _coordY; }

        public SudokuGridCell(int value)
        {
            this.Value = value;
        }
        public void SetCoordCell(int line, int column)
        {
            _coordX = line;
            _coordY = column;
        }
        public void displayPossibleValues()
        {
            Debug.Write(" Values for case : X " +GetCoordX+" - Y "+GetCoordY);
            foreach (int value in _possibleValue)
            {
                Debug.Write(" "+value+",");
            }
            Debug.WriteLine("");
        }
        public override string ToString()
        {
            return this._value.ToString();
        }
    }
}
