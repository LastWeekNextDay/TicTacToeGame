using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool IsOccupied = false;
    private GameObject _pieceObjectAttached;
    private AssetHolder _assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();

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
            if (piece == "X")
            {
                _pieceObjectAttached = Instantiate(_assetHolder.XObjPrefab);
            }
            else if (piece == "O")
            {
                _pieceObjectAttached = Instantiate(_assetHolder.OObjPrefab);
            }
            if (_pieceObjectAttached != null)
            {
                ProperlyPositionPieceObject();
                IsOccupied = true;
            }
            return IsOccupied;
        }
    }

    void ProperlyPositionPieceObject()
    {
        _pieceObjectAttached.transform.parent = transform;
        _pieceObjectAttached.transform.position = transform.position;
        _pieceObjectAttached.transform.position += new Vector3(1, 0, 0);
    }
}