using RayFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGlassBreaker : MonoBehaviour
{
    public RayfireRigid glassRigid;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            glassRigid.Demolish(); 
 
        }
    }
}
