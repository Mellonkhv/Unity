using UnityEngine;
using System.Collections;
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

    private bool searchingForPlayer = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        // Начать новый путь к позиции цели, вернуть результат в метод OnPathComplete
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator SearchForPlayer ()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {
            target = sResult.transform;
            searchingForPlayer = false;
            StartCoroutine(UpdatePath());
            return false;
        }
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return false;
        }

        // Начать новый путь к позиции цели, вернуть результат в метод OnPathComplete
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
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

    void FixedUpdate()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        // TODO: Всегда смотрю на игрока?

        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded) return;

            Debug.Log("Конец пути достигнут.");

            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        // Направление к следующей точке пути
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        // Двигаем AI
        rb.AddForce(dir, fMode);

        float distence = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distence < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }
}
