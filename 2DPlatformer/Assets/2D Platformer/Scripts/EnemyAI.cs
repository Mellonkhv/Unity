using UnityEngine;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour {

    // Цель преследования
    public Transform target;

    // Сколько раз в секунду мы будем обновлять наш путь
    public float updateRate = 2f;

    // Кэш
    private Seeker seeker;
    private Rigidbody2D rb;

    // Вычесление пути
    public Path path;

    // AI скорость в секунду
    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    // Максимальая дистанция для AI точка маршрута для перехода к следующей точки маршрута
    public float nextWaypointDistance = 3;

    // Точка пути к которой мы движемсяв настоящий момент
    private int currentWaypoint = 0;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        if (target == null)
        {
            Debug.LogError("ненайден игрок! Что!?");
            return;
        }

        // Начать новый путь к позиции цели, вернуть результат в метод OnPathComplete
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        // Написать больше
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("Мы получили путь. А есть ли ошибки? " + p.error);
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
