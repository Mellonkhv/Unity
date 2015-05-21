﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour 
{

    public List<GameObject> gems = new List<GameObject>();
    public int GridWidth;
    public int GridHeight;
    public GameObject GemPrefab;

	// Use this for initialization
	void Start () 
    {
	    for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                GameObject g = Instantiate(GemPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                g.transform.parent = gameObject.transform;
                gems.Add(g);
            }

        }
        gameObject.transform.position = new Vector3(-2.5f, -2f, 0);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

}