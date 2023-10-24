public class GridScript
{
    private string[][] _grid; 
    public int Size { get; private set; }

    public void SetGrid(int x)
    {
        _grid = new string[x][];
        for (int i = 0; i < x; i++)
        {
            _grid[i] = new string[x];
        }
        Size = x;
    }

    public void ResetGrid() {        
        for (int x = 0; x < _grid.Length; x++)
        {
            for (int y = 0; y < _grid[x].Length; y++)
            {
                _grid[x][y] = null;
            }
        }
    }

    bool PutValue(int x, int y, PlayerScript player)
    {
        if (_grid[x][y] == null) {
            _grid[x][y] = player.Piece;
            return true;
        }
        else
        {
            return false;
        }
    }

    public string Get(int x, int y)
    {
        return _grid[x][y];
    }
}
