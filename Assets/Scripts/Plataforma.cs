using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma : MonoBehaviour
{
    [SerializeField] int minX;
    [SerializeField] int maxY;
   void Start()
    {
      gameObject.transform.position = new Vector3(Random.Range(minX,maxY),-20,0);   
    }
}


