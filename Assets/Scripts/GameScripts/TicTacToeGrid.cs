using Photon.Pun;
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

    public void SetAssetHolder(AssetHolder assetHolder)
    {
        _assetHolder = assetHolder;
    }

    public void SetupGrid(int size)
    {
        // Create the grid of x size
        _ticTacToeGrid = new Slot[size][];
        for (int xCoordinate = 0; xCoordinate < size; xCoordinate++)
        {
            _ticTacToeGrid[xCoordinate] = new Slot[size];
        }
        Size = size;
        GoThroughGrid(CreateSlot);
    }

    public bool AllSlotsOccupied() 
    {         
        bool allSlotsOccupied = true;
        GoThroughGrid((xCoordinate, yCoordinate) => 
        {
            Debug.Log("Checking slot " + xCoordinate + ", " + yCoordinate + " for occupancy: " + _ticTacToeGrid[xCoordinate][yCoordinate].IsOccupied);
            if (!_ticTacToeGrid[xCoordinate][yCoordinate].IsOccupied) { allSlotsOccupied = false; } 
        });
        return allSlotsOccupied;
    }

    public void ResetGrid()
    {
        GoThroughGrid(ClearSlot);
    }

    public Slot Get(int xCoordinate, int yCoordinate)
    {
        return _ticTacToeGrid[xCoordinate][yCoordinate];
    }

    public void PlacePiece(int xCoordinate, int yCoordinate, Player player)
    {
        if (Set(xCoordinate, yCoordinate, player)) { _gameLogic.OnPiecePlaced(xCoordinate, yCoordinate, player); }
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
        _ticTacToeGrid[x][y] = GameObject.Instantiate(_assetHolder.SlotObjPrefab, new Vector3(x, 0, y), _assetHolder.SlotObjPrefab.transform.rotation).GetComponent<Slot>();
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
