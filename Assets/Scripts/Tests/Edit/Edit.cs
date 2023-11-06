using NUnit.Framework;
using UnityEngine;

public class Edit
{
    [Test]
    public void HasXWon()
    {
        int winCon = 3;
        int size = 3;
        GameLogic gameLogic = new GameObject().AddComponent<GameLogic>();
        gameLogic.Grid = new GameObject().AddComponent<TicTacToeGridUnity>();
        gameLogic.Grid.GridBase = new TicTacToeGrid();
        gameLogic.Grid.GridBase.SetupGrid(size);
        gameLogic.VictoryCalculator = new VictoryCalculator(gameLogic.Grid.GridBase, winCon);
        
        bool hasWon = false;
        gameLogic.Grid.GridBase.Grid[0][0].Piece = "X";
        gameLogic.Grid.GridBase.Grid[0][0].IsOccupied = true;
        gameLogic.Grid.GridBase.Grid[1][1].Piece = "X";
        gameLogic.Grid.GridBase.Grid[1][1].IsOccupied = true;
        gameLogic.Grid.GridBase.Grid[2][2].Piece = "X";
        gameLogic.Grid.GridBase.Grid[2][2].IsOccupied = true;
        hasWon = gameLogic.VictoryCalculator.ValueHasWon(2, 2);

        Assert.IsTrue(hasWon);
    }

    [Test]
    public void HasOWon()
    {
        int winCon = 3;
        int size = 3;
        GameLogic gameLogic = new GameObject().AddComponent<GameLogic>();
        gameLogic.Grid = new GameObject().AddComponent<TicTacToeGridUnity>();
        gameLogic.Grid.GridBase = new TicTacToeGrid();
        gameLogic.Grid.GridBase.SetupGrid(size);
        gameLogic.VictoryCalculator = new VictoryCalculator(gameLogic.Grid.GridBase, winCon);

        bool hasWon = false;
        gameLogic.Grid.GridBase.Grid[0][0].Piece = "O";
        gameLogic.Grid.GridBase.Grid[0][0].IsOccupied = true;
        gameLogic.Grid.GridBase.Grid[1][1].Piece = "O";
        gameLogic.Grid.GridBase.Grid[1][1].IsOccupied = true;
        gameLogic.Grid.GridBase.Grid[2][2].Piece = "O";
        gameLogic.Grid.GridBase.Grid[2][2].IsOccupied = true;
        hasWon = gameLogic.VictoryCalculator.ValueHasWon(2, 2);

        Assert.IsTrue(hasWon);
    }

    [Test]
    public void IsDraw()
    {
        int winCon = 3;
        int size = 3;
        GameLogic gameLogic = new GameObject().AddComponent<GameLogic>();
        gameLogic.Grid = new GameObject().AddComponent<TicTacToeGridUnity>();
        gameLogic.Grid.GridBase = new TicTacToeGrid();
        gameLogic.Grid.GridBase.SetupGrid(size);
        gameLogic.VictoryCalculator = new VictoryCalculator(gameLogic.Grid.GridBase, winCon);

        bool isDraw = false;
        gameLogic.Grid.GridBase.Grid[0][0].Piece = "X";
        gameLogic.Grid.GridBase.Grid[0][0].IsOccupied = true;
        gameLogic.Grid.GridBase.Grid[0][1].Piece = "O";
        gameLogic.Grid.GridBase.Grid[0][1].IsOccupied = true;
        gameLogic.Grid.GridBase.Grid[0][2].Piece = "X";
        gameLogic.Grid.GridBase.Grid[0][2].IsOccupied = true;
        gameLogic.Grid.GridBase.Grid[1][0].Piece = "O";
        gameLogic.Grid.GridBase.Grid[1][0].IsOccupied = true;
        gameLogic.Grid.GridBase.Grid[1][1].Piece = "X";
        gameLogic.Grid.GridBase.Grid[1][1].IsOccupied = true;
        gameLogic.Grid.GridBase.Grid[1][2].Piece = "O";
        gameLogic.Grid.GridBase.Grid[1][2].IsOccupied = true;
        gameLogic.Grid.GridBase.Grid[2][0].Piece = "O";
        gameLogic.Grid.GridBase.Grid[2][0].IsOccupied = true;
        gameLogic.Grid.GridBase.Grid[2][1].Piece = "X";
        gameLogic.Grid.GridBase.Grid[2][1].IsOccupied = true;
        gameLogic.Grid.GridBase.Grid[2][2].Piece = "O";
        gameLogic.Grid.GridBase.Grid[2][2].IsOccupied = true;
        isDraw = gameLogic.VictoryCalculator.GameIsTied();

        Assert.IsTrue(isDraw);
    }

    [Test]
    public void DynamicSize()
    {
        int size1 = 3;
        int size2 = 5;
        int size3 = 6;
        GameLogic gameLogic = new GameObject().AddComponent<GameLogic>();
        gameLogic.Grid = new GameObject().AddComponent<TicTacToeGridUnity>();
        gameLogic.Grid.GridBase = new TicTacToeGrid();

        gameLogic.Grid.GridBase.SetupGrid(size1);
        Assert.AreEqual(size1, gameLogic.Grid.GridBase.Size);

        gameLogic.Grid.GridBase.ResetGrid();
        gameLogic.Grid.GridBase.SetupGrid(size2);
        Assert.AreEqual(size2, gameLogic.Grid.GridBase.Size);

        gameLogic.Grid.GridBase.ResetGrid();
        gameLogic.Grid.GridBase.SetupGrid(size3);
        Assert.AreEqual(size3, gameLogic.Grid.GridBase.Size);
    }

    [Test]
    public void WinConditionTest()
    {
        int size = 5;
        int winCon = 3;
        GameLogic gameLogic = new GameObject().AddComponent<GameLogic>();
        gameLogic.Grid = new GameObject().AddComponent<TicTacToeGridUnity>();
        gameLogic.Grid.GridBase = new TicTacToeGrid();
        gameLogic.Grid.GridBase.SetupGrid(size);
        gameLogic.VictoryCalculator = new VictoryCalculator(gameLogic.Grid.GridBase, winCon);

        bool hasWon = false;
        gameLogic.Grid.GridBase.Grid[0][0].Piece = "O";
        gameLogic.Grid.GridBase.Grid[0][0].IsOccupied = true;
        gameLogic.Grid.GridBase.Grid[1][1].Piece = "O";
        gameLogic.Grid.GridBase.Grid[1][1].IsOccupied = true;
        gameLogic.Grid.GridBase.Grid[2][2].Piece = "O";
        gameLogic.Grid.GridBase.Grid[2][2].IsOccupied = true;
        hasWon = gameLogic.VictoryCalculator.ValueHasWon(2, 2);

        Assert.IsTrue(hasWon);
    }
}
