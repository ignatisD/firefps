using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseControl : MonoBehaviour {

    public static DatabaseControl singleton;

    [SerializeField]
    string account_api = "https://games.ignatisd.gr/api.php";

    public static NetworkMatchInfo matchInfo;


    public delegate void DataReceivedEvent(Dictionary<string, string> userData);
    public static DataReceivedEvent onUserData;

    public string roomName;
    public string networkMatchID;


    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("More than 1 DatabaseControl in scene...");
            return;
        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start () {
      
    }

    public void savePosition(GameObject _player)
    {

    }

    public void joinRoom(string _netId)
    {
        networkMatchID = _netId;
        matchInfo = new NetworkMatchInfo(_netId);
        Debug.Log("Joining Room... ");
    }

    public void createRoom(string _netId, string _roomName)
    {
        roomName = _roomName;
        networkMatchID = _netId;
        matchInfo = new NetworkMatchInfo(_netId, _roomName);
     //   Dictionary<string, object> roomData = matchInfo.ToDictionary();
        Debug.Log("Creating Room... " + _roomName);
    }

    public IEnumerator getDataRequest(string _email, string _password, DataReceivedEvent onUserData)
    {
        // Create a form object
        WWWForm form = new WWWForm();

        form.AddField("email", _email);
        form.AddField("password", _password);
        form.AddField("action", "login");

        // Create a download object
        WWW returned = new WWW(account_api, form);

        // Wait until the download is done
        yield return returned;

        if (!string.IsNullOrEmpty(returned.error))
        {
            Debug.Log("Error downloading: " + returned.error);
            yield break;
        }
      //  Debug.Log(returned.text);
        NetworkResponse response = JsonUtility.FromJson<NetworkResponse>(returned.text);
        Dictionary<string, string> results = response.ToDictionary();

        if (results["success"] == "true" && response.user != null)
        {
            Dictionary<string, string> userData = new Dictionary<string, string>();
            int kills = 0;
            int deaths = 0;
            if (response.user.userdata.Length > 0)
            {
                UserInfo _userData = JsonUtility.FromJson<UserInfo>(response.user.userdata);
                kills = _userData.getKills();
                deaths = _userData.getDeaths();

            }
            userData["kills"] = kills.ToString();
            userData["deaths"] = deaths.ToString();
            onUserData.Invoke(userData);
        }
    }

    public IEnumerator storeDataRequest(string _email, string _password, Player _player)
    {
        // Create a form object
        WWWForm form = new WWWForm();

        form.AddField("email", _email);
        form.AddField("password", _password);
        form.AddField("userdata", _player.userdata);
        form.AddField("action", "store");

        // Create a download object
        WWW returned = new WWW(account_api, form);

        // Wait until the download is done
        yield return returned;

        if (!string.IsNullOrEmpty(returned.error))
        {
            Debug.Log("Error downloading: " + returned.error);
            yield break;
        }
       // Debug.Log(returned.text);
        NetworkResponse response = JsonUtility.FromJson<NetworkResponse>(returned.text);
        Dictionary<string, string> results = response.ToDictionary();

        if (results["success"] != "true")
        {
            Debug.Log(results["errors"]);
        }
    }

}
