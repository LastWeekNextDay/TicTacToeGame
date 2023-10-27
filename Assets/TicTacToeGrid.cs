using UnityEngine;

public class TicTacToeGrid : MonoBehaviour
{
    private Slot[][] _ticTacToeGrid;
    public int Size { get; private set; }

    public void Start()
    {

    }

    public void SetupGrid(int x)
    {
        // Create the grid of x size
        _ticTacToeGrid = new Slot[x][];
        for (int i = 0; i < x; i++)
        {
            _ticTacToeGrid[i] = new Slot[x];
        }
        Size = x;
        AssetHolder assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
        for (int x1 = 0; x1 < _ticTacToeGrid.Length; x1++)
        {
            for (int y = 0; y < _ticTacToeGrid[x1].Length; y++)
            {
                _ticTacToeGrid[x1][y] = Instantiate(assetHolder.SlotObjPrefab, new Vector3(x1, 0, y), Quaternion.identity, transform).GetComponent<Slot>();
                _ticTacToeGrid[x1][y].x = x1;
                _ticTacToeGrid[x1][y].y = y;
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

    bool Set(int x, int y, Player player)
    {
        return _ticTacToeGrid[x][y].AttachPiece(player.Piece);
    }

    public Slot Get(int x, int y)
    {
        return _ticTacToeGrid[x][y];
    }

    public void PlacePiece(int x, int y, Player player)
    {
        if (Set(x, y, player))
        {
            GameLogic gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
            gameLogic.OnPiecePlaced(x, y, player);
        }
    }
}
