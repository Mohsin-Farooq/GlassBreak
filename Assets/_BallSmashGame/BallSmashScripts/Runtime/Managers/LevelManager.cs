using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Levels;
    [SerializeField] private BottleManager bottleManager;

    private int index = 0;

    public static LevelManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LoadLevel(index);
    }

    public void LoadLevel(int index)
    {
      
        index %= Levels.Count;

 
        foreach (GameObject level in Levels)
        {
            level.SetActive(false);
        }

 
        Levels[index].SetActive(true);

   
        bottleManager.RegisterBottlesInLevel(Levels[index]);

     ;
    }

    public void NextLevel()
    {
        index = (index + 1) % Levels.Count; 
        LoadLevel(index);
    }
}
