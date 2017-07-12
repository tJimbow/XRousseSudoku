using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace XRousseSudoku
{
    class SudokuGridData
    {

        protected int _blockW;
        protected int _blockH;
        protected int _width;
        protected int _height;
        protected int _nSymbols;
        protected int[][] _cells;

        public SudokuGridData(int difficulty, int blockW = 3, int blockH  = 3)
        {
            _blockW = blockW;
            _blockH = blockH;
            _nSymbols = _blockW * _blockH;
            _width = _height = _nSymbols;

            // Allocate cells
            _cells = new int[_width][];
            for(int i = 0; i< _width; i++)
            {
                _cells[i] = new int[_height];
            }

            GenerateRandomGrid(difficulty);
        }

        public void GenerateRandomGrid(int difficulty)
        {
            // Generate the first row of block
            int lineStart = 1;
            for(int j = 0; j < _blockH; j++)
            {
                for(int i = 0; i < _width; i++)
                {
                    _cells[i][j] = ( (lineStart + i - 1) % _nSymbols ) + 1;
                }
                lineStart += _blockW;
            }

            // Generate next rows
            for(int j = _blockH ; j < _height; j++)
            {
                int i = 0;
                while(i < _width)
                {
                    int iBlock = i;
                    for(int k = 0; k < _blockW; k++)
                    {
                        int column = (((i + 1) - iBlock) % _blockW ) + iBlock;
                        
                        _cells[i][j] = _cells[column][j - _blockH];
                        i++;
                    }
                }
            }
        }

        public bool checkSymbols(int[] sequence)
        {
            int nFound = 0;
            for (int k = 1; k <= _nSymbols; k++)
            {
                int pos = Array.IndexOf(sequence, k);

                if (pos > -1)
                    nFound++;

            }
            return (nFound != _nSymbols) ? false : true;
        }

        public bool isValid()
        {
            int[] tabColToTest = new int[_nSymbols];
            int[] tabBlockToTest = new int[_nSymbols];

            // Test lines
            for (int j = 0; j < _height; j++)
            {
                if (!checkSymbols(_cells[j]))
                {
                    return false;
                }
            }

            // Test Columns
            for(int i = 0; i < _width; i++)
            {
                for(int j = 0; j < _height; j++)
                {
                    tabColToTest[j] = _cells[i][j];
                }
                if (!checkSymbols(tabColToTest))
                {
                    return false;
                }
            }

            // Test blocks
            for(int i = 0; i < _width; i += _blockW)
            {
                for( int j = 0; j < _height; j+= _blockH)
                {
                    int indexTab = 0;
                    for ( int k = i; k < i + _blockW; k++)
                    {
                        for (int l = j; l< j+_blockH; l++)
                        {
                            int val = _cells[k][l];
                            tabBlockToTest[indexTab] = val;
                            indexTab++;
                        }
                    }
                    if (!checkSymbols(tabBlockToTest))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void Log()
        {
            if (!isValid())
            {
                Debug.WriteLine("Invali Sudoku Grid : ");
            }
            for (int j = 0; j < _height; j++)
            {
                for (int i = 0; i < _width; i++)
                {
                    Debug.Write(_cells[i][j].ToString() + " ");
                }
                Debug.WriteLine("");
            }
        }
    }
}