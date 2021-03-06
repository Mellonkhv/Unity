﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float MaxSpeed = 3f;
    public float Speed = 50f;
    public float JumpPower = 150f;

    public bool Grounded;
    public bool CanDoubleJump;
    public bool WallSliding;
    public bool facingRight = true;

    public int CurHealth;
    public int MaxHealth = 5;

    private Rigidbody2D _rb2D;
    private Animator _anim;
    private GameMaster _gm;
    public Transform wallCheckPoint;
    public bool wallCheck;
    public LayerMask WallLayerMask;

	// Use this for initialization
	void Start ()
	{
	    _rb2D = gameObject.GetComponent<Rigidbody2D>();
	    _anim = gameObject.GetComponent<Animator>();
	    _gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();

	    CurHealth = MaxHealth;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    _anim.SetBool("Grounded", Grounded);
        _anim.SetFloat("Speed", Mathf.Abs(_rb2D.velocity.x));

	    if (Input.GetAxis("Horizontal") < -0.1f)
	    {
	        transform.localScale = new Vector3(-1, 1, 1);
	        facingRight = false;
	    }
        
        else if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }

	    if (Input.GetButtonDown("Jump") && !WallSliding)
	    {
	        if (Grounded)
	        {
	            _rb2D.AddForce(Vector2.up * JumpPower);
	            CanDoubleJump = true;
	        }
	        else
	        {
	            if (CanDoubleJump)
	            {
	                CanDoubleJump = false;
                    _rb2D.velocity = new Vector2(_rb2D.velocity.x, 0);
                    _rb2D.AddForce(Vector2.up * JumpPower / 1.75f);
	            }
	        }
	    }

        if (CurHealth > MaxHealth)
        {
            CurHealth = MaxHealth;
        }

	    if (CurHealth <= 0)
	    {
	        CurHealth = 0;
	        Die();
	    }

	    if (!Grounded)
	    {
	        wallCheck = Physics2D.OverlapCircle(wallCheckPoint.position, 0.01f, WallLayerMask);

	        if (facingRight && Input.GetAxis("Horizontal") > 0.1f || !facingRight && Input.GetAxis("Horizontal") < -0.1f)
	        {
	            if (wallCheck)
	            {
	                HandleWallSliding();
	            }
	        }
	    }

	    if (wallCheck == false || Grounded)
	    {
	        WallSliding = false;
	    }
	}

    void HandleWallSliding()
    {
        _rb2D.velocity = new Vector2(_rb2D.velocity.x, -0.7f);

        WallSliding = true;

        if (Input.GetButtonDown("Jump"))
        {
            if (facingRight)
            {
                _rb2D.AddForce(new Vector2(-1.5f, 2) * JumpPower);
            }
            else
            {
                _rb2D.AddForce(new Vector2(1.5f, 2) * JumpPower);
            }
        }

    }

    void FixedUpdate()
    {
        Vector3 easeVelocity = _rb2D.velocity;
        easeVelocity.y = _rb2D.velocity.y;
        easeVelocity.z = 0.0f;
        easeVelocity.x *= 0.75f;

        float h = Input.GetAxis("Horizontal");

        // Подделка трения / ослобление скорости по оси Х нашего игрока
        if (Grounded)
        {
            _rb2D.velocity = easeVelocity;
        }

        // Двигаем игрока
        if (Grounded)
        {
            _rb2D.AddForce((Vector2.right * Speed) * h);
        }
        else
        {
            _rb2D.AddForce((Vector2.right * Speed / 2) * h);
        }

        // Предел скорости игрока
        if (_rb2D.velocity.x > MaxSpeed)
        {
            _rb2D.velocity = new Vector2(MaxSpeed, _rb2D.velocity.y);
        }

        if (_rb2D.velocity.x < -MaxSpeed)
        {
            _rb2D.velocity = new Vector2(-MaxSpeed, _rb2D.velocity.y);
        }
    }

    void Die()
    {
        if (PlayerPrefs.HasKey("HightScore"))
        {
            if (PlayerPrefs.GetInt("HightScore") < _gm.Score)
            {
                PlayerPrefs.SetInt("HightScore", _gm.Score);
            }
        }
        else
        {
            PlayerPrefs.SetInt("HightScore", _gm.Score);
        }

        Application.LoadLevel(Application.loadedLevel);
    }

    public void Damege(int dmg)
    {
        CurHealth -= dmg;
        gameObject.GetComponent<Animation>().Play("Player_RedFlash");
    }

    public IEnumerator Knockback(float knockDur, float knockBackPwr, Vector3 knockBackDir)
    {
        float timer = 0;

        while (knockDur > timer)
        {
            timer += Time.deltaTime;

            _rb2D.AddForce(new Vector3(knockBackDir.x * -100, knockBackDir.y * knockBackPwr, transform.position.z));
        }
        yield return 0;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Coin"))
        {
            Destroy(col.gameObject);
            _gm.Score += 1;
        }
    }
}
