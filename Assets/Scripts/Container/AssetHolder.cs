using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetHolder : MonoBehaviour
{
    public GameObject XObjPrefab = null;
    public GameObject XMPObjPrefab = null;
    public GameObject OObjPrefab = null;
    public GameObject OMPObjPrefab = null;
    public GameObject SlotObjPrefab = null;
    public GameObject SlotMPObjPrefab = null;
    public GameObject AIPlayerObjPrefab = null;
    public GameObject HumanPlayerObjPrefab = null;
    public GameObject HumanPlayerMPObjPrefab = null;
    public GameObject NetworkManagerPrefab = null;
    // Start is called before the first frame update
    void Start()
    {
        XObjPrefab = Resources.Load<GameObject>("XObj");
        XMPObjPrefab = Resources.Load<GameObject>("XMPObj");
        OObjPrefab = Resources.Load<GameObject>("OObj");
        OMPObjPrefab = Resources.Load<GameObject>("OMPObj");
        SlotObjPrefab = Resources.Load<GameObject>("SlotObj");
        SlotMPObjPrefab = Resources.Load<GameObject>("SlotMPObj");
        AIPlayerObjPrefab = Resources.Load<GameObject>("AIPlayerObj");
        HumanPlayerObjPrefab = Resources.Load<GameObject>("HumanPlayerObj");
        HumanPlayerMPObjPrefab = Resources.Load<GameObject>("HumanPlayerMPObj");
        NetworkManagerPrefab = Resources.Load<GameObject>("NetworkManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public GameObject Spawn(GameObject prefab, Vector3 position)
    {
        if (prefab == null) { return null; }
        if (SessionInfo.Instance.Multiplayer)
        {
            return PhotonNetwork.Instantiate(prefab.name, position, prefab.transform.rotation);
        }
        return Instantiate(prefab, position, prefab.transform.rotation);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Transform parent)
    {
        if (prefab == null) { return null; }
        if (SessionInfo.Instance.Multiplayer)
        {
            return PhotonNetwork.Instantiate(prefab.name, position, prefab.transform.rotation);
        }
        return Instantiate(prefab, position, prefab.transform.rotation, parent); ;
    }
}
