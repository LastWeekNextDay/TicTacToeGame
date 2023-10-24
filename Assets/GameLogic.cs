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
    private VictoryScript _victoryCalc = null;
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
        _grid = new GridScript();
        _grid.SetupGrid(gridSize);
        _victoryCalc = new VictoryScript(_grid, winCondition);
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

    void PlacePiece(int x, int y, PlayerScript player)
    {
        if (_grid.Set(x, y, player))
        {
            if (_victoryCalc.ValueHasWon(x, y))
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
