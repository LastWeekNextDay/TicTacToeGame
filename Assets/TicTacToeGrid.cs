using UnityEngine;

public class TicTacToeGrid : MonoBehaviour
{
    private Slot[][] _ticTacToeGrid;
    public int Size { get; private set; }
    private AssetHolder _assetHolder = null;

    public void Start()
    {
        _assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
    }
    public void SetupGrid(int x)
    {
        //_grid = new string[x][];
        //for (int i = 0; i < x; i++)
        //{
        //    _grid[i] = new string[x];
        //}
        Size = x;
    }

    public void ResetGrid() {        
        for (int x = 0; x < _ticTacToeGrid.Length; x++)
        {
            for (int y = 0; y < _ticTacToeGrid[x].Length; y++)
            {
                _ticTacToeGrid[x][y].Clear();
            }
        }
    }

    public bool Set(int x, int y, Player player)
    {
        return _ticTacToeGrid[x][y].AttachPiece(player.Piece);
    }

    public Slot Get(int x, int y)
    {
        return _ticTacToeGrid[x][y];
    }

    void PlacePiece(int x, int y, Player player)
    {
        GameLogic gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        if (Set(x, y, player))
        {
            if (gameLogic.VictoryCalculator.ValueHasWon(x, y))
            {
                Debug.Log("Player " + player.Piece + " has won!");
            }
            else
            {
                gameLogic.ChangeTurn();
            }
        }
    }
}
