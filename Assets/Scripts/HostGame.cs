using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class HostGame : MonoBehaviour
{

    [SerializeField]
    private uint roomSize = 6;

    private string roomName;

    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (GameManager.singleton.runAsServer || GameManager.singleton.localOnly || GameManager.singleton.platform == "WEB")
        {
            return;
        }
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string _name)
    {
        roomName = _name;
    }

    public void CreateRoom()
    {
        if (roomName != "" && roomName != null)
        {
            Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players.");
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, OnMatchCreate);
        }
    }

    public void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            GameManager.singleton.localMode = false;
            string _netId = matchInfo.networkId.ToString();
            DatabaseControl.singleton.createRoom(_netId, roomName);
        }
        networkManager.OnMatchCreate(success, extendedInfo, matchInfo);
    }

}
