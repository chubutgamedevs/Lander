using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        DisableRagdoll();
    }
    public void EnableRagdoll()
    {
        rb.isKinematic = false;
        rb.detectCollisions = true;
    }
    public void DisableRagdoll()
    {
        rb.isKinematic = true;
        rb.detectCollisions = false;
    }
}
