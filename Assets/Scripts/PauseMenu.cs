using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PauseMenu : MonoBehaviour {

	public static bool IsOn = false;

	private NetworkManager networkManager;

	void Start ()
	{
		networkManager = NetworkManager.singleton;
	}

	public void LeaveRoom ()
	{
        if (GameManager.singleton.platform == "WEB" || GameManager.singleton.localMode)
        { 
            networkManager.StopClient();
            networkManager.StopHost();
            return;
        }
		MatchInfo matchInfo = networkManager.matchInfo;
		networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
		networkManager.StopHost();
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void savePosition()
    {
        DatabaseControl.singleton.savePosition(GameManager.singleton.localPlayer);
    }

    public void resetPosition()
    {
        GameManager.singleton.ResetPosition();
    }
}
