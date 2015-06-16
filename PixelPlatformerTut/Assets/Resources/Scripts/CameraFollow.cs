using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    private Vector2 _velocity;

    public float SmoothTimeX;
    public float SmoothTimeY;

    public GameObject Player;

    public bool Bounds;

    public Vector3 MinCamPos;
    public Vector3 MaxCamPos;

	// Use this for initialization
	void Start () 
    {
        Player = GameObject.FindGameObjectWithTag("Player");
	
	}

    void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, Player.transform.position.x, ref _velocity.x, SmoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, Player.transform.position.y, ref _velocity.y, SmoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);

        if (Bounds)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, MinCamPos.x, MaxCamPos.x),
                                             Mathf.Clamp(transform.position.y, MinCamPos.y, MaxCamPos.y),
                                             Mathf.Clamp(transform.position.z, MinCamPos.z, MaxCamPos.z));
        }
    }
	
}
