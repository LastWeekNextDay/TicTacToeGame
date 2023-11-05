using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionInfo : MonoBehaviour
{
    public static SessionInfo Instance { get; private set; }
    public bool Multiplayer;
    public string MultiplayerType;
    public int GridSize;
    public int WinCondition;
    public string RoomName;
    public string xo;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Destroy the new object if it is not the first
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
