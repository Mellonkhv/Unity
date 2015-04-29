using UnityEngine;
using System.Collections;
using System.Collections.Generic;  // Разрешает использовать слушателей (List)
using Random = UnityEngine.Random; // Говорим что "Random" будет использовать генератор случайных чисел от Unity

public class BoardManager : MonoBehaviour 
{

	//[Serializable] 								// Данный атрибут позволяет увидеть класс с подсвойствами в интерфейсе Unity (Inspector)
	// ==== PUBLIC VARIABLES ====
	public int colums = 8;						// Количество колонок на игровом поле
	public int rows = 8;						// Количество рядов на игровом поле
    public float shift = 0.01f;
    public GameObject[] tiles;                  // Массив с притками
    public static bool _isFirstTile = true;         // Проверяет нажата ли какая либо плитка ранее
    public static Tiles firstTile;

	// ==== PRIVAT VARIABLES ====
    private Transform boardHolder; 				// Переменная для хранения ссылок на расположение объекта "Игровое поле"
    private List<Vector3> gridPositions = new List<Vector3>(); // Перечень возможных местоположений для размещения плиток

    // Очистка списка позиций и подготовка нового
    void InialiseList()
    {
        gridPositions.Clear(); // Очищаем список
        
        for(int x = 0; x < colums; x++)
        {
            for(int y = 0; y < rows; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f)); // Вкаждую ячейку сетки добовляем Vector3 с нашими координатами
            }
        }
    }

	void BoardSetup()
	{
		boardHolder = new GameObject("Board").transform;

		for (int x = 0; x < colums; x++)
		{
			for (int y = 0; y < rows; y++)
			{
                // Создаём объект для последующего заполнения и установки в сетку
                GameObject toInstantiate = new GameObject();
                toInstantiate = tiles[Random.Range(0, tiles.Length)]; // Создаём случайную плитку

                // Создаю экземпляк игрового объекта спомощью типовых объектов (toInstantiate) в текущей позиции и конвентирую его в игровой объект
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                // Устанавливаю родителя пространственной ориентации этого экзерпляра
                instance.transform.SetParent(boardHolder);
			}
		}

	}

	public void SetupScene()
	{
        BoardSetup(); // Заполняем поле плитками
        //InialiseList(); // очищаем и заполняем координационную сетку

	}
}
