using UnityEngine;
using System.Collections;

// abstract ключевое слово которое позволяет создавать классы и членов класса, которые являются неполными и должны быть реализованы в производном классе.
public abstract class MovingObject : MonoBehaviour {

    public float moveTime = 0.1f; // Сколько потребуется времени для движения в секундах
    public LayerMask blockingLayer; // Слой на котором будут проверяться столкновения

    private BoxCollider2D boxCollider; 
    private Rigidbody2D rb2D;
    private float inverseMoveTime; // Используется для более эффективного перемещения

	//Protected виртуальные функции могут быть переопределены с помощью наследования классов.
	protected virtual void Start () 
    {
        boxCollider = GetComponent<BoxCollider2D>(); // Получить ссылку на компонент
        rb2D = GetComponent<Rigidbody2D>(); // Получить ссылку на компонент
        // Сохраняя обратное время перемещения мы можем использовать его путем умножения вместо деления, это более эффективно.
        inverseMoveTime = 1f / moveTime;
	}

    // Move возвращает истину, если он может двигаться и ложно, если нет.
    // Move принимает параметры для направления X, направления Y и RaycastHit2D для проверки столкновения
    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        // Сохраняем стартовое положение для движения объекта от текужего положения
        Vector2 start = transform.position;
        // Рассчитать конечную позицию, основанную на параметрах направления, которые передаются при вызове "Move".
        Vector2 end = start + new Vector2(xDir, yDir); 

        // Отключить BoxCollider для того чтоб linecast не поймал собственный колайдер
        boxCollider.enabled = false;
        // Проводим линию от стартовой к конечной позиции для проверки столкновения на blockingLayer
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
            return;

        T hitComponent = hit.transform.GetComponent <T> ();

        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);
    }


    protected abstract void OnCantMove <T> (T component)
			where T : Component;
}
