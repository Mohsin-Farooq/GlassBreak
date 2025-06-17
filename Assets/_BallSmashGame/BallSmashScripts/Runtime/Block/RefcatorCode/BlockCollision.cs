using System;
using UnityEngine;
public class BlockCollision : MonoBehaviour 
{
    public event Action OnCollsionWithWall;
    public event Action<Vector3,bool> OnCollsionWithBall;
    private bool IsHit;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Default") && !IsHit)
        {
            IsHit = true;
            OnCollsionWithWall?.Invoke();
        }
        if (other.gameObject.CompareTag("Ball"))
        {
            Vector3 ballPosition = other.transform.position;
            OnCollsionWithBall?.Invoke(ballPosition,IsHit);
        }
    }

   
}