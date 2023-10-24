using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool IsOccupied = false;
    private GameObject _pieceObjectAttached;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string Piece()
    {
        if (_pieceObjectAttached == null)
        {
            return null;
        } else
        {
            return _pieceObjectAttached.GetComponent<PieceObject>().Piece;
        } 
    }

    public bool Clear()
    {
        Destroy(_pieceObjectAttached);
        if (_pieceObjectAttached == null)
        {
            IsOccupied = true;
        }
        return IsOccupied;
    }

    public bool AttachPiece(string piece)
    {
        if (IsOccupied)
        {
            return false;
        } else
        {
            return true;
        }
    }
}
