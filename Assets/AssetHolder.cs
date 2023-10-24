using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetHolder : ScriptableObject
{
    public GameObject XObjPrefab = null;
    public GameObject OObjPrefab = null;
    public GameObject SlotObjPrefab = null;
    // Start is called before the first frame update
    void Start()
    {
        XObjPrefab = Resources.Load<GameObject>("XObj");
        OObjPrefab = Resources.Load<GameObject>("OObj");
        SlotObjPrefab = Resources.Load<GameObject>("SlotObj");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
