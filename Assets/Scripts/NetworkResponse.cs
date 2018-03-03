using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NetworkResponse {
    public bool success;
    public LoginError errors;
    public UserInfo user;

    public Dictionary<string, string> ToDictionary()
    {
        Dictionary<string, string> response = new Dictionary<string, string>();
        if (success)
        {
            response["success"] = "true";
            response["errors"] = "";
        }
        else
        {
            response["success"] = "false";
            response["errors"] = errors.getError();
        }
        if(user != null)
        {
            response["user"] = "true";
            //response["email"] = user.email;
            //response["username"] = user.username;
            //response["userdata"] = user.userdata;
            //response["created_at"] = user.created_at;
            //response["updated_at"] = user.updated_at;
        }
        else
        {
            response["user"] = "false";
        }
        return response;
    }
}
