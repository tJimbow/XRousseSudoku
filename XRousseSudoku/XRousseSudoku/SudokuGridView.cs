using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace XRousseSudoku
{
    // SudokuGridView is a Horizontal/Vertical Centered ContentView containing:
    //    -> 1 Grid (FillAndExpand)
    //       -> NxN ContentViews (FillAndExpand, padding 0)
    //          -> 1 Label (CenterAndExpand)
    public class SudokuGridView : ContentView
    {
        protected SudokuGridData _gridData;
        protected Label[][] _labels;
        protected SudokuGridCell _currentSelectedCell = null;
        protected Label _messageHeader;

        // the sudoku grid needs to be inside a Frame Layout using FillAndExpand (containerFrame param)
        public SudokuGridView(SudokuGridData gridData, Frame containerFrame, Label messageHeader)
        {
            // Set label header for log and info and error
            _messageHeader = messageHeader;
            // for now we only support square grids
            Debug.Assert(gridData.IsSquare(), "SudokuGridView only support square grids (for now) !");

            // store associated grid data
            _gridData = gridData;

            // we will store reference to each label in a 2D array
            _labels = new Label[gridData.W][];
            for (int i = 0; i < gridData.W; i++)
                _labels[i] = new Label[gridData.H];

            // Centered, no expand/fill (because we want to control width/height ourselves)
            this.HorizontalOptions = LayoutOptions.Center;
            this.VerticalOptions   = LayoutOptions.Center;

            // padding / bg color
            this.Padding = new Thickness(0);

            // add a callback to set (this) prefered width & height each time containerFrame
            containerFrame.SizeChanged += (object sender, EventArgs e) =>
            {
                // we want the grid to be square, filling the smallest of width or height
                double minSize = Math.Min(containerFrame.Width, containerFrame.Height);

                // little margin (2 x 12 pixels)
                minSize -= 2 * 12;

                // hack to avoid small holes in the grid
                minSize = Math.Floor(minSize / _gridData.W) * _gridData.W;

                //
                this.WidthRequest = minSize;
                this.HeightRequest = minSize;
            };

            // create the grid
            Grid grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 0
            };

            // add lines and columns to the grid
            // (note: do not interleave RowDefinitions.Add & ColumnDefinitions.Add calls, this fails !)
            for (int i = 0; i < gridData.H; i++)
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            for (int j = 0; j < gridData.W; j++)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // grid cells
            for (int i = 0; i < gridData.H; i++)
            {
                for (int j = 0; j < gridData.W; j++)
                {
                    SudokuGridCell curCell = _gridData.GetGridCell(i, j);

                    // each cell is a ContentView containing a Label
                    var cell = new ContentView
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(0),
                        BackgroundColor = GetCellBaseColor(curCell),
                    };

                    // if value =  0, display nothing
                    cell.Content = _labels[i][j] = new Label
                    {
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Text = GetCellText(_gridData.GetCell(i, j)),
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    };

                    // new Event bind on cell click
                    TapGestureRecognizer tap = new TapGestureRecognizer();
                    EventHandler myFunc = (object sender, EventArgs e) => {
                        OnCellClick(curCell);
                    };
                    tap.Tapped += myFunc;
                    cell.GestureRecognizers.Add(tap);

                    // add cell to the grid
                    grid.Children.Add(cell, i, j);
                }
            }

            // add the grid to (this)
            this.Content = grid;
        }

        // Action on when you click on a cell
        public void OnCellClick(SudokuGridCell curCell)
        {
            _currentSelectedCell = curCell;
            if (curCell.IsEditable)
            {
                _messageHeader.Text = "Veuillez sélectionner un caractère";
                Update();
                curCell.IsSelected = true;
                curCell.displayPossibleValues();
                _currentSelectedCell = null;
            }
            else
            {
                Update();
                _currentSelectedCell = null;
                _messageHeader.Text = "Veuillez sélectionner une case";
            }
        }

        public Color GetCellBaseColor(SudokuGridCell curCell)
        {
            int iBloc = curCell.GetCoordX / _gridData.BlocH;
            int jBloc = curCell.GetCoordY / _gridData.BlocW;
            return (((iBloc + jBloc) % 2) == 0) ? Color.White : Color.LightGray;
        }

        //
        public String GetCellText(int gridValue)
        {
            return (gridValue != 0) ? gridValue.ToString() : "";
        }

        // updates the cell colors / labels
        public void Update()
        {
            for (int i = 0; i < _gridData.H; i++)
            {
                for (int j = 0; j < _gridData.W; j++)
                {
                    // get cell from sudokuGridData
                    SudokuGridCell curCell = _gridData.GetGridCell(i, j);
                    curCell.IsSelected = false;
                    // get label
                    var label = _labels[i][j];

                    // set label text
                    label.Text = GetCellText(curCell.Value);

                    // get gridView cell
                    ContentView cell = label.Parent as ContentView;

                    cell.BackgroundColor = GetCellBaseColor(curCell);
                    if (_currentSelectedCell == curCell)
                        cell.BackgroundColor = Color.Green;
                }
            }
        }
    }
};
