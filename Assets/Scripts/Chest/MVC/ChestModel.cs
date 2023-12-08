using UnityEngine;

/* Public Enumeration for ChestType */

public enum ChestType
{
    COMMON,
    RARE,
    EPIC,
    LEGENDARY
}

/* Enum for chest state */

public enum ChestState
{
    LOCKED,
    UNLOCKING,
    UNLOCKED,
    QUEUED
}

/* Chest Model for MVC */

public class ChestModel
{

    // Variables

    private ChestController _chestController;

    public Vector2Int COINS_RANGE {  get; }
    public Vector2Int GEMS_RANGE { get; }
    public float UNLOCK_TIME { get; set; }
    public float GEMS_TO_UNLOCK { get; set; }
    public float MAX_UNLOCK_TIME { get; }
    public float MAX_GEMS_TO_UNLOCK { get; }
    public ChestView ChestView { get; }
    public ChestType ChestType { get; }
    public ChestState ChestState { get; set; }

    // Public contructor

    public ChestModel(ChestScriptableObject chestScriptableObject)
    {
        COINS_RANGE = chestScriptableObject.Chest_Coins_Range;
        GEMS_RANGE = chestScriptableObject.Chest_Gems_Range;
        UNLOCK_TIME = chestScriptableObject.Unlock_Time;
        MAX_UNLOCK_TIME = chestScriptableObject.Max_Unlock_Time;
        MAX_GEMS_TO_UNLOCK = chestScriptableObject.Max_Gems_To_Unlock;
        ChestType = chestScriptableObject.Chest_Type;
        ChestView = chestScriptableObject.ChestView;
        ChestState = ChestState.LOCKED;
    }

    // Setters and Getters

    public ChestController GetChestController() 
    { 
        return _chestController; 
    }
    public void SetChestController(ChestController chestController)
    {
        _chestController = chestController;
    }
}
