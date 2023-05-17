using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour
{

    public GameObject planet;
    private Rigidbody rb;
    [Range(0, 5)]
    public float factor = 1;
    public ForceMode mode;
    public float g = 10;
    private Rigidbody luna;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        luna = planet.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 vector = planet.transform.position - transform.position;
        float FuerzaGravedad = (g * rb.mass * luna.mass)/(vector.magnitude * vector.magnitude);
        Physics.gravity = vector.normalized * FuerzaGravedad; 
        Debug.Log(Physics.gravity.magnitude);



    }
}
