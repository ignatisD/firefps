using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LevelBuilder : NetworkBehaviour
{


    public GameObject floor;
    public GameObject barel;
    public Vector3 faceUpRight = Vector3.zero;
    public int barels;

    public override void OnStartServer()
    {
        Debug.Log("Server");
        SpawnLevel();
    }
    
    void SpawnLevel()
    {

        barels = GameManager.singleton.matchSettings.barrelNum;
        Vector3 scale = GameManager.singleton.matchSettings.floorScale;
        int max = (int)scale.x;
        GameObject temp = Instantiate(floor, gameObject.transform) as GameObject;
        temp.transform.localScale = scale;
        NetworkServer.Spawn(temp);
        Quaternion faceUp = Quaternion.Euler(faceUpRight);
        for (int i = 0; i < barels; i++)
        {
            Vector3 barelPos = new Vector3(Random.Range(-max, max), 0, Random.Range(-max, max));
            temp = Instantiate(barel, barelPos, faceUp, gameObject.transform) as GameObject;
            NetworkServer.Spawn(temp);
        }
    }
}
