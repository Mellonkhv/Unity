using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    public int Score;
    public int HightScore = 0;

    public Text PointsText;
    public Text InpuText;

    void Start()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            if (Application.loadedLevel == 0)
            {
                PlayerPrefs.DeleteKey("Score");
                Score = 0;
            }
            else
            {
                Score = PlayerPrefs.GetInt("Score");
            }
        }

        if (PlayerPrefs.HasKey("HightScore"))
        {
            HightScore = PlayerPrefs.GetInt("HightScore");
        }
    }
	
	void Update () 
    {
        PointsText.text = ("Points: " + Score);
	}
}
