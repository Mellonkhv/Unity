using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float MaxSpeed = 3f;
    public float Speed = 50f;
    public float JumpPower = 150f;

    public bool Grounded;

    private Rigidbody2D _rb2D;
    private Animator _anim;

	// Use this for initialization
	void Start ()
	{
	    _rb2D = gameObject.GetComponent<Rigidbody2D>();
	    _anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    _anim.SetBool("Grounded", Grounded);
        _anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));

	    if (Input.GetAxis("Horizontal") < -0.01f)
	    {
	        transform.localScale = new Vector3(-1, 1, 1);
	    }
        
        else if (Input.GetAxis("Horizontal") > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

	    if (Input.GetButtonDown("Jump") && Grounded)
	    {
	        _rb2D.AddForce(Vector2.up * JumpPower);
	    }
	}

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

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
}
