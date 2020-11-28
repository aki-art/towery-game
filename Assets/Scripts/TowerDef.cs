using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TowerDef", order = 1)]
[Serializable]
public class TowerDef : ScriptableObject
{
    public string PrefabID;
    public int height;
    public int circumference;
    public string displayName;
    public float cellHeight = 0.4f;
    public GameObject prefab;
}
