﻿using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public float fireRate = 0;
    public float Damage = 10;
    public LayerMask whatToHit;

    public Transform BulletTrailPrefab;
    public Transform MuzzleFlashPrefab;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;
    
    float timeToFire = 0;
    Transform firePoint;



	// Use this for initialization
	void Awake () 
    {
        firePoint = transform.FindChild ("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("Нет Точки огня? Совсем дурак?!");
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
	}

    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
        if (Time.time >= timeToSpawnEffect)
        {
            Effect();
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
       // Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);
        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Debug.Log("Мы попали " + hit.collider.name + " и нанесли " + Damage + " урона.");
        }
    }

    void Effect()
    {
        Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation);
        Transform clone  = Instantiate (MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint;
        float size = Random.Range (0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy (clone.gameObject, 0.02f);
    }
}
