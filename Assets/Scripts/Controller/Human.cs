using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Human : Player
{
    private InputController _inputController = null;

    private void Awake()
    {
        if (SessionInfo.Instance.Multiplayer)
        {
            gameObject.AddComponent<PhotonView>();
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        AI = false;
        if (SessionInfo.Instance.Multiplayer)
        {
            gameObject.AddComponent<PhotonView>();
        }
        _inputController = GetComponent<InputController>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (SessionInfo.Instance.Multiplayer)
        {
            if (GetComponent<PhotonView>().IsMine)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _inputController.HandleInput(Input.mousePosition, gameObject);
                }
            }
        }
    }
}
