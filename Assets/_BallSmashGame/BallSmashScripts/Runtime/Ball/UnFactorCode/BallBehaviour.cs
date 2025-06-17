using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

namespace BallThroughGame
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallBehaviour : MonoBehaviour,IBallIntereact
    {
        #region Variables

        [SerializeField] private float _BallRealeasedThereshold;
        [SerializeField] private Rigidbody _ball;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Vector3 _customGravity = new Vector3(0, -4f, 0);
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

            Physics.gravity = _customGravity;
            initialScale = transform.localScale;
            transform.localScale = initialScale;

        }

        private void Update()
        {
            // Check for touches on mobile or mouse input on desktop
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0); // Always consider the first touch only

                if (touch.phase == TouchPhase.Began && !isTouchActive)
                {
                    isTouchActive = true; // Mark that a touch is in progress
                    _StartTouch = touch.position;
                    _StartTime = Time.time;
                }

                if (touch.phase == TouchPhase.Ended && isTouchActive)
                {
                    _EndTouch = touch.position;
                    _EndTime = Time.time;

                    ProcessTouch();
                    isTouchActive = false; // Reset the touch flag
                }
            }
           
        }

        private void ProcessTouch()
        {
            Vector2 Delta = _EndTouch - _StartTouch;

            float swipeDuration = Mathf.Max(_EndTime - _StartTime, 0.01f);
            Vector2 swipeVelocity = Delta / swipeDuration;

            float rawYForce = swipeVelocity.y * verticalSensitivity;
            float clampedY = Mathf.Clamp(rawYForce, -maxYForce, maxYForce);

            float x = swipeVelocity.x * verticalSensitivity;
            float z = swipeVelocity.y * _moveSpeed * 0.01f;

            Vector3 direction = new Vector3(x, clampedY, z).normalized;

            float swipeForce = swipeVelocity.magnitude * _moveSpeed * 0.01f;
            _PendingVelocity = direction * swipeForce;

            if (swipeForce > _BallRealeasedThereshold)
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
                Invoke(nameof(SpawnDelay), 1f);
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
            float duration = 0.3f;
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


        public void BallInteract( )
        {
           
            
        }
    }
}