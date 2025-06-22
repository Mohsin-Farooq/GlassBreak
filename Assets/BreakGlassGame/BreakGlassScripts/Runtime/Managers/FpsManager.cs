using UnityEngine;
public class FpsManager : MonoBehaviour
{
#if UNITY_ANDROID
    private void Start()
    { 
        Application.targetFrameRate = 60;
    }
#endif
}