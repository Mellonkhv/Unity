using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour 
{

    public int offsetX = 2; // Сдвиг, недолжен вызывать ошибок

    // Используются для проверки необходимости расширить спрайт
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;
    
    public bool reverseScale = false;

    private float spriteWidth = 0f;
    private Camera cam;
    private Transform myTransform;


    void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }


	// Use this for initialization
	void Start () 
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(hasALeftBuddy == false || hasARightBuddy == false)
        {
            // Вычесление 
            float camHorisontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            //вычислить позицию Х 
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - camHorisontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth/2) + camHorisontalExtend;

            // Проверяем, если мы можем видеть край элемента тогда мы вызываем MakeNewBuddy() если это возможно
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false)
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }

	}

    // Функция создаёт тело если это требуется
    void MakeNewBuddy(int rightOrLeft)
    {
        // Вычесление новой позиции для нового тела
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        // Создание экземпляра нашего нового тело и хранения его в переменной
        Transform newBuddy = (Instantiate (myTransform, newPosition, myTransform.rotation)) as Transform;

        // Если не затайлено делаем переворот объекта по оси Х что бы избавиться от уродливых швов
        if (reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.lossyScale.x * -1, newBuddy.lossyScale.y, newBuddy.lossyScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if(rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}
