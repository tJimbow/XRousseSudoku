# Architecture

## App states

* Main menu
* Play
* Results screen
* High scores

## Objectives

1. Draft main menu
2. Draft mode Play
3. Implement Play mode 

# Todo

1. bool SudokuGridData.isEditable(SudokuGridCell c)

2. void SudokuGridData.SetCellValue(SudokuGridCell c, int v)
   * in charge of updating all the related data (ex: possible values of other cells)

3. bool SudokuGridCell.ValueIsValid() (means: is in list of possible values)

4. call SudokuGridData.SetCellValue() when user clicks number layout

5. SudokuGridData.IsFullyValid()
   * return IsValid() && (pas de zeros);

6. Define style of a cell :
   * SudokuGridView
     {
         enum CELL_STYLE {SELECTED, VALID, INVALID, READONLY}
     }
     SELECTED: yellow
     VALID: green
     INVALID: red
     READONLY: GetCellBaseColor

7. SudokuGridView.setCellStyle(i, j, enum CELL_STYLE)
   * update "visual" of a cell depending on style (ex: red bg)

8. change SudokuGridView.update() to :
   * compute CELL_STYLE of each cell and call SudokuGridView.setCellStyle()
   * check if game is finished (SudokuGridData.IsFullyValid()) => handle victory

7. display game time in game header

8. handle victory 
   * show victory msg in header + play time
   * hide "abandonner", show "play again" & "main menu" buttons

9. hide/show numbers layout depending if a cell is selected and/or editable

10. add a "X" button in numbers layout, which allows user to put back a 0 in the gridData

11. improve look of layout number

12. improve cell styles (bold / italic, bg color)

13. "print" button
   * app generates html and open it in device browser
   * 3x button in game mode: print current, print initial, print solution

14. Handle difficulty

15. On main menu, user can define the character set (Emoji / Chinese / numbers ...)

16. Improve grid generator code
   * solver based on backtracking


