using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    public int points;

    public Text PointsText;
    public Text InpuText;
	
	// Update is called once per frame
	void Update () 
    {
        PointsText.text = ("Points: " + points);
	}
}
