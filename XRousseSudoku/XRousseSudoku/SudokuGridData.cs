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
        private SudokuGridCell[][] _cells;
        private List<SudokuGridCell> _nullCells = new List<SudokuGridCell>();

        protected int[] _coordGrid;

        // getters
        public int BlocW { get => _blockW; }
        public int BlocH { get => _blockH; }
        public int W { get => _width; }
        public int H { get => _height; }

        public bool IsSquare()
        {
            return W == H;
        }

        // All value from specific Line in tab int[]
        public int[] GetLineGrid(int line)
        {
            int[] tabLine = new int[_width];
            for (int i = 0; i < _width; i++)
            {
                tabLine[i] = _cells[line][i].Value;
            }

            return tabLine;
        }
        // All value from specific Col int tab int[]
        public int[] GetColGrid(int col)
        {
            int[] tabCol = new int[_height];
            for(int i = 0; i < _height; i++)
            {
                tabCol[i] = _cells[i][col].Value;
            }
            return tabCol;
        }

        // Get specific cell
        public int GetCell(int line, int column)
        {
            Debug.Assert((line >= 0) && (line < H), "SudokuGridData, getCell, invalid line: " + line);
            Debug.Assert((column >= 0) && (column < W), "SudokuGridData, getCell, invalid column: " + column);
            return _cells[line][column].Value;
        }

        // constructor
        public SudokuGridData(int difficulty, int blockW = 3, int blockH  = 3)
        {
            _blockW = blockW;
            _blockH = blockH;
            _nSymbols = _blockW * _blockH;
            _width = _height = _nSymbols;

            // Allocate cells
            _cells = new SudokuGridCell[_width][];
            for(int i = 0; i< _width; i++)
            {
                _cells[i] = new SudokuGridCell[_height];
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
                    _cells[i][j] = new SudokuGridCell(( (lineStart + i - 1) % _nSymbols ) + 1, i, j);
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
                        
                        _cells[i][j] = new SudokuGridCell(_cells[column][j - _blockH].Value, i, j);
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
                int[] tabLineValue = GetLineGrid(j);
                if (!CheckSymbols(tabLineValue))
                {
                    return false;
                }
            }
            // Test Columns
            for(int i = 0; i < _width; i++)
            {
                int[] tabColToTest = GetColGrid(i);
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
                            int val = _cells[k][l].Value;
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

                // Do value permutation on each SudokuGridCell concern value
                int tempCellValue;
                for (int i = 0; i<_nSymbols; i++)
                {
                    tempCellValue = _cells[offsetB][i].Value;
                    _cells[offsetB][i].Value = _cells[offsetA][i].Value;
                    _cells[offsetA][i].Value = tempCellValue;
                }
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
                    int tempValue;
                    tempValue = _cells[i][offsetB].Value;
                    _cells[i][offsetB].Value = _cells[i][offsetA].Value;
                    _cells[i][offsetA].Value = tempValue;
                }
            }
        }

        // Hide nb in param values from grid 
        public void RemoveGridValue(int nbRemovedCell)
        {
            for(int i=0; i < nbRemovedCell; i++)
            {
                bool isValueInGrid = false;
                int numLine = 0;
                int numCol = 0;
                // While there is no value in cell
                while (!isValueInGrid)
                {
                    // We generate two numbers beetween 1 and the number of Symbols
                    numLine = rdm.Next(0, _nSymbols);
                    numCol = rdm.Next(0, _nSymbols);
                    // If cell is null we loop else we put the new value 0 in cell
                    if (_cells[numLine][numCol].Value != 0)
                    {
                        _cells[numLine][numCol].Value = 0;
                        isValueInGrid = true;
                    }
                }
            }
        }

        // Create ordered list with all SudokuGridCell with null values
        public void GetNullCells()
        {
            _nullCells.Clear();
            for(int i=0; i< _width; i++)
            {
                for(int j=0; j<_height; j++)
                {
                    if(_cells[i][j].Value == 0)
                    {
                        _nullCells.Add(_cells[i][j]);
                    }
                }
            }
            foreach(SudokuGridCell cell in _nullCells)
            {
                ChangePossibleListCellValues(cell);
                cell.displayPossibleValues();
            }
        }

        // Update list possible values for a null cell
        public void ChangePossibleListCellValues(SudokuGridCell cell)
        {
            cell.PossibleValue.Clear();
            for(int i=1; i <= _nSymbols; i++)
            {
                bool isPresent = false;
                int[] tabLineToCheck = GetLineGrid(cell.GetCoordX);
                int[] tabColToCheck = GetColGrid(cell.GetCoordY);
                int[] tabBlockCheck = new int[_blockW*_blockH];
                int numBlockW = cell.GetCoordX / _blockW;
                int minBlockW = numBlockW * _blockW;
                int maxBlockW = minBlockW + _blockW;
                int numBlockH = cell.GetCoordY / _blockH;
                int minBlockH = numBlockH * _blockH;
                int maxBlockH = minBlockH + _blockH;
                int indexOfTabBlock = 0;
                for(int w = minBlockW; w < maxBlockW; w++)
                {
                    for(int h = minBlockH; h< maxBlockH; h++)
                    {
                        tabBlockCheck[indexOfTabBlock] = _cells[w][h].Value;
                        indexOfTabBlock++;
                    }
                }
                if (Array.IndexOf(tabLineToCheck, i) > -1 || Array.IndexOf(tabColToCheck, i) > -1 || Array.IndexOf(tabBlockCheck, i) > -1)
                {
                    isPresent = true;
                }
                if (!isPresent)
                {
                    cell.PossibleValue.Add(i);
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