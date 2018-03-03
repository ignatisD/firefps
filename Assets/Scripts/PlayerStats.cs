using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour {

	public Text killCount;
	public Text deathCount;

	// Use this for initialization
	void Start () {
		if (UserAccountManager.IsLoggedIn)
            UserAccountManager.singleton.GetUserData(UserAccountManager.LoggedIn_Email, OnReceivedData);
    }

    void OnReceivedData(Dictionary<string, string> data)
    {
        Debug.Log("Received data for " + UserAccountManager.LoggedIn_Username);
        if (killCount == null || deathCount == null)
            return;

        killCount.text = data["kills"] + " KILLS";
        deathCount.text = data["deaths"] + " DEATHS";
    }

}
