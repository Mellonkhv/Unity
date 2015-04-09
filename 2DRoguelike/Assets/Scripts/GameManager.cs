using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;                  // Задержка перед запуском уровня
    public float turnDelay = .1f;                       // Задержка между каждым ходоом игрока
    public static GameManager instance = null;          // Статический экземпляр GameManager, что позволят ему быть доступным любому другому сценарию
    public BoardManager boardScript;                    // Сохраняем ссылку на класс Игровой доски которая создаст нам уровень
    public int playerFoodPoins = 100;                   // Стартовое колличество очков Еды(жизни) у игрока
    [HideInInspector] public bool playerTurn = true;    // Логическая проверка, если игрок ходит, скрытый от инспектора, но публичный


    private Text levelText;                             // Текст отображает номер уровня
    private GameObject levelImage;                     // Ширма для того чтобы спрятать пройденый и только что сгенерированный уговень и вывести информацию
    private int level = 1;                              // Номер текущего уровеня используется в игре как "день 1"
    private List<Enemy> enemies;                        // Список всех вражеских едениц, использующих команды движения
    private bool enemiesMoving;                         // Логическая проверка если враги в движении
    private bool doingSetup = true;                     // используется в том случаи, если строится уровень, для запрещения игроку перемещаться

	// Вызывается первой до функции Start
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
        // Назначаем врагов в новый лист вражесих объектов
        enemies = new List<Enemy>();
        // получаем ссылку на скрипт Игровой доски
        boardScript = GetComponent<BoardManager>();
        // Инициализируем первый уровень
        InitGame();
	}

    // Вызывается в то время когда уровень загружается
    private void OnLevelWasLoaded(int index)
    {
        // Увеличиваем счётчик уровня
        level++;
        // Инициализируем новый уровень
        InitGame();
    }

    // Инициализация игры для каждого уровня
    void InitGame()
    {
        // Запрещаем игроку двигаться
        doingSetup = true;
        // Получаем ссылку на LevelImage
        levelImage = GameObject.Find("LevelImage");
        // Получаем ссылку на LevelText
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        // Выводим информацию о дне выживания в стране Зомбидии
        levelText.text = "День " + level;
        // Скрыть игрока и уровень ширмой
        levelImage.SetActive(true);
        // Убираем ширму xthtp 2 ctreyls
        Invoke("HideLevelImage", levelStartDelay);

        // Очищаем список врагов
        enemies.Clear();
        // Генерируем уровень передавая его номер
        boardScript.SetupScene(level);
    }

    // Спрятать ширму используемую между уровнями
    void HideLevelImage()
    {
        // Скрываем ширму
        levelImage.SetActive(false);
        // разрешаем игроку двигаться
        doingSetup = false;
    }
	
    // Вызывается когда у игрока заканчиваются очки еды.
    public void GameOver()
    {
        // Говорим сколько выживал игрок
        levelText.text = "После " + level + " дней, ты окочурился.";
        // Активируем ширму
        levelImage.SetActive(true);
        // Выключает этот GameManager
        enabled = false;
    }

	// Update is called once per frame
	void Update () 
    {
        // Проверяем, что статус playersTurn или enemiesMoving или doingSetup в настоящее время не верны
        if (playerTurn || enemiesMoving || doingSetup)
            // Если где-то совпало повторить и не запускать MovingEnemy
            return;
        // Начало движения врагов
        StartCoroutine(MoveEnemies());
	}

    // Вызывается для добавления нового врага в список
    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script); // Добавление врага в список врагов
    }

    // Сопрограмма последовательного движения врагов
    IEnumerator MoveEnemies()
    {
        // Пока враги перемещаются игрок не может двигаться
        enemiesMoving = true;
        // Ожидание своей очереди в секундах (по умолчанию 100 мс)
        yield return new WaitForSeconds(turnDelay);
        // Если несоздани ниодного врага (это про первый уровень)
        if(enemies.Count == 0)
            // Задержка между очередью хода 
            yield return new WaitForSeconds(turnDelay);
        
        // перебираем список врагов
        for(int i = 0; i < enemies.Count; i++)
        {
            // Вызываем метод движения врагов для врага из списка
            enemies[i].MoveEnemy();
            // ждём поа враг движется перед тем как начать двигать следующего
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        // Все враги подвигались ьеперь и игрок может ходить
        playerTurn = true;
        // Враги двигаться не могут пока не подвигается игрок
        enemiesMoving = false;
    }
}
