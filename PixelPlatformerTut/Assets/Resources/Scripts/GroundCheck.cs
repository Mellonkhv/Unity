using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour
{

    private Player _player;

    void Start()
    {
        _player = GetComponentInParent<Player>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        _player.Grounded = true;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        _player.Grounded = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        _player.Grounded = false;
    }
}

