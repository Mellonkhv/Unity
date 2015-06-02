using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public int CurrentScore = 1;
    //public GUIText ScoreGUI;
    public GameObject TitleScreen;
    public Text Score;

    public void AddScore(int amountToAdd)
    {
        CurrentScore += amountToAdd;
        Score.text = CurrentScore.ToString();
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 30, 30), "X"))
        {
            GameObject g = Instantiate(TitleScreen) as GameObject;
            g.name = "Title";
            Destroy(gameObject);
        }
    }
}
