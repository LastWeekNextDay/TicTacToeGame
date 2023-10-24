using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Player Player1 = null;
    public Player Player2 = null;
    private Player _turn;
    private Grid _grid = null;
    private VictoryCalculator _victoryCalculator = null;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeGame(int gridSize, int winCondition)
    {
        _grid = new Grid();
        _grid.SetupGrid(gridSize);
        _victoryCalculator = new VictoryCalculator(_grid, winCondition);
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

    void PlacePiece(int x, int y, Player player)
    {
        if (_grid.Set(x, y, player))
        {
            if (_victoryCalculator.ValueHasWon(x, y))
            {
                Debug.Log("Player " + player.Piece + " has won!");
            }
            else
            {
                ChangeTurn();
            }
        }
    }
}
