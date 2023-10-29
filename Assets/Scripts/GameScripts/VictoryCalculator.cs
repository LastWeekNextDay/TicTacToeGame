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

    public bool GameIsTied()
    {
        return _grid.AllSlotsOccupied();
    }

    public bool ValueHasWon(int valueXCoordinate, int valueYCoordinate)
    {
        string valuePiece = _grid.Get(valueXCoordinate, valueYCoordinate).Piece();
        if (valuePiece != "O" && valuePiece != "X") { return false; }

        int sameValuesInARow;
        // Check each slot that is adjacent to the provided slot
        for (int yCoordinateToCheck = valueYCoordinate - 1; yCoordinateToCheck <= valueYCoordinate + 1; yCoordinateToCheck++)
        {
            for (int xCoordinateToCheck = valueXCoordinate - 1; xCoordinateToCheck <= valueXCoordinate + 1; xCoordinateToCheck++)
            {
                // Skip slots that are out of bounds or are at the position of the provided slot
                if (xCoordinateToCheck < 0 ||
                    xCoordinateToCheck >= _grid.Size ||
                    yCoordinateToCheck < 0 ||
                    yCoordinateToCheck >= _grid.Size ||
                    (xCoordinateToCheck == valueXCoordinate && yCoordinateToCheck == valueYCoordinate))
                {
                    continue;
                }

                string adjacentPiece = _grid.Get(xCoordinateToCheck, yCoordinateToCheck).Piece();
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

    int ValuesInARow(int xCoordinate, int yCoordinate, int deltaX, int deltaY)
    {
        xCoordinate += deltaX;
        yCoordinate += deltaY;

        if (xCoordinate < 0 ||
            xCoordinate >= _grid.Size ||
            yCoordinate < 0 ||
            yCoordinate >= _grid.Size)
        {
            return 0;
        }

        string originalPiece = _grid.Get(xCoordinate - deltaX, yCoordinate - deltaY).Piece();
        string currentPiece = _grid.Get(xCoordinate, yCoordinate).Piece();

        if (originalPiece != currentPiece)
        {
            return 0;
        }

        return 1 + ValuesInARow(xCoordinate, yCoordinate, deltaX, deltaY);
    }
}
