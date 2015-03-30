using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {
	
    public int rotationOffcet = 90;
	
	// Update is called once per frame
	void Update () 
    {
		// Вычитание позиции игрока с позиции мыши
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();     // Нормализция вектора. Это означает, что все сумма вектора будет равна 1

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffcet);
	}
}
