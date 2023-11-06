using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeGridUnity : MonoBehaviour
{
    public TicTacToeGrid GridBase;

    private GameLogic _gameLogic;
    private AssetHolder _assetHolder;
    public GameObject[][] SlotObjGrid;
    // Start is called before the first frame update
    void Start()
    {
        GridBase = new TicTacToeGrid();
        _gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        _assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupGrid(int size)
    {
        GridBase.SetupGrid(size);
        SlotObjGrid = new GameObject[size][];
        for (int x = 0; x < size; x++)
        {
            SlotObjGrid[x] = new GameObject[size];
        }
        GridBase.GoThroughGrid(CreateSlot);
    }

    void CreateSlot(int x, int y)
    {
        SlotObjGrid[x][y] = Instantiate(_assetHolder.SlotObjPrefab, new Vector3(x, 0, y), _assetHolder.SlotObjPrefab.transform.rotation);
        SlotObjGrid[x][y].GetComponent<SlotUnity>().x = x;
        SlotObjGrid[x][y].GetComponent<SlotUnity>().Slot.x = x;
        SlotObjGrid[x][y].GetComponent<SlotUnity>().y = y;
        SlotObjGrid[x][y].GetComponent<SlotUnity>().Slot.y = y;
        GridBase.Grid[x][y] = new Slot();
        GridBase.Grid[x][y].x = x;
        GridBase.Grid[x][y].y = y;
    }
    
    public void PlacePiece(int xCoordinate, int yCoordinate, Player player)
    {
        if(GridBase.PlacePiece(xCoordinate, yCoordinate, player))
        {
            SlotObjGrid[xCoordinate][yCoordinate].GetComponent<SlotUnity>().AttachPiece(player.Piece);
            _gameLogic.OnPiecePlaced(xCoordinate, yCoordinate, player);
        }
    }
}
