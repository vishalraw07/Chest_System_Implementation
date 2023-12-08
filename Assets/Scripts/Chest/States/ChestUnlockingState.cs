using System;
using System.Collections;
using UnityEngine;

/* Chest Unlocking state */

public class ChestUnlockingState : ChestBaseState
{
    public ChestUnlockingState(ChestSM chestSM) : base(chestSM) { }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        chestSM.GetChestController().GetChestView().StartCoroutine(StartTimer());
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        chestSM.GetChestController().GetChestView().StopCoroutine(StartTimer());
    }
    private IEnumerator StartTimer()
    {
        ChestController chestController = chestSM.GetChestController();
        ChestModel chestModel = chestController.GetChestModel();
        ChestSlot slot = Array.Find(ChestSlotService.Instance.ChestSlots, i => i.ChestController == chestController);

        while(chestModel.UNLOCK_TIME > 0 && chestModel.ChestState == ChestState.UNLOCKING)
        {
            chestController.ChangeUnlockTimer(Time.deltaTime);
            slot.TimerText.text = GetTimerText(chestModel.UNLOCK_TIME);
            yield return new WaitForEndOfFrame();
        }
        chestSM.ChangeState(ChestState.UNLOCKED);
        ChestQueueService.Instance.DeQueueChest();
    }

    private string GetTimerText(float UNLOCK_TIME)
    {
        int hours = Mathf.FloorToInt(UNLOCK_TIME / 3600);
        int minutes = Mathf.FloorToInt((UNLOCK_TIME % 3600) / 60);
        int seconds = Mathf.FloorToInt(UNLOCK_TIME % 60);

        string finalString = "";

        finalString += hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");

        return finalString;
    }
}
