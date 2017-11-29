using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//=============================================
//    PLAYER CONTROLLER
//=============================================
public class PlayerController : MonoBehaviour {
    // Публичные объекты
    [Header("Player Characteristic")]
    public float Acceleration;
    public float Steering;

    private Rigidbody2D _rb2d;

    // Метод пробуждения объекта
    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    // Обновление с участием физики
    private void FixedUpdate()
    {
        // Нажатие клавишь для движения по гооризонтали
        float horizontal = Input.GetAxis("Horizontal");

        // Вектор движения игрока и ускорение
        Vector2 speed = transform.up * Acceleration;
        _rb2d.AddForce(speed); // Назначаем скорость движения

        // Направление движения игрока
        float direction = Vector2.Dot(_rb2d.velocity, _rb2d.GetRelativeVector(Vector2.up));
        if (direction >= 0f)
        {
            _rb2d.rotation += horizontal * Steering * (_rb2d.velocity.magnitude / 5f); // Поворачиваем игрока направо 
        }
        else
        {
            _rb2d.rotation -= horizontal * Steering * (_rb2d.velocity.magnitude / 5f); // Поворачиваем игрока налево
        }

        Vector2 forward = new Vector2(0f, 0.5f);
        // Получить левый или правый угол 90 градусов на основе Rigidbody текущей скорости вращения
        float steeringRightAngle;
        if (_rb2d.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }
        // Найти Vector2, который равен 90 градусам относительно локального прямого направления (2D сверху вниз)
        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward; //Vector2.up;

        float driftForce = Vector2.Dot(_rb2d.velocity, _rb2d.GetRelativeVector(rightAngleFromForward.normalized));
        
        // Рассчёт противоположной силы дрейфу и применение его для генерации бокового сцепления (например, сцепления с шинами)
        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);

        _rb2d.AddForce(_rb2d.GetRelativeVector(relativeForce)); // Движение по кривой
    }
}
