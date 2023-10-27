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
        // Go through each slot and if at least one is not occupied, return false - Game not tied
        int x, y;
        for (x = 0; x < _grid.Size; x++)
        {
            for (y = 0; y < _grid.Size; y++)
            {
                if (!_grid.Get(x, y).IsOccupied)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool ValueHasWon(int x, int y)
    {
        string value = _grid.Get(x, y).Piece();
        if (value == null)
        {
            return false;
        }
        int count = 1;
        // Check each slot that is adjacent to the provided slot
        for (int y2 = y - 1; y2 <= y + 1; y2++)
        {
            if (y2 < 0 || y2 >= _grid.Size)
            {
                continue;
            }
            for (int x2 = x - 1; x2 <= x + 1; x2++)
            {
                // Skip slots that are out of bounds or are at the position of the provided slot
                count = 1;
                if (x2 < 0 || x2 >= _grid.Size)
                {
                    continue;
                }
                if (x2 == x && y2 == y)
                {
                    continue;
                }
                // Go forwards along the path of the slot which is occupied by the same value until the end or different value is reached, then do the same backwards
                if (_grid.Get(x2, y2).Piece() == value)
                {
                    int delta_x = x2 - x;
                    int delta_y = y2 - y;
                    count += ValuesInARow(x, y, delta_x, delta_y);
                    count += ValuesInARow(x, y, -delta_x, -delta_y);
                    Debug.Log(count + "/" + WinCondition + " for " + value);
                    if (count >= WinCondition)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    int ValuesInARow(int x, int y, int delta_x, int delta_y)
    {
        // Recurring function to count the number of values in a row in a given direction (delta_x, delta_y) from a given position (x, y)
        if (x >= 0 && x < _grid.Size && y >= 0 && y < _grid.Size)
        {
            if (x + delta_x >= 0 && x + delta_x < _grid.Size && y + delta_y >= 0 && y + delta_y < _grid.Size)
            {
                if (_grid.Get(x, y).Piece() == _grid.Get(x + delta_x, y + delta_y).Piece())
                {
                    return 1 + ValuesInARow(x + delta_x, y + delta_y, delta_x, delta_y);
                }
            }
        }
        return 0;
    }
}
