using UnityEngine;
using System;
using System.Collections.Generic;  // Разрешает использовать слушателей (List)
using Random = UnityEngine.Random; // Говорим что "Random" будет использовать генератор случайных чисел от Unity

public class BoardManager : MonoBehaviour 
{
    [Serializable] // Данный атрибут позволяет увидеть класс с подсвойствами в интерфейсе Unity (Inspector)
    public class Count
    {
        public int minimum;                     // Минимальное значение для нашего класса подсчёта
        public int maximum;                     // Максимальное значение для нашего класса подсчёта

        // Задание конструктора
        public Count( int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int colums = 8;                      // Количество колонок на игровом поле
    public int rows = 8;                        // Количество рядов на игровом поле
    public Count wallCount = new Count(5, 9);   // Нижний и верхний придел случайного количества стен на уровне
    public Count foodCount = new Count(1, 5);   // Нижний и верхний предел случайного количества еды на уровне
    public GameObject exit;                     // Точка выхода с уровня
    public GameObject[] floorTiles;             // Массив с плитками пола
    public GameObject[] wallTiles;              
    public GameObject[] foodTiles;
    public GameObject[] enemiesTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder; // Переменная для хранения ссылок на расположение объекта "Игровое поле"
    private List<Vector3> gridPositions = new List<Vector3>(); // Перечень возможных местоположений для размещения плиток

    // Очистка списка gridPositions и подготовка его для нового игрового поля
    void InitialiseList()
    {
        gridPositions.Clear(); // Очистка списка
        
        for (int x = 1; x < colums - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f)); // В каждую ячейку сетки добавляем Vector3 с нашими координатами
            }
        }
    }

    // Устанавливаем на игровом поле внешние стены и пол
    void BoardSetup ()
    {
        boardHolder = new GameObject("Board").transform; // Создать поле и установить в boardHolder его координаты и ориентацию

        for (int x = -1; x < colums + 1; x++) // Цикл по оси Х начинается с -1 для заполнения края поля непроходимой стеной
        {
            for (int y = -1; y < rows + 1; y++) // Цикл по оси Y начинается с -1 для заполнения края поля непроходимой стеной
            {
                // Создаём объект для последующего заполнения и установки в сетку
                GameObject toInstantiate = new GameObject(); // = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == colums || y == -1 || y == rows) // координаты по краю поля
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)]; // заполняем объект непроходимой стеной
                else toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)]; // Иначе заполняем плиткой пола 

                // Создаём экземпляр игрового объекта с помощью типовых объектов выбранных ранее (toInstantiate) в текущей позиции в сетке и конвертирование его в Игровой объект
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                // Устанавливаем родителя пространственной ориентации нашего экземпляра объекта воизбежании загромождения иерархии
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    // Возвращает случайное число из списка gridPositions
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count); // Случайное целое число между 0 и количеством элементов в gridPositions
        Vector3 randomPosition = gridPositions[randomIndex]; // Случайная позиция типа Vector3 значения взяты из gridPosition на основании randomIndex
        gridPositions.RemoveAt(randomIndex); // Удаление записи на randomIndex так чтоб она более неиспользовалась
        return randomPosition; // возвращаем полученную случайную позицию
    }

    // Принимает массив игровых объектов и создаёт необходимое количество из диапазона минимум максимум
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1); // получаем количество создаваемых объектов

        for (int i = 0; i < objectCount; i++) // создаём выпавшее количество оъектов
        {
            Vector3 randomPosition = RandomPosition();  // получаем случайную позицию нового объекта
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)]; // Выбираем случайный вид объекта
            Instantiate(tileChoice, randomPosition, Quaternion.identity); // Создаём новый объект на поле
        }
    }

    // Инициализируем создание уровня и вызываем прежде описаные функции
	public void SetupScene (int level)
    {
        BoardSetup(); // Создаём стены и пол
        InitialiseList(); // Очищаем и заполняем координационную сетку поля
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum); // заполняем поле стенками которые ломаются
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum); // заполняем поле едой
        int enemyCount = (int)Mathf.Log(level, 2f); // случайное количество врагов
        LayoutObjectAtRandom(enemiesTiles, enemyCount, enemyCount); // ставим врагов на поле
        Instantiate(exit, new Vector3(colums - 1, rows - 1, 0f), Quaternion.identity); // устанавливаем точку выхода
    }
}
