using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public PlayerScript Player1 = null;
    public PlayerScript Player2 = null;
    private PlayerScript _turn;
    private GridScript _grid = null;
    private int _winCondition = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeGame(int gridSize)
    {
        _grid = new GridScript();
        _grid.SetGrid(gridSize);
        RandomizeFirstGoer();
    }

    void ChangeTurn()
    {
        if (_turn == Player1)
        {
            _turn = Player2;
        }
        else
        {
            _turn = Player1;
        }
    }

    void RandomizeFirstGoer() {        
        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            _turn = Player1;
            Player1.Piece = "X";
            Player2.Piece = "O";
        }
        else
        {
            _turn = Player2;
            Player2.Piece = "X";
            Player1.Piece = "O";
        }
    }

    bool ReturnifValueWon(int x, int y)
    {
        string value = _grid.Get(x, y);
        int count = 1;
        for (int y2 = y-1; y2 <= y+1; y2++)
        {
            if (y2 < 0 || y2 >= _grid.Size)
            {
                continue;
            }
            for (int x2 = x-1; x2 <= x+1; x2++)
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
                    count += ReturnValuesInARow(x, y, delta_x, delta_y);
                    count += ReturnValuesInARow(x, y, -delta_x, -delta_y);
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

    int ReturnValuesInARow(int x, int y, int delta_x, int delta_y)
    {
        if (x < 0 || x >= _grid.Size || y < 0 || y >= _grid.Size)
        {
            return 0;
        }
        if (_grid.Get(x, y) == _grid.Get(x + delta_x, y + delta_y))
        {
            return 1 + ReturnValuesInARow(x + delta_x, y + delta_y, delta_x, delta_y);
        }
        else
        {
            return 0;
        }
    }
}
