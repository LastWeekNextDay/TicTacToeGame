using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetHolder : ScriptableObject
{
    public GameObject XPrefab = null;
    public GameObject OPrefab = null;
    public GameObject SlotPrefab = null;
    // Start is called before the first frame update
    void Start()
    {
        XPrefab = Resources.Load<GameObject>("XObj");
        OPrefab = Resources.Load<GameObject>("OObj");
        SlotPrefab = Resources.Load<GameObject>("SlotObj");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
