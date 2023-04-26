using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class gamemanager : MonoBehaviour
{
  [SerializeField] GameObject MensajeFinal;
  [SerializeField] GameObject ReiniciarButton;
  public string mensaje;
  public bool estado;
    public void Restart(){
        SceneManager.LoadSceneAsync("SampleScene");
    }
    public void final(){
        MensajeFinal.GetComponent<TMPro.TextMeshProUGUI>().text = mensaje;
        MensajeFinal.SetActive(true);
        ReiniciarButton.SetActive(true);
    }
    
}
