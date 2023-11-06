using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotUnity : MonoBehaviour
{
    private GameObject _pieceObjectAttached = null;
    private AssetHolder _assetHolder = null;
    public Slot Slot = new Slot();
    public int x;
    public int y;
    // Start is called before the first frame update
    void Start()
    {
        _assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Clear()
    {
        Destroy(_pieceObjectAttached);
        Slot.Clear();
        return Slot.IsOccupied;
    }

    public bool AttachPiece(string piece)
    {
        
        if (!Slot.IsOccupied)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition += new Vector3(0, 0.1f, 0);
            GameObject prefabX;
            GameObject prefabO;
            if (SessionInfo.Instance.Multiplayer)
            {
                prefabX = _assetHolder.XMPObjPrefab;
                prefabO = _assetHolder.OMPObjPrefab;
            }
            else
            {
                prefabX = _assetHolder.XObjPrefab;
                prefabO = _assetHolder.OObjPrefab;
            }
            if (piece == "X") { _pieceObjectAttached = _assetHolder.Spawn(prefabX, spawnPosition, transform); }
            if (piece == "O") { _pieceObjectAttached = _assetHolder.Spawn(prefabO, spawnPosition, transform); }
        }
        if (_pieceObjectAttached != null)
        {
            Slot.IsOccupied = true;
            StartCoroutine(PlayExplosion(_pieceObjectAttached.transform.position));
            StartCoroutine(PlaySound(_pieceObjectAttached.transform.position));
            if (SessionInfo.Instance.Multiplayer)
            {
                Networking NetworkManager = GameObject.Find("NetworkManager").GetComponent<Networking>();
                NetworkManager.PlayExplosion(_pieceObjectAttached.transform.position.x,
                                             _pieceObjectAttached.transform.position.y,
                                             _pieceObjectAttached.transform.position.z);
                NetworkManager.PlaySound(_pieceObjectAttached.transform.position.x,
                                         _pieceObjectAttached.transform.position.y,
                                         _pieceObjectAttached.transform.position.z);
                NetworkManager.SendOccup(Slot.x, Slot.y);
            }
        }
        return Slot.IsOccupied;
    }

    public static IEnumerator PlayExplosion(Vector3 position)
    {
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

    public static IEnumerator PlaySound(Vector3 position)
    {
        AssetHolder assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
        AudioSource boom = Instantiate(assetHolder.SoundExplosionPrefab, position, Quaternion.identity).GetComponent<AudioSource>();
        float duration = boom.clip.length;
        float now = 0f;
        while (now < duration)
        {
            now += Time.deltaTime;
            yield return null;
        }
        Destroy(boom.gameObject);
    }
}
