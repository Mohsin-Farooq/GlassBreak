using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    private List<BreakGlass> bottles = new List<BreakGlass>();
    private int totalBottles;
    public void RegisterBottlesInLevel(GameObject activeLevel)
    {
     
        bottles.Clear();
        totalBottles = 0;
       
        BreakGlass[] levelBottles = activeLevel.GetComponentsInChildren<BreakGlass>(true);

        foreach (BreakGlass bottle in levelBottles)
        {
            if (bottle != null)
            {
                bottles.Add(bottle);
                totalBottles++;
                bottle.gameObject.SetActive(true); 
               
            }
        }
    }
    public void BottleSmashed(BreakGlass bottle)
    {
        bottles.Remove(bottle);

        if (bottles.Count == 0)
        {
            UI_Manager.instance.GetActiveRestartImage();

            Invoke(nameof(LoadNextLevel), 0.2f);
           
        }
    }

    private void LoadNextLevel()
    {
        levelManager.NextLevel();
        Invoke(nameof(DiabsleImage), 0.5f);
    }

    private void DiabsleImage()
    {
        UI_Manager.instance.DeactibateRestartImage();
    }

}
