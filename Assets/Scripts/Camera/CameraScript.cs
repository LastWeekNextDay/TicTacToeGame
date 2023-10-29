using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupCamera()
    {
        // Try and position the camera perpendicular to the grid and somewhere in the middle
        GameLogic gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        TicTacToeGrid grid = gameLogic.Grid;
        int size = grid.Size;
        Vector3 middleSlot = grid.Get(grid.Size / 2, grid.Size / 2).transform.position;
        transform.position = new Vector3(middleSlot.x, middleSlot.y + size, middleSlot.z);
    }
}
