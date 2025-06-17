using MoreMountains.NiceVibrations;
using System;

using UnityEngine;

public class Block : MonoBehaviour 
{
    [SerializeField] private Rigidbody rb;
    public float explosionForce = 1f;
    public float explosionRadius = 1f; 
    public bool IsHit;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Default") && !IsHit)
        {
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
            
            AudioManager._instance.PlaySound("WoodHit_0");
            IsHit = true;

        }

        if (other.gameObject.CompareTag("Ball"))
        {
            AudioManager._instance.PlaySound("WoodHit_0");

            if (!IsHit)
            {
                rb.AddExplosionForce(explosionForce, other.transform.position, explosionRadius, 200f);

            }
            else
            {
                rb.AddExplosionForce(1, other.transform.position, 0.5f, 1f, ForceMode.Force);

            }

        }
    }

 
}

