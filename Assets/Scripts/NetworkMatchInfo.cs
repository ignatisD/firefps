using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMatchInfo {

    public string netID;
    public string roomName;
    public Dictionary<string, Player> players;

    public NetworkMatchInfo(string _netID)
    {
        netID = _netID;
        players = new Dictionary<string, Player>();
    }

    public NetworkMatchInfo(string _netID, string _roomName)
    {
        netID = _netID;
        roomName = _roomName;
        players = new Dictionary<string, Player>();
    }

    public void syncData(object data)
    {
        Debug.Log(data);
    }
    
    public Player GetPlayer(string player_username)
    {
        return players[player_username];
    }

    public void SyncPlayer(Player _player)
    {
        players[_player.username] = _player;
        UserAccountManager.singleton.syncPlayer(_player);
    }

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result["netID"] = netID;
        result["roomName"] = roomName;
        foreach (KeyValuePair<string, Player> _player in players)
        {
            result["/players/" + _player.Key] = _player.Value.ToDictionary();
        }

        return result;
    }
}
