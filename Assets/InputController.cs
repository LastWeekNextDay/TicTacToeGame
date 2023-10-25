using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleInput(Vector3 mousePosition, GameObject initiator)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Slot slot = hit.collider.gameObject.GetComponent<Slot>();
            if (slot != null)
            {
                GameLogic gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
                if (initiator.GetComponent<Player>().Piece != gameLogic.Turn.Piece) { return;}
                if (!slot.IsOccupied)
                {
                    GameObject.Find("Grid").GetComponent<TicTacToeGrid>().PlacePiece(slot.x, slot.y, initiator.GetComponent<Player>());
                }
            }
        }
    }
}
