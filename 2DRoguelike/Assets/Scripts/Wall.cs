using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    public Sprite dmgSprite; // Альтернативный спрайт разрушающейся стены
    public int hp = 4;  // Количество жизней стены

    private SpriteRenderer spriteRenderer; // Сохраняем ссылку на компонент SPriteRenderer

	// Use this for initialization
	void Awake () 
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Получаем ссылку на компонент
	}
	
    // Вызываеся когда игрок атакует стену
    public void DamageWall(int loss)
    {
        // Меняем спрайт атакованной стены
        spriteRenderer.sprite = dmgSprite;
        hp -= loss; // Уменьшаем количество жизни у стены
        if (hp <= 0)
            gameObject.SetActive(false); // Если здоровь кончилось отключаем объект
    }
}
