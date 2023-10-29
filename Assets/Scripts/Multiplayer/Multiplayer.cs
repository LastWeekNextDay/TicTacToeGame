using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Multiplayer : NetworkManager
{
    public float TimeoutTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool ClientWaitTimedOut(float timeFromLastFrame)
    {
        TimeoutTimer += timeFromLastFrame;
        if (TimeoutTimer < 10.0f) { return false; }
        TimeoutTimer = 0.0f;
        return true;
    }

    //public IEnumerator WaitForSecondClient(int size, int winCon)
    //{
    //    GameLogic gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
    //    while (gameLogic.Player2.gameObject.GetComponent<Client>() == null)
    //    {
    //        if (ClientWaitTimedOut(Time.deltaTime)) { yield break; };
    //        yield return null;
    //    }
    //    gameLogic.InitializeGame(size, winCon);
    //}
}
