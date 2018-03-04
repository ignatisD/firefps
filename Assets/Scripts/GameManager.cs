using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // Make it a singleton

    [Header("Console Settings")]
    public string serverAddress = "139.59.213.137";
    public string platform = "CONSOLE";
    public bool localOnly = false;
    public bool localMode = false;
    public bool runAsServer = false;
    public string loginScene = "LoginMenu";

    public static GameManager singleton;
    public string collidableEnvironement = "collidableEnvironement";

    public MatchSettings matchSettings;
    public GameObject[] playerPrefabs;

    [SerializeField]
    private GameObject sceneCamera;

    private Transform MainCameraTransform;

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    [HideInInspector]
    public GameObject localPlayer;

    public Vector3 defaultRespawnPosition = Vector3.zero;
    public Transform playerSpawnPosition;

    public GameObject canvas;


    public bool saveEnabled = false;


    public delegate void OnPlayerKilledCallback(string player, string source);
    public OnPlayerKilledCallback onPlayerKilledCallback;

    private NetworkManager networkManager;

    void Awake ()
    {
        if (singleton != null)
        {
            Debug.LogError("More than 1 GameManager in scene...");
            return;
        }
        singleton = this;
        DontDestroyOnLoad(this);
        if (sceneCamera == null && Camera.main != null)
        {
            sceneCamera = Camera.main.gameObject;
        }
        MainCameraTransform = sceneCamera.transform;
        // Show GUI
    }

    void Start()
    {
        MatchSettings settings = new MatchSettings(1); // defaults
        networkManager = NetworkManager.singleton;
        networkManager.playerPrefab = playerPrefabs[0];
        RegisterSettings(settings);
        RegisterStartPosition(defaultRespawnPosition, Quaternion.identity);

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("Web Platform detected!");
            platform = "WEB";
        }

        if (platform == "WEB")
        {
            NetworkManager.singleton.useWebSockets = true;
        }
        else
        {
            NetworkManager.singleton.useWebSockets = false;
        }

        if (runAsServer || IsHeadlessMode())
        {
            StartLocalServer();
        }
        else
        {
            SceneManager.LoadScene(loginScene);
        }
    }

    public void StartLocalServer()
    {
        runAsServer = true;
        NetworkManager.singleton.StartServer();
        DatabaseControl.singleton.joinRoom("7777");
    }

    #region Camera stuff

    public void SetSceneCameraActive(bool isActive)
    {
        if (sceneCamera == null)
        {
            if (Camera.main != null)
            {
                sceneCamera = Camera.main.gameObject;
            }else
            {
                Debug.LogError("No SceneCamera");
                return;
            }
        }

        sceneCamera.SetActive(isActive);
    }

    public void SetSceneCameraFollow(Transform LookAtTarget)
    {
        if (sceneCamera == null)
        {
            if (Camera.main != null)
            {
                sceneCamera = Camera.main.gameObject;
            }
            else
            {
                Debug.LogError("No SceneCamera");
                return;
            }
        }

        sceneCamera.GetComponent<UnityStandardAssets.Utility.SmoothFollow>().setTarget(LookAtTarget);
    }

    public void ResetSceneCamera()
    {
        sceneCamera.transform.position = MainCameraTransform.position;
        sceneCamera.transform.rotation = MainCameraTransform.rotation;
    }

    #endregion

    #region Player tracking

    

    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
        Debug.Log(_playerID);
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    public static Player[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }



    #endregion
    
    public void RegisterStartPosition(Vector3 _startPosition, Quaternion _startRotation)
    {

        playerSpawnPosition.position = _startPosition;
        playerSpawnPosition.rotation = _startRotation;

        NetworkManager.RegisterStartPosition(playerSpawnPosition);
    }

    public void RegisterSettings(MatchSettings _settings)
    {
        matchSettings = _settings;
    }


    public void ResetPosition()
    {
        if(playerSpawnPosition != null)
        {
            localPlayer.transform.position = playerSpawnPosition.position;
            localPlayer.transform.rotation = playerSpawnPosition.rotation;
        }
    }
    public static bool IsHeadlessMode()
    {
        Debug.Log(SystemInfo.graphicsDeviceType);
        return SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;
    }
}

