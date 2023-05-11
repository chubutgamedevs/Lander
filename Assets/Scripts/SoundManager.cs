using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource AS;
    private float altura;
    [SerializeField] new AudioClip[] audio;
    [SerializeField] GameObject nave;
    public static SoundManager instance;  // Instancia única del SoundManager
    public AudioSource[] audioSources;    // Lista de fuentes de audio
    private AudioSource freeSource;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    void Start()
    {
        AS = GetComponent<AudioSource>();
        freeSource = GetFreeAudioSource();
    }
    void Update()
    {
         Altura();
    }

    public void Altura()
    {
        altura = nave.GetComponent<Nave>().altura;
        if ( Mathf.Abs( altura-15f)<0.7f)
        {
            PlaySound(audio[0]);
        }
        if ( Mathf.Abs( altura-10f)<0.7f)
        {
            PlaySound(audio[1]);   
        }
        if ( Mathf.Abs( altura-5f)<0.7f)
        {
            PlaySound(audio[2]);
        }
    }
    public IEnumerator SonidoAmbiente()
    {
        AS.clip = audio[4];
        yield return new WaitForSeconds(audio[5].length);
        StartCoroutine(SonidoAmbiente());
    }

    public void PlaySound(AudioClip clip)

    {
        // Busca una fuente de audio libre
        //freeSource = GetFreeAudioSource();

        if (freeSource == null)
        {
            Debug.LogWarning("No hay fuentes de audio libres disponibles.");
            return;
        }

        // Configura el clip y reproduce el sonido
        freeSource.clip = clip;
        if (!freeSource.isPlaying)
        {
            freeSource.Play();
        }

    }
    public void Pause()
    {
        freeSource.Pause();
    }

    private AudioSource GetFreeAudioSource()
    {
        // Busca una fuente de audio no ocupada
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (!audioSources[i].isPlaying)
                return audioSources[i];
            else
            {
                //return new AudioSource();
            }
        }
        // Si no hay fuentes de audio libres, muestra un mensaje de advertencia
        return null;
    }
}
