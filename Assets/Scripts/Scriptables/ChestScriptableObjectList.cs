using UnityEngine;

/* List for Chest Scriptable Objects */

[CreateAssetMenu(fileName = "ChestScriptableObjectList", menuName = "ScriptableObjects/List/NewChestScriptableObjectList")]
public class ChestScriptableObjectList: ScriptableObject
{
    public ChestScriptableObject[] ChestList;
}
