using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Host : NetworkManager
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

    public bool ClientWaitTimedOut(float timeFromLastFram)
    {
        TimeoutTimer += timeFromLastFram;
        if (TimeoutTimer > 10.0f)
        {
            TimeoutTimer = 0.0f;
            return true;
        }
        return false;
    }

    public IEnumerator WaitForSecondClient(int size, int winCon)
    {
        GameLogic gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        while (gameLogic.Player2.gameObject.GetComponent<Client>() == null)
        {
            if (ClientWaitTimedOut(Time.deltaTime)) { yield break; };
            yield return null;
        }
        gameLogic.InitializeGame(size, winCon);
    }
}
