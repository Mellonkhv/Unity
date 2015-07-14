using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{

    private bool _attaking = false;

    private float _attackTimer = 0;
    private float _attackCd = 0.3f;

    public Collider2D AttackTrigger;

    private Animator _anim;

    void Awake()
    {
        _anim = gameObject.GetComponent<Animator>();
        AttackTrigger.enabled = false;
    }
    
    // Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown("f") && !_attaking)
	    {
	        _attaking = true;
	        _attackTimer = _attackCd;

	        AttackTrigger.enabled = true;
	    }

	    if (_attaking)
	    {
	        if (_attackTimer > 0)
	        {
	            _attackTimer -= Time.deltaTime;
	        }
	        else
	        {
	            _attaking = false;
	            AttackTrigger.enabled = false;
	        }
	    }

        _anim.SetBool("Attacking", _attaking);
	}
}
