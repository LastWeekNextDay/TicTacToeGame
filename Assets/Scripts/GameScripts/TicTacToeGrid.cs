using Photon.Pun;
using System;
using UnityEngine;

public class TicTacToeGrid
{
    public Slot[][] Grid;

    public int Size { get; private set; }

    public void SetupGrid(int size)
    {
        Grid = new Slot[size][];
        for (int xCoordinate = 0; xCoordinate < size; xCoordinate++)
        {
            Grid[xCoordinate] = new Slot[size];
        }
        GoThroughGrid(CreateEmptySlot);
        Size = size;
    }

    public bool AllSlotsOccupied() 
    {         
        bool allSlotsOccupied = true;
        GoThroughGrid((xCoordinate, yCoordinate) => 
        {
            if (!Grid[xCoordinate][yCoordinate].IsOccupied) { allSlotsOccupied = false; } 
        });
        return allSlotsOccupied;
    }

    void CreateEmptySlot(int x, int y)
    {
        Grid[x][y] = new Slot();
        Grid[x][y].x = x;
        Grid[x][y].y = y;
    }

    public void ResetGrid()
    {
        GoThroughGrid(ClearSlot);
    }

    public bool PlacePiece(int xCoordinate, int yCoordinate, Player player)
    {
        return SetPiece(xCoordinate, yCoordinate, player);
    }

    public void GoThroughGrid(Action<int,int> action)
    {
        for (int x = 0; x < Grid.Length; x++)
        {
            for (int y = 0; y < Grid[x].Length; y++)
            {
                action(x, y);
            }
        }
    }

    void ClearSlot(int x, int y)
    {
        Grid[x][y].Clear();
    }

    bool SetPiece(int x, int y, Player player)
    {
        Grid[x][y].Piece = player.Piece;
        Grid[x][y].IsOccupied = true;
        return Grid[x][y].Piece == player.Piece;
    }
}
