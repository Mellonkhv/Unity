using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float m_speed = 0.1f;
    private Camera mycam;

	// Use this for initialization
	void Start ()
	{
	    mycam = GetComponent<Camera>();

	}
	
	// Update is called once per frame
	void Update ()
	{
	    mycam.orthographicSize = (Screen.height / 100f) / 4f;

	    if (Target)
	    {
	        transform.position = Vector3.Lerp(transform.position, Target.position, m_speed) + new Vector3(0, 0, -10);
	    }
	}
}
