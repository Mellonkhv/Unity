using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject {

    public int wallDamage = 1;              // Сколько повреждений наносится стене
    public int pointsPerFood = 10;          // Сколько очков еды получает от еды
    public int pointsPerSoda = 20;          // Сколько очков еды получает от соды
    public float restartLevelDelay = 1f;    // Задержка в секундах перед перезагрузкой уровня
    public Text foodText;                   // Индикатор количества еды

    private Animator animator;              // Ссылка на компонент Аниматор для игрока
    private int food;                       // Количество 


    // Start перекрывает метод базового класса
	protected override void Start () 
    {
        animator = GetComponent<Animator>();

        // Получить текущую количество очков пища хранится в GameManager.instance между уровнями.
        food = GameManager.instance.playerFoodPoins;
        // Выводим информацию о еде
        foodText.text = "Еда: " + food;

        // Вызвать метод Start из базового класса
        base.Start();
	}

    // Эта функция вызывается, когда игрок станет заблокированным или неактивным.
    private void OnDisable()
    {
        // Сохраняем информацию о количестве еды
        GameManager.instance.playerFoodPoins = food;
    }
	
	// Update is called once per frame
	void Update () 
    {
        //  Если это не ход игрока, выйти из функции.
        if (!GameManager.instance.playerTurn) return;
        
        int horizontal = 0; // Использовать для горизонтального перемещения
        int vertical = 0;   // Использовать для вертикального перемещения

        // Получить информацию от менеджера ввода, вокруг него на целое и в магазине в горизонтальной установить по оси Х направлению движения
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        // Получить информацию от менеджера ввода, вокруг него на целое и в магазине в вертикальной установить по оси Y направлению движения
        vertical = (int)Input.GetAxisRaw("Vertical");

        // Если двигаемся горизонтально
        if (horizontal != 0)
            vertical = 0; // по вертикали запрещаем движение
        // Если Игрок движется
        if (horizontal != 0 || vertical != 0)
            // Вызов AttemptMove с общм параметром Wall, так это то с чем игрок может взаимодействовать, если они сталкиваются друг с другом (атакуя его).
            AttemptMove<Wall>(horizontal, vertical);
	}
    // Заменяем базовый класс AttemptMove
    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        // Обновляем информацию о еде
        foodText.text = "Еда: " + food;
        // Каждый ход отимаем еду
        food--;
        // Вызываем AttemptMove базового класса
        base.AttemptMove<T>(xDir, yDir);
        // Hit позволяет ссылаться на результат Linecast сделано в движении.
        RaycastHit2D hit;

        // Поскольку игрок перемещается и теряет очки еды, проверьте, жив ли он.
        CheckIfGameOver();

        // Ход игрока закончился
        GameManager.instance.playerTurn = false;
    }

    // OnTriggerEnter2D посылается, когда другой объект входит в пусковой коллайдер присоединены к этому объекту (только 2D физики).
    private void OnTriggerEnter2D (Collider2D other)
    {
        // Если столкнулся с объектом Выход
        if (other.tag == "Exit")
        {
            // Вызвать запуск следующего уровня с задержкой в 1 секунду
            Invoke("Restart", restartLevelDelay);
            // отключаем объект игрока уровень пройден
            enabled = false;
        }
        // Проверяем столкновение с едой
        else if (other.tag == "Food")
        {
            // Добавляем очки еды игроку
            food += pointsPerFood;
            // Обновляем информацию о еде
            foodText.text = "+" + pointsPerFood + " Еда: " + food;
            // Удаляем съеденый объект
            other.gameObject.SetActive(false);
        }
        // проверяем столкновение с Содовой
        else if (other.tag == "Soda")
        {
            // добавляем очки еды
            food += pointsPerSoda;
            // Обновляем информацию о еде
            foodText.text = "+" + pointsPerSoda + " Еда: " + food;
            // Удаляем съеденый объект 
            other.gameObject.SetActive(false);
        }
    }

    // Если игрок сталкнулся со стеной, но начинает её атаковать
    protected override void OnCantMove<T>(T component)
    {
        // Установить hitWall равным компоненту передающемуся в качестве параметра.
        Wall hitWall = component as Wall;
        // Наносим стене удар
        hitWall.DamageWall(wallDamage);
        // Проигрываем анимацию удара
        animator.SetTrigger("playerChop");
    }

    // Вызываем перезапуск уровня
    private void Restart()
    {
        // Загрузка последней сцены загружен, в этом случае Майне, только сцены в игре.
        Application.LoadLevel(Application.loadedLevel);
    }

    // Вызывается при нападении врага с параметром количество полученных повреждений
    public void LooseFood(int loss)
    {
        // Проигрываем анмацию получение повреждений
        animator.SetTrigger("playerHit");
        // отнимаем очки еды
        food -= loss;
        // Обновляем информацию о еде
        foodText.text = "-" + loss + " Еда: " + food;
        // проверяем живой ли игрок
        CheckIfGameOver();
    }

    // проверка жив ли ещё игрок
    private void CheckIfGameOver()
    {
        // если очков еди меньше или равно нулю
        if (food <= 0)
            // Объевляем конец игры
            GameManager.instance.GameOver();
    }
}
