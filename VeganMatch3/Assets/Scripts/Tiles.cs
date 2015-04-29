using UnityEngine;
using System.Collections;

public class Tiles : MonoBehaviour 
{

    //private bool _isFirstTile = true;         // Проверяет нажата ли какая либо плитка ранее

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	   
	}

    void OnMouseDown()
    {
        Debug.Log("Mouse is down");
        Debug.Log("Name = " + this.name);
        Debug.Log("Tag = " + this.tag);
        
        if(BoardManager.firstTile == null)
        {
            BoardManager._isFirstTile = false;
            BoardManager.firstTile = this;
            Debug.Log("Первая плитка");
        }
        else if (BoardManager.firstTile == this)
        {
            BoardManager.firstTile = null;
            Debug.Log("Таже плитка");
        }
        else if (BoardManager.firstTile != null && BoardManager.firstTile != this)
        {
            BoardManager._isFirstTile = true;
            BoardManager.firstTile = this;
            
            Debug.Log("Вторая плитка");
        }
    }
}
