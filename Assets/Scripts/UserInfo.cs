using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserInfo
{

    public string email;
    public string username;
    public string userdata = "";
    public string created_at;
    public string updated_at;
    public int kills = 0;
    public int deaths = 0;

    public static UserInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<UserInfo>(jsonString);
    }

    public Dictionary<string, string> ToDictionary()
    {
        Dictionary<string, string> response = new Dictionary<string, string>();
        response["email"] = email;
        response["username"] = username;
        response["userdata"] = userdata;
        response["created_at"] = created_at;
        response["updated_at"] = updated_at;
        return response;
    }

    public int getKills()
    {
        return kills;
    }

    public int getDeaths()
    {
        return deaths;
    }

}
