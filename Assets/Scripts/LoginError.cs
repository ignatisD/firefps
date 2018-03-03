using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoginError {

    public string login;
    public string email;
    public string action;

    public string register;
    public string username;
    public string password;
    public string password_confirm;
    public string exception;
    public string connection;

    public string inuse;
    public string failure;


    public string getError()
    {
        if (login != null)
            return login;
        if (email != null)
            return email;
        if (action != null)
            return action;
        if (register != null)
            return register;
        if (username != null)
            return username;
        if (password != null)
            return password;
        if (password_confirm != null)
            return password_confirm;
        if (exception != null)
            return exception;
        if (connection != null)
            return connection;
        if (failure != null)
            return failure;
        if (inuse != null)
            return inuse;

        return "Unknown error";
    }
    public static LoginError CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<LoginError>(jsonString);
    }
    
}
