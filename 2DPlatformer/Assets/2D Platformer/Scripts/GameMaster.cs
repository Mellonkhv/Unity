using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;
    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 3.5f;
    public Transform spawnPrefab;

    public CameraShake cameraShake;

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    void Start()
    {
        if (cameraShake == null)
        {
            Debug.LogError("Нет подключенной камеры для встряски");
        }
    }

    public IEnumerator _RespawnPlayer ()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(spawnDelay);
        
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
        Destroy(clone, 3f);
    }

	public static void KillPlayer (Player player)
    {
        Destroy(player.gameObject);
        gm.StartCoroutine(gm._RespawnPlayer());
    }

    public static void KillEnemy (Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy enemy)
    {
        GameObject clone = Instantiate(enemy.deathParticles, enemy.transform.position, Quaternion.identity) as GameObject;
        Destroy(clone, 5f);
        cameraShake.Shake(enemy.shakeAmt, enemy.shakeLength);
        Destroy(enemy.gameObject);
    }
}
