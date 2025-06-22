using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float verticalSensitivity = 0.005f;
    [SerializeField] private float maxYForce = 5f;
    private Rigidbody ballRigidbody;
    private Vector3 pendingVelocity;
    private void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody>();
    }
    public void ProcessMovementInput(Vector2 swipeVelocity, float swipeDuration)
    {
        float rawYForce = swipeVelocity.y * verticalSensitivity;
        float clampedY = Mathf.Clamp(rawYForce, -maxYForce, maxYForce);

        float x = swipeVelocity.x * verticalSensitivity;
        float z = swipeVelocity.y * moveSpeed * 0.01f;

        Vector3 direction = new Vector3(x, clampedY, z).normalized;
        float swipeForce = swipeVelocity.magnitude * moveSpeed * 0.01f;
        pendingVelocity = direction * swipeForce;
    }
    public void ApplyMovement()
    {
        ballRigidbody.isKinematic = false;
        ballRigidbody.velocity = pendingVelocity;
    }
    public void ResetMovement()
    {
        ballRigidbody.isKinematic = true;
    }
}