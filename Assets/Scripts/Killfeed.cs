using UnityEngine;
using System.Collections;

public class Killfeed : MonoBehaviour
{

    [SerializeField]
    GameObject killfeedItemPrefab;

    // Use this for initialization
    void Start()
    {
        GameManager.singleton.onPlayerKilledCallback += OnKill;
    }

    public void OnKill(string player, string source)
    {
        GameObject go = Instantiate(killfeedItemPrefab, transform) as GameObject;
        go.GetComponent<KillfeedItem>().Setup(player, source);

        Destroy(go, 4f);
    }
    
    private void OnDestroy()
    {
        GameManager.singleton.onPlayerKilledCallback -= OnKill;
    }
}
