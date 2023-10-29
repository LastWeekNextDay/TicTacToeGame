using System;
using UnityEngine;

public class TicTacToeGrid
{
    private AssetHolder _assetHolder;
    private GameLogic _gameLogic;
    private Slot[][] _ticTacToeGrid;

    public int Size { get; private set; }

    public TicTacToeGrid(AssetHolder assetHolder, GameLogic gameLogic)
    {
        _assetHolder = assetHolder;
        _gameLogic = gameLogic;
    }

    public void SetupGrid(int x)
    {
        // Create the grid of x size
        _ticTacToeGrid = new Slot[x][];
        for (int i = 0; i < x; i++)
        {
            _ticTacToeGrid[i] = new Slot[x];
        }
        Size = x;
        GoThroughGrid(CreateSlot);
    }

    public bool AllSlotsOccupied() 
    {         
        bool allSlotsOccupied = true;
        GoThroughGrid((x, y) => { if (!_ticTacToeGrid[x][y].IsOccupied) { allSlotsOccupied = false; } });
        return allSlotsOccupied;
    }

    public void ResetGrid()
    {
        GoThroughGrid(ClearSlot);
    }

    public Slot Get(int x, int y)
    {
        return _ticTacToeGrid[x][y];
    }

    public void PlacePiece(int x, int y, Player player)
    {
        if (Set(x, y, player)) { _gameLogic.OnPiecePlaced(x, y, player); }
    }

    void GoThroughGrid(Action<int,int> action)
    {
        for (int x = 0; x < _ticTacToeGrid.Length; x++)
        {
            for (int y = 0; y < _ticTacToeGrid[x].Length; y++)
            {
                action(x, y);
            }
        }
    }

    void CreateSlot(int x, int y)
    {
        _ticTacToeGrid[x][y] = _assetHolder.Spawn(_assetHolder.SlotObjPrefab, new Vector3(x, 0, y)).GetComponent<Slot>();
        _ticTacToeGrid[x][y].x = x;
        _ticTacToeGrid[x][y].y = y;
    }

    void ClearSlot(int x, int y)
    {
        _ticTacToeGrid[x][y].Clear();
    }

    bool Set(int x, int y, Player player)
    {
        return _ticTacToeGrid[x][y].AttachPiece(player.Piece);
    }
}
