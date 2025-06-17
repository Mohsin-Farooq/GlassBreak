using UnityEngine;

[RequireComponent(typeof(BallMovement))]
[RequireComponent(typeof(BallCollisionHandler))]
[RequireComponent(typeof(BallVisualEffects))]
[RequireComponent(typeof(InputManager))]
public class BallManager : MonoBehaviour
{
   [SerializeField] private Vector3 customGravity = new Vector3(0, -4f, 0);

    private InputManager inputHandler;
    private BallInputAction inputAction;
    private BallCollisionHandler ballCollisionHandler; 
    private void Awake()
    {
        Physics.gravity = customGravity;

        inputHandler = GetComponent<InputManager>();
        inputAction = GetComponent<BallInputAction>();
        ballCollisionHandler = GetComponent<BallCollisionHandler>();
    }
    private void OnEnable()
    {
        inputHandler.OnTouchProcessed += inputAction.HandleTouchInput;
    }
    private void OnDisable()
    {
        inputHandler.OnTouchProcessed -= inputAction.HandleTouchInput;
    }
}