using UnityEngine;

public class TicTacToeGrid : MonoBehaviour
{
    private Slot[][] _ticTacToeGrid;
    public int Size { get; private set; }
    private AssetHolder _assetHolder = null;

    public void Start()
    {
        _assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
        SetupGrid(3);
    }
    public void SetupGrid(int x)
    {
        _ticTacToeGrid = new Slot[x][];
        for (int i = 0; i < x; i++)
        {
            _ticTacToeGrid[i] = new Slot[x];
        }
        Size = x;
        for (int x1 = 0; x1 < _ticTacToeGrid.Length; x1++)
        {
            for (int y = 0; y < _ticTacToeGrid[x1].Length; y++)
            {
                Instantiate(_assetHolder.SlotObjPrefab, new Vector3(x1, 0, y), Quaternion.identity, transform);
            }
        }
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
