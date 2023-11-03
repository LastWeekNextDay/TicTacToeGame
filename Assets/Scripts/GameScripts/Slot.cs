using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool IsOccupied = false;
    private GameObject _pieceObjectAttached = null;
    public int x;
    public int y;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string Piece()
    {
        if (_pieceObjectAttached == null){ return null; }
        return _pieceObjectAttached.GetComponent<PieceObject>().Piece;
    }

    public bool Clear()
    {
        Destroy(_pieceObjectAttached);
        if (_pieceObjectAttached == null) { IsOccupied = false; }
        return IsOccupied;
    }

    public bool AttachPiece(string piece)
    {
        AssetHolder assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
        if (!IsOccupied)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition += new Vector3(0, 0.1f, 0);
            GameObject prefabX;
            GameObject prefabO;
            if (SessionInfo.Instance.Multiplayer)
            {
                prefabX = assetHolder.XMPObjPrefab;
                prefabO = assetHolder.OMPObjPrefab;
            }
            else
            {
                prefabX = assetHolder.XObjPrefab;
                prefabO = assetHolder.OObjPrefab;
            }
            if (piece == "X") { _pieceObjectAttached = assetHolder.Spawn(prefabX, spawnPosition, transform); }
            if (piece == "O") { _pieceObjectAttached = assetHolder.Spawn(prefabO, spawnPosition, transform); }
        }
        if (_pieceObjectAttached != null) {
            StartCoroutine(PlayExplosion(_pieceObjectAttached.transform.position));
            if (SessionInfo.Instance.Multiplayer)
            {
                GameObject.Find("NetworkManager").GetComponent<Networking>().PlayExplosion(_pieceObjectAttached.transform.position.x,
                                                                                            _pieceObjectAttached.transform.position.y,
                                                                                            _pieceObjectAttached.transform.position.z);
            }
            IsOccupied = true; 
        }
        return IsOccupied;
    }

    public static IEnumerator PlayExplosion(Vector3 position) {
        AssetHolder assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
        ParticleSystem boom = Instantiate(assetHolder.EnergyExplosionPrefab, position, Quaternion.identity).GetComponent<ParticleSystem>();
        float duration = boom.main.duration;
        float now = 0f;
        while (now < duration)
        {
            now += Time.deltaTime;
            yield return null;
        }
        Destroy(boom.gameObject);
    }
}
