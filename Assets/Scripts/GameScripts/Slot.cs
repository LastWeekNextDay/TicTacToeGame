using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Slot
{
    public bool IsOccupied = false;
    public string Piece;
    public int x;
    public int y;

    public bool Clear()
    {
        Piece = "";
        IsOccupied = false;
        return IsOccupied;
    }
}
