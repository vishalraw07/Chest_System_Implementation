using UnityEngine;

/* Chest base state */

public class ChestBaseState
{
    protected ChestSM chestSM;

    public ChestBaseState(ChestSM chestSM)
    {
        this.chestSM = chestSM;
    }

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
    public virtual void Tick() { }
}
