using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LobbyMenuManager : MonoBehaviour {

    public Text usernameText;
    public GameObject joinGameMenu;
    public GameObject hostGameMenu;
    public GameObject roomNameInput;

    private NetworkManager networkManager;


    public Text serverAddress;
    public GameObject joinLanGameMenu;
    public GameObject hostLanGameBtn;
    public InputField changeServerAddressInput;
    public GameObject changeServerAddress;
    public GameObject Loading;
    public Text loadingText;
    // Use this for initialization
    EventSystem system;

    void Start()
    {
        system = EventSystem.current;
        networkManager = NetworkManager.singleton;
        if (GameManager.singleton.platform == "WEB" || GameManager.singleton.localOnly == true)
        {
            joinGameMenu.SetActive(false);
            hostGameMenu.SetActive(false);
            ApplyServerAddressToText();
            Loading.SetActive(false);
            joinLanGameMenu.SetActive(true);
            if (GameManager.singleton.platform != "WEB")
                hostLanGameBtn.SetActive(true);
        }
        else
        {
            system.SetSelectedGameObject(roomNameInput, new BaseEventData(system));
        }
        if (UserAccountManager.IsLoggedIn)
            usernameText.text = UserAccountManager.LoggedIn_Username;

    }

    void Update()
    {
        if(GameManager.singleton.platform != "WEB" && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.BackQuote))
        {
            ApplyServerAddressToText();
            Loading.SetActive(false);
            changeServerAddress.SetActive(false);
            joinLanGameMenu.SetActive(true);
            if (GameManager.singleton.platform != "WEB")
                hostLanGameBtn.SetActive(true);
        }
        if (GameManager.singleton.platform != "WEB" && Input.GetKeyDown(KeyCode.Return) && changeServerAddress.activeSelf == true)
        {
            ChangeServerAddress();
        }
    }

    public void LogOut()
    {
        if (UserAccountManager.IsLoggedIn)
            UserAccountManager.singleton.LogOut();
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ApplyServerAddressToText()
    {
        serverAddress.text = "Server Address: " + GameManager.singleton.serverAddress;
    }

    public void showChangeServerAddress()
    {
        joinLanGameMenu.SetActive(false);
        changeServerAddress.SetActive(true);
        if (Loading.activeSelf == true)
        {
            StopCoroutine(WaitForJoin(0));
            Loading.SetActive(false);
        }
        system.SetSelectedGameObject(changeServerAddressInput.gameObject, new BaseEventData(system));
    }

    public void StopJoinLocalGame()
    {
        if (Loading.activeSelf == true)
        {
            StopCoroutine(WaitForJoin(0));
        }
        hideChangeServerAddress();
        networkManager.StopClient();
        networkManager.StopHost();
    }

    public void hideChangeServerAddress()
    {
        Loading.SetActive(false);
        changeServerAddress.SetActive(false);
        joinLanGameMenu.SetActive(true);
    }

    public void ChangeServerAddress()
    {
        GameManager.singleton.serverAddress = changeServerAddressInput.text;
        ApplyServerAddressToText();
        hideChangeServerAddress();
    }

    public void HostLocalGame()
    {
        GameManager.singleton.localMode = true;
        GameManager.singleton.serverAddress = "127.0.0.1";
        networkManager.networkAddress = GameManager.singleton.serverAddress;
        networkManager.StartHost();
        DatabaseControl.singleton.joinRoom("7777");
        StartCoroutine(WaitForJoin(3));
    }

    public void JoinLocalMatch()
    {
        GameManager.singleton.localMode = true;
        networkManager.networkAddress = GameManager.singleton.serverAddress;
        networkManager.StartClient();
        DatabaseControl.singleton.joinRoom("7777");
        StartCoroutine(WaitForJoin(10));
    }

    IEnumerator WaitForJoin(int _count)
    {
        joinLanGameMenu.SetActive(false);
        Loading.SetActive(true);
        int countdown = _count;
        while (countdown > 0)
        {
            loadingText.text = "JOINING... (" + countdown + ")";

            yield return new WaitForSeconds(1);

            countdown--;
        }

        // Failed to connect
        loadingText.text = "Failed to connect.";
        yield return new WaitForSeconds(1);

        Loading.SetActive(false);
        StopJoinLocalGame();
    }

}
