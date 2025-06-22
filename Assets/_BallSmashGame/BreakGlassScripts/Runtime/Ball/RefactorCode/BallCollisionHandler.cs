using System;
using System.Collections;
using MoreMountains.NiceVibrations;
using UnityEngine;
public class BallCollisionHandler : MonoBehaviour
{ 
    [SerializeField] private string wallTag = "Wall";
   [SerializeField]  private InputManager inputHandler;
   [SerializeField]  private BallMovement ballMovement; 
   [SerializeField]  private BallVisualEffects visualEffects;
   [SerializeField]  private BallInputAction ballInputAction; 
    private bool hasHit;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
            HandleWallCollision();
        }

       
    }
    public void ResetCollisionState()
    {
        hasHit = false;
    }
    public void HandleWallCollision()
    {
        Invoke(nameof(ReturnBallToPool), 1f);
    }
    private void ReturnBallToPool()
    {
        if (!hasHit)
        {
            StartCoroutine(ReturnAfterEffect());
            hasHit = true;
        }
    }
    private IEnumerator ReturnAfterEffect()
    {
        yield return StartCoroutine(visualEffects.ScaleAndReturnToPool(0.3f)); 
        PoolManager.instance.ReturnBall(this.gameObject); 
        ResetBall(); 
    }
    private void ResetBall()
    {   visualEffects.ResetVisuals();
        ballMovement.ResetMovement();
        ResetCollisionState();
        ballInputAction.ResetInputState();
    }
}