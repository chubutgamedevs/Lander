using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class gamemanager : MonoBehaviour
{
  [SerializeField] GameObject MensajeFinal;
  [SerializeField] GameObject ReiniciarButton;
  [SerializeField] GameObject SoundManager;
  public string mensaje;
  public bool estado;
    public void final(){
        MensajeFinal.GetComponent<TMPro.TextMeshProUGUI>().text = mensaje;
        MensajeFinal.SetActive(true);
        ReiniciarButton.SetActive(true);
    }
    public void GameLoop(){
        SceneManager.LoadSceneAsync("SampleScene");
    }
    
}
