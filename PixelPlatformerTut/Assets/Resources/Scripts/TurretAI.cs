using UnityEngine;
using System.Collections;

public class TurretAI : MonoBehaviour 
{
    public int CurHealth;
    public int MaxHealth;

    public float Distance;
    public float WakeRange;
    public float ShootInterval;
    public float BulletSpeed = 100;
    public float BulletTimer;

    public bool AwakeTurret = false;
    public bool LookingRight = true;

    public GameObject Bullet;
    public Transform Target;
    public Animator Anim;
    public Transform ShootPointLeft;
    public Transform ShootPointRight;
    private GameMaster _gm;

    void Awake()
    {
        Anim = gameObject.GetComponent<Animator>();
        _gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

	// Use this for initialization
	void Start () 
    {
        CurHealth = MaxHealth;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Anim.SetBool("Awake", AwakeTurret);
	    Anim.SetBool("LookingRight", LookingRight);

        RangeCheck();

	    LookingRight = Target.transform.position.x > transform.position.x;

	    if (CurHealth <= 0)
	    {
	        Destroy(gameObject);
	        _gm.points += 10;
	    }
    }

    void RangeCheck()
    {
        Distance = Vector3.Distance(transform.position, Target.transform.position);

        AwakeTurret = Distance < WakeRange;
    }

    public void Attack(bool attackingRight)
    {
        BulletTimer += Time.deltaTime;

        if (BulletTimer >= ShootInterval)
        {
            Vector2 direction = Target.transform.position - transform.position;
            direction.Normalize();
            if (!attackingRight)
            {
                var bulletClone = Instantiate(Bullet, ShootPointLeft.transform.position, ShootPointLeft.transform.rotation) as GameObject;
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * BulletSpeed;

                BulletTimer = 0;
            }

            if (attackingRight)
            {
                var bulletClone = Instantiate(Bullet, ShootPointRight.transform.position, ShootPointRight.transform.rotation) as GameObject;
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * BulletSpeed;

                BulletTimer = 0;
            }
        }
    }

    public void Damage(int damage)
    {
        CurHealth -= damage;
        gameObject.GetComponent<Animation>().Play("Player_RedFlash");
    }
}
