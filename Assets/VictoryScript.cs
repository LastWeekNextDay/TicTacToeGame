using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScript
{
    private GridScript _grid = null;
    private int _winCondition = 3;

    public VictoryScript(GridScript grid, int winCondition)
    {
        _grid = grid;
        _winCondition = winCondition;
    }

    public bool ValueHasWon(int x, int y)
    {
        string value = _grid.Get(x, y);
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
                if (_grid.Get(x2, y2) == value)
                {
                    int delta_x = x2 - x;
                    int delta_y = y2 - y;
                    count += ValuesInARow(x, y, delta_x, delta_y);
                    count += ValuesInARow(x, y, -delta_x, -delta_y);
                    if (count >= _winCondition)
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
        if (_grid.Get(x, y) == _grid.Get(x + delta_x, y + delta_y))
        {
            return 1 + ValuesInARow(x + delta_x, y + delta_y, delta_x, delta_y);
        }
        else
        {
            return 0;
        }
    }
}
