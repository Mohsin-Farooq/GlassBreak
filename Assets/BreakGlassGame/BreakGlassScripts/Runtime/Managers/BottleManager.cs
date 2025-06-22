using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this script is mainly responsible for register each bottle in level and to check if all the bottles smashed in the level.
/// </summary>

public class BottleManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private ButtonManager buttonMnager;
    private List<BreakGlass> bottles = new List<BreakGlass>();
    private int totalBottles;

    public void RegisterBottle(BreakGlass bottle)
    {
        bottles.Add(bottle);
        totalBottles++;
    }
    public void BottleSmashed(BreakGlass bottle)
    {
        bottles.Remove(bottle);

        if (bottles.Count == 0)
        {           
            StartCoroutine(WaitAndLoadNextLevel(0.8f));
            Invoke(nameof(LevelRestartCall), 0.5f);
        }
    }

    public void ResetManager()
    {
        bottles.Clear();
        totalBottles = 0;
    }

    private IEnumerator WaitAndLoadNextLevel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // Wait for the specified time
        levelManager.NextLevel();
    }

    private void LevelRestartCall()
    {
        buttonMnager.Restart(); 
    }

}
