using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameObject gameManager;

	// Use this for initialization
	void Awake () 
    {
        // Проверка назначен ли GameManager на статическую переменную или нет
        if (GameManager.instance == null)
            Instantiate(gameManager); // Создание gameManager из префаба
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
