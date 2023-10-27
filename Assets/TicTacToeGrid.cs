using UnityEngine;

public class TicTacToeGrid
{
    private AssetHolder _assetHolder;
    private GameLogic _gameLogic;
    private Slot[][] _ticTacToeGrid;

    public int Size { get; private set; }

    public TicTacToeGrid(AssetHolder assetHolder, GameLogic gameLogic)
    {
        _assetHolder = assetHolder;
        _gameLogic = gameLogic;
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
        for (int x1 = 0; x1 < _ticTacToeGrid.Length; x1++)
        {
            for (int y = 0; y < _ticTacToeGrid[x1].Length; y++)
            {
                _ticTacToeGrid[x1][y] = _assetHolder.Spawn(_assetHolder.SlotObjPrefab, new Vector3(x1, 0, y)).GetComponent<Slot>();
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
            _gameLogic.OnPiecePlaced(x, y, player);
        }
    }
}
