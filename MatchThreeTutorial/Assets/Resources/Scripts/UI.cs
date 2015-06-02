using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour
{

    public GameObject game;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnGUI()
    {
        if(GUI.Button(new Rect(Screen.width/2 - 60,Screen.height/2,120,30), "New Game"))
        {
            GameObject g = Instantiate(game) as GameObject;
            g.name = "Game";
            Destroy(gameObject);
        }
    }
}
