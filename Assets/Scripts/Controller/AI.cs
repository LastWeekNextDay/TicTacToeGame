using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AI : Player
{
    GameLogic _gameLogic;
    bool _makingMove = false;
    bool _justPlacedPiece = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        AI = true;
        _gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Piece != "X" && Piece != "O") { return; }
        if (!_makingMove)
        {
            if (_gameLogic.Turn.gameObject == this.gameObject)
            {
                if (!_justPlacedPiece)
                {
                    StartCoroutine(MakeRandomMove());
                }
            }
        }
    }

    public void Reset()
    {
        _makingMove = false;
        _justPlacedPiece = false;
    }

    IEnumerator MakeRandomMove()
    {
        _makingMove = true;
        _justPlacedPiece = false;
        TicTacToeGrid gridBase = _gameLogic.Grid.GridBase;
        int gridSize = gridBase.Size;
        if (gridSize < 1) { yield break; };
        Dictionary<int, List<int>> valuesXY = new Dictionary<int, List<int>>();
        for (int x = 0; x < gridSize; x++)
        {
            valuesXY.Add(x, new List<int>());
            for (int y = 0; y < gridSize; y++)
            {
                valuesXY[x].Add(y);
            }
        }
        while (valuesXY.Count > 0)
        {
            int x = Random.Range(0, valuesXY.Count);
            if (valuesXY.ContainsKey(x))
            {
                int y = Random.Range(0, valuesXY[x].Count);
                if (valuesXY[x].Contains(y))
                {
                    if (!gridBase.Grid[x][y].IsOccupied)
                    {
                        _gameLogic.Grid.PlacePiece(x, y, this);
                        break;
                    }
                    else
                    {
                        valuesXY[x].Remove(y);
                        if (valuesXY[x].Count == 0)
                        {
                            valuesXY.Remove(x);
                        }
                    }
                }
            }
            yield return null;
        }
        _makingMove = false;
        _justPlacedPiece = true;
    }
}
