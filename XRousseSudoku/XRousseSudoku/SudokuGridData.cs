using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace XRousseSudoku
{
    public class SudokuGridData
    {
        //Randomize need to be initialized once
        protected Random rdm = new Random();

        // members
        protected int _blockW;
        protected int _blockH;
        protected int _width;
        protected int _height;

        protected int _nSymbols;
        protected int[][] _cells;

        // getters
        public int BlocW { get => _blockW; }
        public int BlocH { get => _blockH; }
        public int W { get => _width; }
        public int H { get => _height; }

        public bool IsSquare()
        {
            return W == H;
        }

        public int GetCell(int line, int column)
        {
            Debug.Assert((line >= 0) && (line < H), "SudokuGridData, getCell, invalid line: " + line);
            Debug.Assert((column >= 0) && (column < W), "SudokuGridData, getCell, invalid column: " + column);
            return _cells[line][column];
        }

        // constructor
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
            for(int i = 0; i <= 20; i++)
            {
                RandomizeRows(10);
                RandomizeColumns(10);
            }
            
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
            Log();
        }
        // Is Each symbols unique per line, column and block
        public bool CheckSymbols(int[] sequence)
        {

            // REVIEW : Modification du check si des cases sont vide ne renvoie pas d'erreur et va donc
            // Compter les cases vides et les soustraits du total
            int nFound = 0;
            int nbNull = 0;
            // Count of null value in tab
            for(int i = 0; i < sequence.Length; i++)
            {
                if(sequence[i] == 0)
                {
                    nbNull++;
                }
            }
            // Check foreach Symbols is unique
            for (int k = 1; k <= _nSymbols; k++)
            {
                int pos = Array.IndexOf(sequence, k);
                if (pos > -1)
                    nFound++;

            }
            return (nFound != (_nSymbols - nbNull)) ? false : true;
        }

        public bool IsValid()
        {

            // Test lines
            for (int j = 0; j < _height; j++)
            {
                if (!CheckSymbols(_cells[j]))
                {
                    return false;
                }
            }
            // Test Columns
            int[] tabColToTest = new int[_height];
            for(int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    tabColToTest[j] = _cells[i][j];
                }
                if (!CheckSymbols(tabColToTest))
                {
                    return false;
                }
            }

            // Test blocks
            int[] tabBlockToTest = new int[_blockW * _blockH];
            for(int i = 0; i < _width; i += _blockW)
            {
                for( int j = 0; j < _height; j+= _blockH)
                {
                    int indexTab = 0;
                    for ( int k = i; k < (i + _blockW); k++)
                    {
                        for (int l = j; l < (j + _blockH); l++)
                        {
                            int val = _cells[k][l];
                            tabBlockToTest[indexTab] = val;
                            indexTab++;
                        }
                    }
                    if (!CheckSymbols(tabBlockToTest))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // Randomize helper
        public void RandomizeHelper(int blockNb, out int offsetA, out int offsetB)
        {
            int rBlock = rdm.Next(0, blockNb);

            // Do Randomize While Column A is not like Column B 
            int rA, rB;
            rA = rdm.Next(1, (blockNb + 1));
            do
            {
                rB = rdm.Next(1, (blockNb + 1));
            } while (rA == rB);
            offsetA = ((rBlock * _blockW) + rA) - 1;
            offsetB = ((rBlock * _blockW) + rB) - 1;
        }

        // Randomize columns
        public void RandomizeColumns(int nbTimes)
        {
            for( int nb = 0; nb <= nbTimes; nb++)
            {
                // Get offset to be modified via RandomizeHelper method
                RandomizeHelper(_blockW, out int offsetA, out int offsetB);

                // Do permutation
                int[] tempTab = _cells[offsetB];
                _cells[offsetB] = _cells[offsetA];
                _cells[offsetA] = tempTab;
            }
        }

        //Randomize rows
        public void RandomizeRows(int nbTimes)
        {
            for (int nb = 0; nb <= nbTimes; nb++)
            {
                // Get offset to be modified via RandomizeHelper method
                RandomizeHelper(_blockH, out int offsetA, out int offsetB);

                // Do Permutation
                for (int i = 0; i < _width; i++)
                {
                    int tempValue = 0;
                    tempValue = _cells[i][offsetB];
                    _cells[i][offsetB] = _cells[i][offsetA];
                    _cells[i][offsetA] = tempValue;
                }
            }
        }

        public void Log()
        {
            if (!IsValid())
            {
                Debug.WriteLine("Invalid Sudoku Grid : ");
            }
            else
            {
                Debug.WriteLine("Valid Sudoku Grid : ");
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