using UnityEngine;
using System.Collections;

using System.Collections.Generic;					//Allows us to use Lists.
using UnityEngine.UI;								//Allows us to use UI.

public class GameManager : MonoBehaviour 
{

    public static GameManager instance = null;      // Статический экземпляр GameManager, что позволят ему быть доступным любому другому классу

	public BoardManager boardScript;				// Сохраняем ссылку на класс Игровой доски которая создаст нам уровень


	// Use this for initialization
	void Awake () 
	{
        // Проверяем сужествует ли экземпляр
        if (instance == null)
            // Если нет назначаем в качестве экземпляра этот
            instance = this;
        // Если экземпляр существует и не является этим
        else if (instance != this)
            // уничтожить его, в живых остаться должен только один
            Destroy(gameObject);

        // Устанавливает этот, не уничтожает при перезагрузке сцену
        DontDestroyOnLoad(gameObject);

        // получаем ссылку на скрипт Игровой доски
		boardScript = GetComponent<BoardManager>();

        InitGame();
	}

    void InitGame()
    {

        // Генерируем уровень
        boardScript.SetupScene();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
