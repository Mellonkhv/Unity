using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;
    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;

    void Start()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public IEnumerator RespawnPlayer ()
    {
        Debug.Log("TODO: Add waiting for spawn sound");
        yield return new WaitForSeconds(spawnDelay);
        
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("TODO: Add Spawn Particles");
    }

	public static void KillPlayer (Player player)
    {
        Destroy(player.gameObject);
        gm.StartCoroutine(gm.RespawnPlayer());
    }
}
