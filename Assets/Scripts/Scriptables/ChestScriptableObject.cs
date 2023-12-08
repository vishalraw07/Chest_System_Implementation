using UnityEngine;
using UnityEngine.UI;

/* Scriptable Object for Chest */

[CreateAssetMenu(fileName = "ChestScriptableObject", menuName = "ScriptableObjects/NewChestScriptableObject")]
public class ChestScriptableObject: ScriptableObject
{
    public Vector2Int Chest_Coins_Range;
    public Vector2Int Chest_Gems_Range;
    public float Unlock_Time;
    public float Gems_To_Unlock;
    public float Max_Unlock_Time;
    public float Max_Gems_To_Unlock;
    public ChestView ChestView;
    public ChestType Chest_Type;
}
