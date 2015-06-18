using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float MaxSpeed = 3f;
    public float Speed = 50f;
    public float JumpPower = 150f;

    public bool Grounded;
    public bool CanDoubleJump;

    public int CurHealth;
    public int MaxHealth = 5;

    private Rigidbody2D _rb2D;
    private Animator _anim;

	// Use this for initialization
	void Start ()
	{
	    _rb2D = gameObject.GetComponent<Rigidbody2D>();
	    _anim = gameObject.GetComponent<Animator>();

	    CurHealth = MaxHealth;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    _anim.SetBool("Grounded", Grounded);
        _anim.SetFloat("Speed", Mathf.Abs(_rb2D.velocity.x));

	    if (Input.GetAxis("Horizontal") < -0.01f)
	    {
	        transform.localScale = new Vector3(-1, 1, 1);
	    }
        
        else if (Input.GetAxis("Horizontal") > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

	    if (Input.GetButtonDown("Jump"))
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
	        Die();
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
        _rb2D.AddForce((Vector2.right * Speed) * h);

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
        Application.LoadLevel(Application.loadedLevel);
    }
}
