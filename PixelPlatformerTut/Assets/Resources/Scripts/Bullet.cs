using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.isTrigger != true)
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponent<Player>().Damege(1);
            }

            Destroy(gameObject);
        }
    }
}
