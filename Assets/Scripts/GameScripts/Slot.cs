using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool IsOccupied = false;
    private GameObject _pieceObjectAttached = null;
    public int x;
    public int y;

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
        if (_pieceObjectAttached == null){ return null; }
        return _pieceObjectAttached.GetComponent<PieceObject>().Piece;
    }

    public bool Clear()
    {
        Destroy(_pieceObjectAttached);
        if (_pieceObjectAttached == null) { IsOccupied = false; }
        return IsOccupied;
    }

    public bool AttachPiece(string piece)
    {
        AssetHolder assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
        if (!IsOccupied)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition += new Vector3(0, 0.1f, 0);
            if (piece == "X") { _pieceObjectAttached = assetHolder.Spawn(assetHolder.XObjPrefab, spawnPosition, transform); }
            if (piece == "O") { _pieceObjectAttached = assetHolder.Spawn(assetHolder.OObjPrefab, spawnPosition, transform); }
        }
        if (_pieceObjectAttached != null) { IsOccupied = true; }
        return IsOccupied;
    }
}
