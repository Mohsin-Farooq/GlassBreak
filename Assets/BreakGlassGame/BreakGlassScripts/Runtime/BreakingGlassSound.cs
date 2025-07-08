using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Scriptable", menuName =" Leveldata")]
public class BreakingGlassSoundSO :ScriptableObject
{
    public ItemsData[] Bottles, Glasses, Shelfs;
}

[System.Serializable]
public class ItemsData
{

    public GameObject ballPrefab;
    public Vector3 ballPos;


}