
using UnityEngine;
//using UnityEngine.Networking;
//using System;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using Firebase;
//using Firebase.Database;
//using Firebase.Unity.Editor;
//using System.Collections;
//using UnityEngine.SceneManagement;

public class DatabaseManager : MonoBehaviour
{

    //    Firebase.Auth.FirebaseAuth auth;
    //    Firebase.Auth.FirebaseUser user;

    //    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    //    DatabaseReference RootRef;
    //    DataSnapshot playerSnapshot;
    //    DataSnapshot gameSnapshot;

    //    public static FireManager singleton;
    //    public string loginScene = "LoginMenu";
    //    private string username = "";
    //    private string password = "";
    //    public string roomName;
    //    public string networkMatchID;
    //    public static NetworkMatchInfo matchInfo;

    //    public delegate void AuthResponseEvent(string _username);
    //    public static event AuthResponseEvent authResponse;

    //    public delegate void DataReceivedEvent(Dictionary<string, string> userData);
    //    public delegate void DataReceivedEvent(string userData);
    //    public static event DataReceivedEvent onUserData;


    //    void Awake()
    //    {
    //        if (singleton != null)
    //        {
    //            Debug.LogError("More than 1 FireManager in scene...");
    //            return;
    //        }
    //        else
    //        {
    //            singleton = this;
    //            DontDestroyOnLoad(this);
    //        }
    //    }

    //    void Start()
    //    {

    //        dependencyStatus = FirebaseApp.CheckDependencies();
    //        if (!DependencyCheck())
    //        {
    //            FirebaseApp.FixDependenciesAsync().ContinueWith(task =>
    //            {
    //                dependencyStatus = FirebaseApp.CheckDependencies();
    //                if (DependencyCheck())
    //                {
    //                    InitializeFirebase();
    //                }
    //                else
    //                {
    //                    Debug.LogError(
    //                        "Could not resolve all Firebase dependencies: " + dependencyStatus);
    //                }
    //            });
    //        }
    //        else
    //        {
    //            InitializeFirebase();
    //        }
    //    }

    //    #region Firebase Login
    //    public bool DependencyCheck()
    //    {
    //        return dependencyStatus == DependencyStatus.Available;
    //    }

    //    void InitializeFirebase()
    //    {
    //        FirebaseApp app = FirebaseApp.DefaultInstance;
    //        app.SetEditorServiceAccountEmail("unity-177@unitytest-54c93.iam.gserviceaccount.com");
    //        app.SetEditorP12Password("notasecret");
    //        app.SetEditorP12FileName("UnityTest-185e7ac50893.p12");
    //        app.SetEditorDatabaseUrl("https://unitytest-54c93.firebaseio.com/");
    //        if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);

    //        RootRef = FirebaseDatabase.DefaultInstance.RootReference;

    //        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    //        auth.StateChanged += AuthStateChanged;
    //        AuthStateChanged(this, null);
    //        Enable UI for Login

    //        SceneManager.LoadScene(loginScene);
    //    }

    //    void AuthStateChanged(object sender, EventArgs eventArgs)
    //    {
    //        if (auth.CurrentUser != user)
    //        {
    //            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
    //            if (!signedIn && user != null)
    //            {
    //                Debug.Log("Signed out " + user.UserId);
    //            }
    //            user = auth.CurrentUser;
    //            if (signedIn)
    //            {
    //                username = user.UserId;
    //                Debug.Log("Signed in " + user.UserId);

    //                if (authResponse != null)
    //                    authResponse(username);
    //            }

    //        }
    //    }

    //    void OnDestroy()
    //    {
    //        auth.StateChanged -= AuthStateChanged;
    //        auth = null;
    //    }

    //    public void SignOut()
    //    {
    //        Debug.Log("Signing out.");
    //        auth.SignOut();
    //    }


    //    public void CreateUser(string _username, string _password)
    //    {
    //        Debug.Log(String.Format("Attempting to create user {0}...", _username));
    //        auth.CreateUserWithEmailAndPasswordAsync(_username, password);
    //    }


    //    public void Signin(string _username, string _password)
    //    {
    //        username = _username;
    //        password = _password;

    //        Debug.Log(String.Format("Attempting to sign in as {0}...", username));
    //        auth.SignInWithEmailAndPasswordAsync(username, password);
    //    }
    //    #endregion

    //    public void syncPlayer(Player _player)
    //    {
    //        if (networkMatchID == null)
    //        {
    //            Debug.Log("networkMatchID is null");
    //            return;
    //        }
    //        Dictionary<string, object> playerData = _player.ToDictionary();
    //        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
    //        childUpdates["/games/" + networkMatchID + "/" + _player.username] = playerData;
    //        childUpdates["/players/" + _player.username] = playerData;
    //        Debug.Log("Updating roomData... ");
    //        RootRef.UpdateChildrenAsync(childUpdates)
    //            .ContinueWith(task =>
    //        {
    //            if (task.IsFaulted)
    //            {
    //                Debug.LogWarning(task.Exception);
    //            }
    //            else if (task.IsCompleted)
    //            {
    //                Debug.Log("Sync success");
    //            }
    //        });
    //    }

