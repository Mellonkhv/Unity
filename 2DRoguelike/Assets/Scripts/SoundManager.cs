using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioSource efxSource;               // Подключаемый в Инспекторе звук
    public AudioSource musicSource;             // Подключаемый в Инспекторе звук
    public static SoundManager instance = null; // Разрешает другим сценариям вызовать функций из Soundmanager.
    public float lowPitchRange = .95f;          // Низкие звуковые эффекты будут разбыты в случайном порядке
    public float highPitchRange = 1.05f;        // Высокие звуковые эффекты будут разбыты в случайном порядке
         

	// Use this for initialization
	void Awake () 
    {
        // Проверяем существует ли менеджер звуков если нет
        if (instance == null)
            // назначаем этот
            instance = this;
        // если да и он не этот
        else if (instance != this)
            // Уничтожаем его
            Destroy(gameObject);

        // Недаём уничтожить этот менеджер звуков если перезагружается уровень
        DontDestroyOnLoad(gameObject);
	}

    // Используется для проигрования зувыковых отрезков
    public void PlaySingle (AudioClip clip)
    {
        // Установить отрезок нашему источнику эффектов звук от звукового отрезка переданного в качестве параметра.
        efxSource.clip = clip;
        // проиграть этот отрезок
        efxSource.Play();
    }
	
    // RandomizeSfx выбирает случайным образом между различными звуковыми отрезками и слегка изменяет их тональность.
    public void RandomizeSfx (params AudioClip[] clips)
    {
        // Получаем случайный номер звуковоро отрезка
        int randomIndex = Random.Range(0, clips.Length);
        // Выбираем случайный тон, чтобы воспроизвести наш отрезок на уровне между нашими высоким и низким уровнями частот основного тона.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        
        // Устанавливаем тон источника звука на плученный уровень
        efxSource.pitch = randomPitch;
        // устанавливаем случайно выбранный звуковой отрезок 
        efxSource.clip = clips[randomIndex];
        // Воспроизводим полученый отрезок
        efxSource.Play();

    }

}
