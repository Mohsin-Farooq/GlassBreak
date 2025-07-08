using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlassBreakGame
{
    public class PresistenceCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject RestartImage;
        public static PresistenceCanvas instance;

        private void Awake()
        {
            instance = this;
        }

        public void DeactibateRestartImage()
        {
            RestartImage.SetActive(false);
        }

        public void GetActiveRestartImage()
        {
            RestartImage.SetActive(true);
            DontDestroyOnLoad(this);
        }

        public void DestroyCanvas()
        {
            StartCoroutine(DestroyCanvasAfterDelay(0.37f));
        }
        private IEnumerator DestroyCanvasAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}