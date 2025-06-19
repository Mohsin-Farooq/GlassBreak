using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour
{
    public void Restart()
    {
        UI_Manager.instance.GetActiveRestartImage();
        Invoke(nameof(LoadScene), 0.37f);
    }
    private void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        UI_Manager.instance.DestroyCanvas();
        
    }
}