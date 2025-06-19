using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

namespace BallThroughGame
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallBehaviour : MonoBehaviour
    {
        #region Variables

        [SerializeField] private float _BallRealeasedThereshold;
        [SerializeField] private Rigidbody _ball;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float maxYForce = 5f;
        [SerializeField] private float verticalSensitivity = 0.005f;
        private int ballLayer = 8, BlockLayer = 6;
        private Vector2 _StartTouch;
        private Vector2 _EndTouch;
        private Vector3 _PendingVelocity;
        private float _StartTime, _EndTime;
        private bool _isTouching, HasHit;
        private bool isTouchActive = false;
        Vector3 initialScale;
        #endregion

        private void Start()
        {    
            initialScale = transform.localScale;
            transform.localScale = initialScale;

        }

        private void Update()
        {
            // Check for touches on mobile or mouse input on desktop
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0); // Consider the first touch

                if (touch.phase == TouchPhase.Began && !isTouchActive)
                {
                    isTouchActive = true; // Mark that a touch is in progress
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 targetPosition = hit.point;
                        ProcessTouch(targetPosition);
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    isTouchActive = false; // Reset the touch flag
                }
            }
        
            else if (Input.GetMouseButtonDown(0)) // Left mouse click or tap
            {
               
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 targetPosition = hit.point;
                    ProcessTouch(targetPosition);
                }
            }
        }


        private void ProcessTouch(Vector3 targetPosition)
        {
            // Calculate the direction and velocity for the ball
            Vector3 direction = (targetPosition - transform.position).normalized;

            float distance = Vector3.Distance(transform.position, targetPosition);

            // Ensure strong movement in Z-direction
            direction.y = Mathf.Clamp(direction.y, -maxYForce, maxYForce);
            _PendingVelocity = direction * _moveSpeed * Mathf.Clamp(distance, 1f, 30f);

            if (_PendingVelocity.magnitude > _BallRealeasedThereshold)
            {
                _isTouching = true;
            }
        }

        private void FixedUpdate()
        {
            if (_isTouching)
            {
                _ball.isKinematic = false;
                _ball.velocity = _PendingVelocity;
                _isTouching = false;
                this.enabled = false;
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
                AudioManager._instance.PlaySound("throw");
                Invoke(nameof(SpawnDelay), 0.1f);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
                //  AudioManager._instance.PlaySound("WoodHit_0");
                Invoke(nameof(BallReturnedToPool), 1f);
            }
        }
        private void BallReturnedToPool()
        {
            if (!HasHit)
            {
                StartCoroutine(ScaleAndReturn());
                HasHit = true;
            }
        }

        private IEnumerator ScaleAndReturn()
        {
            float duration = 0.1f;
            float elapsed = 0f;

            initialScale = transform.localScale;
            Vector3 targetScale = Vector3.zero;

            while (elapsed < duration)
            {
                transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;

            PoolManager.instance.ReturnBall(this.gameObject);
            this.enabled = true;
            _ball.isKinematic = true;

        }


        private void OnEnable()
        {
            HasHit = false;

        }

        private void SpawnDelay()
        {
            
            BallSpawning.instance.Spawn();

        }

    }
}