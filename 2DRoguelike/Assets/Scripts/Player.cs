using UnityEngine;
using System.Collections;

public class Player : MovingObject {

    public int wallDamage = 1;              // Сколько повреждений наносится стене
    public int pointsPerFood = 10;          // Сколько очков еды получает от еды
    public int pointsPerSoda = 20;          // Сколько очков еды получает от соды
    public float restartLevelDelay = 1f;    // Задержка в секундах перед перезагрузкой уровня

    private Animator animator;              // Ссылка на компонент Аниматор для игрока
    private int food;                       // Количество 


	// Use this for initialization
	protected override void Start () 
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoins;

        base.Start();
	}

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoins = food;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (!GameManager.instance.playerTurn) return;
        
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;
        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);
	}

    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        food--;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        CheckIfGameOver();

        GameManager.instance.playerTurn = false;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LooseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
            GameManager.instance.GameOver();
    }
}
