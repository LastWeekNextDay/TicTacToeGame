using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetHolder : MonoBehaviour
{
    public GameObject XObjPrefab = null;
    public GameObject OObjPrefab = null;
    public GameObject SlotObjPrefab = null;
    public GameObject AIPlayerObjPrefab = null;
    public GameObject HumanPlayerObjPrefab = null;
    public GameObject NetworkManagerPrefab = null;
    // Start is called before the first frame update
    void Start()
    {
        XObjPrefab = Resources.Load<GameObject>("XObj");
        OObjPrefab = Resources.Load<GameObject>("OObj");
        SlotObjPrefab = Resources.Load<GameObject>("SlotObj");
        AIPlayerObjPrefab = Resources.Load<GameObject>("AIPlayerObj");
        HumanPlayerObjPrefab = Resources.Load<GameObject>("HumanPlayerObj");
        NetworkManagerPrefab = Resources.Load<GameObject>("NetworkManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Spawn(GameObject prefab, Vector3 position)
    {
        if (prefab == null) { return null; }
        if (SessionInfo.Instance.Multiplayer)
        {
            GameObject obj = PhotonNetwork.Instantiate(prefab.name, position, prefab.transform.rotation, 0);
            return obj;
        }
        return Instantiate(prefab, position, prefab.transform.rotation);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Transform parent)
    {
        if (prefab == null) { return null; }
        if (SessionInfo.Instance.Multiplayer)
        {
            GameObject obj = PhotonNetwork.Instantiate(prefab.name, position, prefab.transform.rotation, 0);
            obj.transform.parent = parent;
            return obj;
        }
        return Instantiate(prefab, position, prefab.transform.rotation, parent);
    }
}
