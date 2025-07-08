using UnityEngine;
using UnityEngine.SceneManagement;

namespace GlassBreakGame
{
    public class ButtonManager : MonoBehaviour
    {
        public void Restart()
        {
            PresistenceCanvas.instance.GetActiveRestartImage();
            Invoke(nameof(LoadScene), 0.6f);
        }
        private void LoadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            PresistenceCanvas.instance.DestroyCanvas();

        }
    }
}