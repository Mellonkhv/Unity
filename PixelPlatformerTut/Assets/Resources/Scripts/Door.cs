using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Door : MonoBehaviour {

    public int LevelToLoad;

    private GameMaster _gm;

    void Start()
    {
        _gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _gm.InpuText.text = ("[E] для Входа");
            if (Input.GetKeyDown("e"))
            {
                SaveScore();
                Application.LoadLevel(LevelToLoad);
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            if (Input.GetKeyDown("e"))
            {
                SaveScore();
                Application.LoadLevel(LevelToLoad);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {

            _gm.InpuText.text = ("");
        }
    }

    void SaveScore()
    {
        PlayerPrefs.SetInt("Score", _gm.Score);
    }
}
