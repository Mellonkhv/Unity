using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds; //Массив для всех задников, передников паралакса
	private float[] parallaxScales; //Пропорции движения камер для движения фонов
	public float smoothing = 1f;	//Как сграживать параллакс 

	private Transform cam;			// Ссылка на основные параметры камеры
	private Vector3 previousCamPos;	// Позиция камеры в предыдущем кадре

	// Запускается перед функцией Start(). Отлично подходит для ссылок
	void Awake ()
	{
		// настройка ссылки на камеру
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () 
	{
		// позиция камеры в предыдущем кадре совападает с позицией камеры в текущем кадре
		previousCamPos = cam.position;

		// Назначение соответствующих parallaxScales
		parallaxScales = new float[backgrounds.Length];
		for (int i = 0; i < backgrounds.Length; i++)
		{
			parallaxScales[i] = backgrounds[i].position.z * -1;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < backgrounds.Length; i++)
		{
			// Параллакс противоположен движению камеры потому что предыдущий кадр умножается на масштаб
			float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

			// установка конечного положения текущей позиции + параллакс
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;

			// создаём целевую позицию которая является текущей позицией фона с его целевым положением Х
			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			// исчезать между текущей позиции и целевой позиции с использованием Lerp
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

		// установить previousCamPos в положение последнего кадра камеры
		previousCamPos = cam.position;
	}
}
