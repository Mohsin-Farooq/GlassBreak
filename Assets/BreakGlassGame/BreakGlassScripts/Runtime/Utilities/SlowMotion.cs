using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlassBreakGame
{
    class SlowMotion : MonoBehaviour
    {
        public KeyCode resetKey = KeyCode.R;
        public KeyCode combination = KeyCode.LeftShift;
        [Range(0.01f, 20f)] public float timeScaleMultiplier = 1;
        [Range(0.01f, 20f)] public float timeScaleCurrent;

        float scale = 1;
        private void Update()
        {
            timeScaleCurrent = Time.timeScale;
            if (Input.GetKey(combination))
            {
                scale += Input.GetAxis("Mouse ScrollWheel") * timeScaleMultiplier;
                Time.timeScale = Mathf.Clamp(scale, 0.01f, 20f);
            }
            if (Input.GetKeyDown(resetKey))
            {
                Time.timeScale = 1f;
                scale = 1;
            }
        }
    }
}