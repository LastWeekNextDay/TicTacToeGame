using UnityEngine;

public class Grid
{
    private Slot[][] _grid;
    public int Size { get; private set; }
    private AssetHolder _assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
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
        for (int x = 0; x < _grid.Length; x++)
        {
            for (int y = 0; y < _grid[x].Length; y++)
            {
                _grid[x][y].Clear();
            }
        }
    }

    public bool Set(int x, int y, Player player)
    {
        return _grid[x][y].AttachPiece(player.Piece);
    }

    public Slot Get(int x, int y)
    {
        return _grid[x][y];
    }
}
