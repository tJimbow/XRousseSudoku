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

        // the sudoku grid needs to be inside a Frame Layout using FillAndExpand (containerFrame param)
        public SudokuGridView(SudokuGridData gridData, Frame containerFrame)
        {
            // for now we only support square grids
            Debug.Assert(gridData.IsSquare(), "SudokuGridView only support square grids (for now) !");

            // store associated grid data
            _gridData = gridData;

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
                int iBloc = i / gridData.BlocH;
                for (int j = 0; j < gridData.W; j++)
                {
                    int jBloc = j / gridData.BlocW;

                    // each cell is a ContentView containing a Label
                    var cell = new ContentView
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(0),
                        BackgroundColor = (((iBloc + jBloc) % 2) == 0) ? Color.White : Color.LightGray,
                    };
                    
                    // if value =  0, display nothing
                    String text = (_gridData.GetCell(i, j) != 0) ? _gridData.GetCell(i, j).ToString() : "";
                    cell.Content = new Label
                    {
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Text = text,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    };

                    // add cell to the grid
                    grid.Children.Add(cell, i, j);
                }
            }

            // add the grid to (this)
            this.Content = grid;
        }

        // updates the cell colors / labels
        public void Update()
        {

        }
    }
};
