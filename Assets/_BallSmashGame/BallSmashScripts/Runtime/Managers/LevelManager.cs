using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Levels;
    [SerializeField] private BottleManager bottleManager;
    private int index = 0;

    private void Awake()
    { 
        Physics.gravity = new Vector3(0, -1.8f, 0);
    }
    public void LoadLevel(int index)
    {
        if (index >= Levels.Count)
        {   
            index = 0;
        }
        PlayerPrefs.SetInt("Level", index);

        foreach (GameObject level in Levels)
        {
            level.SetActive(false);
        }

        Levels[PlayerPrefs.GetInt("Level")].SetActive(true);
        bottleManager.ResetManager();
    }

    public void NextLevel()
    {
        index++;
        LoadLevel(index);
    }

    private void Start()
    {    
        if (PlayerPrefs.HasKey("Level"))
        {
            index = PlayerPrefs.GetInt("Level");
        }
        else
        {
            index = 0;
            PlayerPrefs.SetInt("Level", index);
        }
        LoadLevel(index);
    }
}
