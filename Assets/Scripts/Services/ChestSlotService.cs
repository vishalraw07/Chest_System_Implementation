using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SlotType
{
    EMPTY,
    FILLED
}

[Serializable]

/* Class for each chest slot */

public class ChestSlot
{
    public GameObject Slot;
    public GameObject EmptyText;
    public TextMeshProUGUI TimerText;
    public Button UnlockButton;
    public ChestController ChestController;
    public SlotType SlotType;
}

/* Chest slot service for taking care of Chest Slots */

public class ChestSlotService : GenericMonoSingleton<ChestSlotService>
{
    public ChestSlot[] ChestSlots;

    public void SetSlotType(ChestSlot chestSlot, SlotType slotType)
    {
        ChestSlot slot = Array.Find(ChestSlots, i => i.Slot == chestSlot.Slot);
        slot.SlotType = slotType;
    }
    public void SetTimerText(ChestSlot chestSlot, string _text)
    {
        chestSlot.TimerText.text = _text;
    }
    public void SetUnlockButtonText(ChestSlot chestSlot, string _buttonText)
    {
        ChestSlot slot = Array.Find(ChestSlots, i => i.Slot == chestSlot.Slot);
        slot.UnlockButton.GetComponentInChildren<TextMeshProUGUI>().text = _buttonText;
    }
}
