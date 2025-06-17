using System.Collections;
using UnityEngine;
public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    [SerializeField] private GameObject RestartImage;
    private void Awake()
    {
        instance = this;
    }
    public void GetActiveRestartImage()
    {
        RestartImage.SetActive(true);
        DontDestroyOnLoad(this);
    }
    public void DestroyCanvas()
    { 
        StartCoroutine(DestroyCanvasAfterDelay(0.5f));
    }
    private IEnumerator DestroyCanvasAfterDelay(float delay)
    { 
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}