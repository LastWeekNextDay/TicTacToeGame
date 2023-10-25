using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour
{
    public string Piece;

    public bool AI = false;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    // to string
    public override string ToString()
    {
        return Piece;
    }
}
