using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource AS;
    private float altura;
    [SerializeField] new AudioClip[] audio;
    [SerializeField] GameObject nave;


    void Start()
    {
        AS = GetComponent<AudioSource>();
    }
    private void Update()
    {
        Altura();
    }

    void Altura(){
        altura = nave.GetComponent<Nave>().altura;

        if (altura == 15){
            AS.PlayOneShot(audio[0]);
        }
        if (altura == 10){
            AS.PlayOneShot(audio[1]);
        }
        if (altura == 5){
            AS.PlayOneShot(audio[2]);
        }
    }
    public IEnumerator Propulsor(){
        AS.clip = audio[4];
        yield return new WaitForSeconds(audio[4].length);
        StartCoroutine(Propulsor());
    }
    public IEnumerator SonidoAmbiente(){
        AS.clip = audio[5];
        yield return new WaitForSeconds(audio[5].length);
        StartCoroutine(SonidoAmbiente());
    }
}
