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
        TicTacToeGrid grid = GameObject.Find("Grid").GetComponent<TicTacToeGrid>();
        Vector3 middleSlot = grid.GetComponent<TicTacToeGrid>().Get(grid.GetComponent<TicTacToeGrid>().Size / 2, grid.GetComponent<TicTacToeGrid>().Size / 2).transform.position;
        int size = grid.GetComponent<TicTacToeGrid>().Size;
        transform.position = new Vector3(middleSlot.x, middleSlot.y + size, middleSlot.z);
    }
}
