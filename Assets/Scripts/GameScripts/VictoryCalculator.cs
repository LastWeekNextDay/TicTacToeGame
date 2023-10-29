using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCalculator
{
    private TicTacToeGrid _grid = null;
    public int WinCondition = 3;

    public VictoryCalculator(TicTacToeGrid grid, int winCondition)
    {
        _grid = grid;
        WinCondition = winCondition;
    }

    public bool GameIsTied()
    {
        return _grid.AllSlotsOccupied();
    }

    public bool ValueHasWon(int valueXCoordinate, int valueYCoordinate)
    {
        string valuePiece = _grid.Get(valueXCoordinate, valueYCoordinate).Piece();
        if (valuePiece != "O" && valuePiece != "X") { return false; }
        // Check each slot that is adjacent to the provided slot
        int sameValuesInARow = 1;
        for (int yCoordinateToCheck = valueYCoordinate - 1; yCoordinateToCheck <= valueYCoordinate + 1; yCoordinateToCheck++)
        {
            if (yCoordinateToCheck < 0 || yCoordinateToCheck >= _grid.Size)
            {
                continue;
            }
            for (int xCoordinateToCheck = valueXCoordinate - 1; xCoordinateToCheck <= valueXCoordinate + 1; xCoordinateToCheck++)
            {
                // Skip slots that are out of bounds, not equal to value or are at the position of the provided slot
                sameValuesInARow = 1;
                if (xCoordinateToCheck < 0 || 
                    xCoordinateToCheck >= _grid.Size || 
                    (xCoordinateToCheck == valueXCoordinate && yCoordinateToCheck == valueYCoordinate) || 
                    _grid.Get(xCoordinateToCheck, yCoordinateToCheck).Piece() != valuePiece)
                {
                    continue;
                }
                // Go forwards along the path of the slot which is occupied by the same value until the end or different value is reached, then do the same backwards
                int rateOfChangeForX = xCoordinateToCheck - valueXCoordinate;
                int rateOfChangeForY = yCoordinateToCheck - valueYCoordinate;
                sameValuesInARow += ValuesInARow(valueXCoordinate, valueYCoordinate, rateOfChangeForX, rateOfChangeForY);
                sameValuesInARow += ValuesInARow(valueXCoordinate, valueYCoordinate, -rateOfChangeForX, -rateOfChangeForY);
                if (sameValuesInARow >= WinCondition)
                {
                    break;
                }
            }
        }
        if (sameValuesInARow >= WinCondition)
        {
            return true;
        }
        return false;
    }

    int ValuesInARow(int xCoordinate, int yCoordinate, int rateOfChangeForX, int rateOfChangeForY)
    {
        // Recurring function to count the number of values in a row in a given direction (delta_x, delta_y) from a given position (x, y)
        if (xCoordinate < 0 || 
            xCoordinate >= _grid.Size || 
            yCoordinate < 0 || 
            yCoordinate >= _grid.Size || 
            xCoordinate + rateOfChangeForX < 0 || 
            xCoordinate + rateOfChangeForX >= _grid.Size || 
            yCoordinate + rateOfChangeForY < 0 || 
            yCoordinate + rateOfChangeForY >= _grid.Size || 
            _grid.Get(xCoordinate, yCoordinate).Piece() != _grid.Get(xCoordinate + rateOfChangeForX, yCoordinate + rateOfChangeForY).Piece())
        {
            return 0;
        }
        return 1 + ValuesInARow(xCoordinate + rateOfChangeForX, yCoordinate + rateOfChangeForY, rateOfChangeForX, rateOfChangeForY);
    }
}
