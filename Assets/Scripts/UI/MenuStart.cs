using UnityEngine;

public class MenuStart : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject singlePlayer;
    public GameObject multiPlayer;
    public GameObject host;
    public GameObject join;

    // Start is called before the first frame update
    void Start()
    {
        if (mainMenu != null) mainMenu.SetActive(true);
        if (singlePlayer != null) singlePlayer.SetActive(false);
        if (multiPlayer != null) multiPlayer.SetActive(false);
        if (host != null) host.SetActive(false);
        if (join != null) join.SetActive(false);
    }
}
