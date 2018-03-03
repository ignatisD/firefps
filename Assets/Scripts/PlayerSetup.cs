//-------------------------------------
// Responsible for setting up the player.
// This includes adding/removing him correctly on the network.
//-------------------------------------

using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{

    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayerName = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPrefab;
    [HideInInspector]
    public GameObject playerUIInstance;

    void Start()
    {
        // Disable components that should only be
        // active on the player that we control
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            // Disable player graphics for local player
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            // Create PlayerUI
            playerUIInstance = Instantiate(playerUIPrefab) as GameObject;
            playerUIInstance.name = playerUIPrefab.name;

            // Configure PlayerUI
            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
                Debug.LogError("No PlayerUI component on PlayerUI prefab.");
            ui.SetPlayer(GetComponent<Player>());

            GetComponent<Player>().SetupPlayer();
            GameManager.singleton.localPlayer = gameObject;
            string _username = "Loading...";
            if (UserAccountManager.IsLoggedIn)
                _username = UserAccountManager.LoggedIn_Username;
            else
                _username = transform.name;

            CmdSetUsername(transform.name, _username);
        }
    }

    [Command]
    void CmdSetUsername(string playerID, string username)
    {
        Player player = GameManager.GetPlayer(playerID);
        if (player != null)
        {
            Debug.Log(username + " has joined!");
            player.username = username;
        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }


    public override void OnStartServer()
    {
        base.OnStartServer();
        if (!isServer)
            return;
        Debug.Log("OnStartServer");
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID, _player);
        _player.MiniSetup();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isServer)
        {
            // In case we are both a server and a client
            return;
        }
        Debug.Log("OnStartClient");

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID, _player);
        if (!isLocalPlayer)
        {
            _player.MiniSetup();
        }
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    // When we are destroyed
    void OnDisable()
    {

        if (isLocalPlayer)
        {
            Destroy(playerUIInstance);
            GameManager.singleton.SetSceneCameraActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);
    }

}
