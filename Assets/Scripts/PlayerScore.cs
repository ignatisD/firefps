using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerScore : MonoBehaviour {

    public int lastKills = 0;
    public int lastDeaths = 0;

    Player player;

    void Start()
    {
        player = GetComponent<Player>();
        player.userdata = UserAccountManager.User_Info;
        StartCoroutine(SyncScoreLoop());
    }

    IEnumerator SyncScoreLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            SyncNow();
        }
    }

    void SyncNow()
    {
        if (UserAccountManager.IsLoggedIn)
        {
            if (player.kills <= lastKills && player.deaths <= lastDeaths)
                return;

            int killsSinceLast = player.kills - lastKills;
            int deathsSinceLast = player.deaths - lastDeaths;
            
            UserInfo _userData = JsonUtility.FromJson<UserInfo>(player.userdata);
            int kills = _userData.getKills();
            int deaths = _userData.getDeaths();

            int newKills = killsSinceLast + kills;
            int newDeaths = deathsSinceLast + deaths;

            player.userdata = "{\"kills\":" + newKills + ",\"deaths\":" + newDeaths + "}";
          //  Debug.Log(player.userdata);
            DatabaseControl.matchInfo.SyncPlayer(player);

            lastKills = player.kills;
            lastDeaths = player.deaths;
        }
    }

    private void OnDestroy()
    {
        if(player != null)
        SyncNow();
    }
}
