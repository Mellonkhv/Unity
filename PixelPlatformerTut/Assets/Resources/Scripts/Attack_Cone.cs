using UnityEngine;
using System.Collections;

public class Attack_Cone : MonoBehaviour
{

    public TurretAI TurretAi;

    public bool IsLeft = false;

    void Awake()
    {
        TurretAi = gameObject.GetComponentInParent<TurretAI>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            TurretAi.Attack(!IsLeft);
    }

}
