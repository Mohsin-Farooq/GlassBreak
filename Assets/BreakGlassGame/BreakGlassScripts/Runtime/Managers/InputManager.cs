
using UnityEngine;
public class InputManager : MonoBehaviour
{   
    public delegate void TouchProcessed(Vector2 swipeVelocity, float duration);
    public event TouchProcessed OnTouchProcessed;
    private bool isTouchActive = false;
    private Vector2 startTouch;
    private float startTime;
    private void Update()
    { 
        if (Input.touchCount > 0)
        { 
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && !isTouchActive)
            { 
                isTouchActive = true;
                startTouch = touch.position;    
                startTime = Time.time;
            }

            if (touch.phase == TouchPhase.Ended && isTouchActive)
            {
                Vector2 endTouch = touch.position;
                float endTime = Time.time;

                Vector2 delta = endTouch - startTouch;
                float swipeDuration = Mathf.Max(endTime - startTime, 0.01f);
                Vector2 swipeVelocity = delta / swipeDuration;
                
                OnTouchProcessed?.Invoke(swipeVelocity, swipeDuration);
                isTouchActive = false;
            }
        }
    }
}
