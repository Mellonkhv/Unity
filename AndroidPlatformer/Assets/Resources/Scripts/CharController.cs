using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour
{

    public Rigidbody2D rb2d;
    public float playerSpeed;
    public float jumpPower;
    public int directionInput;
    public bool groundCheck;
    public bool facingRight = true;

	// Use this for initialization
	void Start ()
	{
	    rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if ((directionInput < 0) && facingRight)
	        Flip();

	    if ((directionInput > 0) && !facingRight)
	        Flip();
	    groundCheck = true;
    }

    public void Move(int inputAxis)
    {
        directionInput = inputAxis;
        rb2d.velocity = new Vector2(playerSpeed * directionInput, rb2d.velocity.y);
    }

    public void Jump(bool isJump)
    {
        isJump = groundCheck;
        if (groundCheck)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
