using MoreMountains.NiceVibrations;
using UnityEngine;
public class BallInputAction : MonoBehaviour
{
    [SerializeField] private float ballReleaseThreshold = 1f;
    private BallMovement ballMovement;
    private bool isActive = true;
    private void Awake()
    {
        ballMovement = GetComponent<BallMovement>();
    }
    
    public void HandleTouchInput(Vector2 swipeVelocity, float duration)
    {
        if (!isActive) return;

        ballMovement.ProcessMovementInput(swipeVelocity, duration);

        float swipeForce = swipeVelocity.magnitude * duration;
        if (swipeForce > ballReleaseThreshold)
        {
            ballMovement.ApplyMovement();
            isActive = false;

            MMVibrationManager.Haptic(HapticTypes.LightImpact);
            AudioManager._instance.PlaySound("throw");

            Invoke(nameof(SpawnNewBall), 1f);
        }
    }
    private void SpawnNewBall()
    {
        BallSpawning.instance.Spawn();
    }

    public void ResetInputState()
    {
        isActive = true;
    }
}