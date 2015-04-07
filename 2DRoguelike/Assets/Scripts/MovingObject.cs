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
        // Повторное включение BoxCollider после linecast
        boxCollider.enabled = true;

        // Проверяй если что-то ударил
        if (hit.transform == null)
        {
            // Если нет столкновения запустить SmoothMovement для перехода в позицию end 
            StartCoroutine(SmoothMovement(end));
            // Возвращаем истину
            return true;
        }
        // Если во что-то упёрлись возвращаем лож, движение не удалось
        return false;
    }

    // Метод для перемещения объекта от текущёй точки в конечную
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        // Вычислить оставшееся расстояние для перемещения на основе квадратный величины разности между текущей позицией и позицией end.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        // Пока расстояние больше чем почти 0
        while (sqrRemainingDistance > float.Epsilon)
        {
            //  Найти новую позицию пропорционально ближе к концу, на основе moveTime
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            // Вызов MovePosition в прилагаемом Rigidbody2D и перемещение его в вычесленную позицию
            rb2D.MovePosition(newPosition);
            // Пересчитываем оставшееся растояние после перемещения
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            // Возвращаемся в цкил пока sqrRemainingDistance не станет достаточно близко к нулю
            yield return null;
        }
    }

    // AttemptMove берёт сгенерированный параметр T, для указания типа компонента мы ожидаем от наших объектов, если он заблокирован (Игрок для врагов, стены для игрока).
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        // Hit будт хранить любые столкновения при вызове функции Move
        RaycastHit2D hit;
        // Устанавливает canMove в истину, если операция прошла успешно и ложно если иначе 
        bool canMove = Move(xDir, yDir, out hit);

        // Проверяем небыло ли столкновения
        if (hit.transform == null)
            return;

        // Получаем ссылку компонента с которым произошло столкновение
        T hitComponent = hit.transform.GetComponent <T> ();

        // Если canMove равен false и hitComponent не равен null значит объект неможет двигаться и столкнулся с чем-то с чем он может взаимодействовать
        if (!canMove && hitComponent != null)
            
            OnCantMove(hitComponent);
    }

    // Абстратный метод должен быть дописан при наследовании классов
    protected abstract void OnCantMove <T> (T component)
			where T : Component;
}
