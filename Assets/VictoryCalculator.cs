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

    public bool ValueHasWon(int x, int y)
    {
        string value = _grid.Get(x, y).Piece();
        if (value == null)
        {
            return false;
        }
        int count = 1;
        for (int y2 = y - 1; y2 <= y + 1; y2++)
        {
            if (y2 < 0 || y2 >= _grid.Size)
            {
                continue;
            }
            for (int x2 = x - 1; x2 <= x + 1; x2++)
            {
                if (x2 < 0 || x2 >= _grid.Size)
                {
                    continue;
                }
                if (x2 == x && y2 == y)
                {
                    continue;
                }
                if (_grid.Get(x2, y2).Piece() == value)
                {
                    int delta_x = x2 - x;
                    int delta_y = y2 - y;
                    count += ValuesInARow(x, y, delta_x, delta_y);
                    count += ValuesInARow(x, y, -delta_x, -delta_y);
                    if (count >= WinCondition)
                    {
                        return true;
                    }
                    count = 1;
                }
            }
        }
        return false;
    }

    int ValuesInARow(int x, int y, int delta_x, int delta_y)
    {
        if (x < 0 || x >= _grid.Size || y < 0 || y >= _grid.Size)
        {
            return 0;
        }
        if (_grid.Get(x, y).Piece() == _grid.Get(x + delta_x, y + delta_y).Piece())
        {
            return 1 + ValuesInARow(x + delta_x, y + delta_y, delta_x, delta_y);
        }
        else
        {
            return 0;
        }
    }
}