    //    public void GetUserData(string _username, DataReceivedEvent onUserData)
    //    {
    //        Debug.Log("Requesting data for " + _username);
    //        RootRef
    //            .Child("players")
    //            .Child(_username)
    //            .GetValueAsync().ContinueWith(task =>
    //            {
    //                if (task.IsFaulted)
    //                {
    //                    Debug.LogWarning("Error with task");
    //                }
    //                else if (task.IsCompleted)
    //                {
    //                    setStartPosition(task.Result);
    //                    Dictionary<string, string> userData = new Dictionary<string, string>();
    //                    if (task.Result.Child("kills").Exists)
    //                    {
    //                        userData["kills"] = task.Result.Child("kills").Value.ToString();
    //                    }
    //                    else
    //                    {
    //                        userData["kills"] = "0";
    //                    }
    //                    if (task.Result.Child("deaths").Exists)
    //                    {
    //                        userData["deaths"] = task.Result.Child("kills").Value.ToString();
    //                    }
    //                    else
    //                    {
    //                        userData["deaths"] = "0";
    //                    }
    //                    if (onUserData != null)
    //                        onUserData.Invoke(userData);
    //                }
    //            });
    //    }

    //    public void joinRoom(string _netId)
    //    {
    //        networkMatchID = _netId;
    //        matchInfo = new NetworkMatchInfo(_netId);
    //        Debug.Log("Joining Room... ");
    //        RootRef
    //            .Child("games")
    //            .Child(networkMatchID)
    //            .ValueChanged += RoomDataValueChanged;
    //    }

    //    public void createRoom(string _netId, string _roomName)
    //    {
    //        roomName = _roomName;
    //        networkMatchID = _netId;
    //        matchInfo = new NetworkMatchInfo(_netId, _roomName);
    //        Dictionary<string, object> roomData = matchInfo.ToDictionary();
    //        Debug.Log("Creating Room... ");
    //        RootRef
    //            .Child("games")
    //            .Child(networkMatchID)
    //            .UpdateChildrenAsync(roomData)
    //            .ContinueWith(task =>
    //            {
    //                if (task.IsFaulted)
    //                {
    //                    Debug.LogWarning(task.Exception);
    //                    Debug.LogWarning(matchInfo.ToString());
    //                }
    //                else if (task.IsCompleted)
    //                {
    //                    Debug.Log("Create room success");
    //                    RootRef
    //                        .Child("games")
    //                        .Child(networkMatchID)
    //                        .ValueChanged += RoomDataValueChanged;
    //                }
    //            });
    //    }

    //    private void RoomDataValueChanged(object sender, ValueChangedEventArgs args)
    //    {
    //        if (args.DatabaseError != null)
    //        {
    //            Debug.LogError(args.DatabaseError.Message);
    //            return;
    //        }

    //        matchInfo.syncData(args.Snapshot.Value);
    //    }

    //    public void playerLogin()
    //    {

    //        RootRef
    //            .Child("players")
    //            .Child(playerUsername)
    //            .GetValueAsync().ContinueWith(task =>
    //            {
    //                if (task.IsFaulted)
    //                {
    //                    Debug.LogError("Error with task");
    //                }
    //                else if (task.IsCompleted)
    //                {
    //                    setStartPosition(task.Result);
    //                }
    //            });
    //    }


    //    public void setStartPosition(DataSnapshot childSnapshot)
    //    {
    //        Vector3 respawn = gameManager.defaultRespawnPosition;
    //        Quaternion ang = Quaternion.identity;


    //        if (childSnapshot.Child("respawnPosition").Exists)
    //        {
    //            respawn = JsonUtility.FromJson<Vector3>(childSnapshot.Child("respawnPosition").GetRawJsonValue());
    //        }
    //        if (childSnapshot.Child("position").Exists)
    //        {
    //            respawn = JsonUtility.FromJson<Vector3>(childSnapshot.Child("position").GetRawJsonValue());
    //        }
    //        if (childSnapshot.Child("rotation").Exists)
    //        {
    //            ang = JsonUtility.FromJson<Quaternion>(childSnapshot.Child("rotation").GetRawJsonValue());
    //        }
    //        playerUsername = childSnapshot.Child("username").Value.ToString();

    //        gameManager.registerStartPosition(respawn, ang);
    //        // Lets say from GUI or db that we have the <string> name of the match we want to play
    //        fetchRooms();
    //    }



    //    private void fetchRooms()
    //    {

    //        Debug.Log("Fetching Rooms");
    //        RootRef
    //            .Child("rooms")
    //            .GetValueAsync().ContinueWith(task =>
    //            {
    //                if (task.IsFaulted)
    //                {
    //                    Debug.LogError("Error with task");
    //                    enableLobbyGUI(null);
    //                }
    //                else if (task.IsCompleted)
    //                {
    //                    enableLobbyGUI(task.Result);
    //                }
    //            });
    //    }


    //    private void enableLobbyGUI(DataSnapshot childSnapshot)
    //    {

    //        gameManager.roomName = "default";
    //        MatchSettings settings = new MatchSettings();
    //        gameManager.RegisterSettings(settings);
    //    }


    //    public void savePosition(GameObject _player)
    //    {
    //        Player localPlayer = _player.GetComponent<Player>();
    //        GameManager.singleton.RegisterStartPosition(_player.transform.position, _player.transform.rotation);
    //        Debug.Log("Saving respawn position!");
    //        DatabaseReference userRef = RootRef
    //            .Child("players")
    //            .Child(localPlayer.username);

    //        userRef.Child("position")
    //            .SetRawJsonValueAsync(JsonUtility.ToJson(_player.transform.position));
    //        userRef.Child("rotation")
    //            .SetRawJsonValueAsync(JsonUtility.ToJson(_player.transform.rotation));
    //    }

}
