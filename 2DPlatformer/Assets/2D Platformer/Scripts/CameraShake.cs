using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour 
{
    public Camera MainCam;

    float shakeAmount = 0;

    void Awake()
    {
        if (MainCam == null)
        {
            MainCam = Camera.main;
        }
    }
    
    public void Shake(float amt, float lenght)
    {
        shakeAmount = amt;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", lenght);
    }

    void DoShake()
    {
        if(shakeAmount > 0)
        {
            Vector3 camPos = MainCam.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += offsetX;
            camPos.y += offsetY;

            MainCam.transform.position = camPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        MainCam.transform.localPosition = Vector3.zero;
        
    }
}
