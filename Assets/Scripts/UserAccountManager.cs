using UnityEngine;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour {
    
    public static UserAccountManager singleton;
    void Awake ()
	{
		if (singleton != null)
		{
			Destroy(gameObject);
			return;
		}

        singleton = this;
		DontDestroyOnLoad(this);
	}

    public static string LoggedIn_Email { get; protected set; } //stores username once logged in
    public static string LoggedIn_Username { get; protected set; } //stores username once logged in
    public static string LoggedIn_Password { get; protected set; } //stores username once logged in
    public static string User_Info { get; protected set; } //stores username once logged in

    public static bool IsLoggedIn { get; protected set; }

	public string loggedInSceneName = "Lobby";
	public string loggedOutSceneName = "LoginMenu";
    
	public void LogOut ()
	{
		LoggedIn_Username = "";

		IsLoggedIn = false;

		Debug.Log("User logged out!");

		SceneManager.LoadScene(loggedOutSceneName);
	}

	public void LogIn(string email, string username, string password, string _info)
    {
        LoggedIn_Email = email;
        LoggedIn_Username = username;
        LoggedIn_Password = password;
        User_Info = _info;
        IsLoggedIn = true;
		SceneManager.LoadScene(loggedInSceneName);
	}

    public void GetUserData(string _email, DatabaseControl.DataReceivedEvent onUserData)
    {
        if (IsLoggedIn)
        {
            //ready to send request
            StartCoroutine(DatabaseControl.singleton.getDataRequest(LoggedIn_Email, LoggedIn_Password, onUserData)); //calls function to send get data request
        }
    }

    public void syncPlayer(Player _player)
    {
        User_Info = _player.userdata;
        StartCoroutine(DatabaseControl.singleton.storeDataRequest(LoggedIn_Email, LoggedIn_Password, _player)); //calls function to send get data request
    }

}
