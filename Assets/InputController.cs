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
        // This function will handle the mouse clicks by going through all possible variants of clicks
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (SlotPiecePlaceAttempt(hit, initiator)) { return; }
        }
    }

    bool SlotPiecePlaceAttempt(RaycastHit hit, GameObject initiator)
    {
        // Attempt to place a piece on a slot, even if unsuccessful, return true if a slot was clicked
        // However, return false if the game is not active or a slot was not clicked
        GameLogic gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        if (gameLogic.GameActive) {
            Slot slot = hit.collider.gameObject.GetComponent<Slot>();
            if (slot != null)
            {
                if (initiator.GetComponent<Player>().Piece == gameLogic.Turn.Piece)
                {
                    if (!slot.IsOccupied)
                    {
                        GameObject.Find("Grid").GetComponent<TicTacToeGrid>().PlacePiece(slot.x, slot.y, initiator.GetComponent<Player>());
                    }
                }
                return true;
            }
        }
        return false;
    }
}
