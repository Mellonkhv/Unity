using UnityEngine;
using System.Collections;

public class Enemy : MovingObject{

    public int playerDamege;    // Количество очков еды выпадающих из игрока при атаке

    private Animator animator;  // Переменная типа Аниматор сохраняет ссылку на компонент Аниматора
    private Transform target;   // координаты цели перемешения каждого хода
    private bool skipMove;      // определяет следует ли врагу пропустить ход или двигаться в этот ход

	// Start перекрывает метод базового класса
	protected override void Start () 
    {
        // Регистрация врага в GameMaster добавлением его в список
        // Это позволяет выдавать GameMaster команды перемещения
        GameManager.instance.AddEnemyToList(this);
        // подключаем компонент Аниматор
        animator = GetComponent<Animator>();
        // Найти игровой объект игрока используя тэг "Player" и сохранить ссылку на его координаты
        target = GameObject.FindGameObjectWithTag("Player").transform;
        // Вызвать метод Start из базового класса
        base.Start();
	}

    // Переопределяем базовый метод. Включаем возможность пропустить ход
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        // Если стоит пропустить ход
        if(skipMove)
        {
            // отменяем пропуск следующего хода
            skipMove = false;
            // и собственно говоря ход пропускаем
            return;
        }

        // Двигаем врага в заданном направлении
        base.AttemptMove<T>(xDir, yDir);
        // пропустить следующий ход
        skipMove = true;
    }

    // вызывается из GameMaster каждый ход для каждого врага чтобы пытаться двигаться в направлении игрока
    public void MoveEnemy()
    {
        // Объявляем переменные направления для осей X и Y они варьируются от -1 до +1
        // Эти значения позволяют выбирать направление: вверх, вниз, вправо, влево
        int xDir = 0;
        int yDir = 0;

        // Если разница в положениях по оси X приблизительно равна нулю (эпсилон) делаем следующее:
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            // Если координаты по оси Y игрока (цели) больше чем у врага то yDir = +1(вверх) иначе -1(вниз)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        // Если разница в позициях по Х не близка у нулю (Эпсилон) делаем следующее:
        else
            // Если координаты по оси X игрока больше чем у врага то xDir = +1(вправо) иначе -1(влево)
            xDir = target.position.x > transform.position.x ? 1 : -1;

        // вызываем функцию движения и передаём общий параметры игрока потому что враг движется и ожидает столкновения с игроком
        AttemptMove<Player>(xDir, yDir);
    }

    // Вызывается если враг пытается двигаться в  пространстве зянятым игроком, заменяет метод базового класса
    protected override void OnCantMove<T>(T component)
    {
        // Объявляем hitPlyer и объявляем полученный компонент классом игрока
        Player hitPlayer = component as Player;

        // Переключаем анимацию в атаку
        animator.SetTrigger("enemyAttack");

        // отнимаем у игрока очки еды (здоровье)
        hitPlayer.LooseFood(playerDamege);
    }
}
