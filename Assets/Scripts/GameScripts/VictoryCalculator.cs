using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCalculator
{
    private TicTacToeGrid _gridBase = null;
    public int WinCondition = 3;

    public VictoryCalculator(TicTacToeGrid grid, int winCondition)
    {
        _gridBase = grid;
        WinCondition = winCondition;
    }

    public bool GameIsTied()
    {
        return _gridBase.AllSlotsOccupied();
    }

    public bool ValueHasWon(int valueXCoordinate, int valueYCoordinate)
    {
        string valuePiece = _gridBase.Grid[valueXCoordinate][valueYCoordinate].Piece;
        if (valuePiece != "O" && valuePiece != "X") { return false; }

        int sameValuesInARow;
        // Check each slot that is adjacent to the provided slot
        for (int yCoordinateToCheck = valueYCoordinate - 1; yCoordinateToCheck <= valueYCoordinate + 1; yCoordinateToCheck++)
        {
            for (int xCoordinateToCheck = valueXCoordinate - 1; xCoordinateToCheck <= valueXCoordinate + 1; xCoordinateToCheck++)
            {
                // Skip slots that are out of bounds or are at the position of the provided slot
                if (xCoordinateToCheck < 0 ||
                    xCoordinateToCheck >= _gridBase.Size ||
                    yCoordinateToCheck < 0 ||
                    yCoordinateToCheck >= _gridBase.Size ||
                    (xCoordinateToCheck == valueXCoordinate && yCoordinateToCheck == valueYCoordinate))
                {
                    continue;
                }

                string adjacentPiece = _gridBase.Grid[xCoordinateToCheck][yCoordinateToCheck].Piece;
                if (adjacentPiece != valuePiece) continue;

                // If adjacent piece is equal to valuePiece, calculate the direction to check
                int deltaX = xCoordinateToCheck - valueXCoordinate;
                int deltaY = yCoordinateToCheck - valueYCoordinate;

                sameValuesInARow = 1; // Reset count for new direction
                sameValuesInARow += ValuesInARow(valueXCoordinate, valueYCoordinate, deltaX, deltaY);
                sameValuesInARow += ValuesInARow(valueXCoordinate, valueYCoordinate, -deltaX, -deltaY);

                if (sameValuesInARow >= WinCondition)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void SetGrid(TicTacToeGrid grid)
    {
        _gridBase = grid;
    }

    int ValuesInARow(int xCoordinate, int yCoordinate, int deltaX, int deltaY)
    {
        xCoordinate += deltaX;
        yCoordinate += deltaY;

        if (xCoordinate < 0 ||
            xCoordinate >= _gridBase.Size ||
            yCoordinate < 0 ||
            yCoordinate >= _gridBase.Size)
        {
            return 0;
        }

        string originalPiece = _gridBase.Grid[xCoordinate - deltaX][yCoordinate - deltaY].Piece;
        string currentPiece = _gridBase.Grid[xCoordinate][yCoordinate].Piece;

        if (originalPiece != currentPiece)
        {
            return 0;
        }

        return 1 + ValuesInARow(xCoordinate, yCoordinate, deltaX, deltaY);
    }
}
