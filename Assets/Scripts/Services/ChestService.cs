using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* Chest Service for Spawning of Chests */

[RequireComponent(typeof(Button))]
public class ChestService : GenericMonoSingleton<ChestService>
{
    // Current amount of currency available
    public int _currentCoins;
    public int _currentGems;

    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _gemText;

    [SerializeField] private Button spawnButton;
    [SerializeField] private ChestScriptableObjectList ChestScriptableObjectList;

    private void OnEnable()
    {
        EventService.Instance.OnUpdateCurrency += UpdateCurrency;
    }
    private void Start()
    {
        spawnButton.onClick.AddListener(SpawnChest);
        UpdateCurrency(0, 0);
    }
    private void UpdateCurrency(int _coins, int _gems)
    {
        _currentCoins -= _coins;
        _currentGems -= _gems;

        _coinText.text = ": " + _currentCoins.ToString();
        _gemText.text = ": " + _currentGems.ToString();
    }
    private void SpawnChest()
    {
        SoundService.Instance.PlayClip(SoundType.ButtonClick);
        for(int i = 0; i < ChestSlotService.Instance.ChestSlots.Length; i++)
        {
            ChestSlot slot = ChestSlotService.Instance.ChestSlots[i];
            if(slot.SlotType == SlotType.EMPTY)
            {
                ChestSlotService.Instance.SetSlotType(slot, SlotType.FILLED);
                GenerateRandomChest(slot);
                SetEmptyText(slot);
                break;
            }
            if(i == ChestSlotService.Instance.ChestSlots.Length - 1)
            {
                EventService.Instance.InvokeOnSlotsFull();
            }
        }
    }

    public void SetEmptyText(ChestSlot slot)
    {
        if(slot.SlotType == SlotType.EMPTY)
            slot.EmptyText.SetActive(true);
        else
            slot.EmptyText.SetActive(false);
    }

    private void GenerateRandomChest(ChestSlot _slot)
    {
        // Getting random SO from list
        int randomRange = (int)UnityEngine.Random.Range(0, ChestScriptableObjectList.ChestList.Length);
        ChestScriptableObject obj = ChestScriptableObjectList.ChestList[randomRange];

        // Setting all MVC components and Instantiating the view
        ChestModel model = new ChestModel(obj);
        ChestView view = GameObject.Instantiate<ChestView>(model.ChestView, _slot.Slot.transform);
        //view.transform.SetParent(_slot.Slot.transform);
        view.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;

        // Linking all components
        ChestController newChest = new ChestController(view, model);
        view.SetChestController(newChest);
        model.SetChestController(newChest);

        _slot.ChestController = newChest;
        newChest.SetChestSM(newChest);
        EventService.Instance.InvokeOnChestSpawn(newChest);
    }
    private void OnDisable()
    {
        EventService.Instance.OnUpdateCurrency -= UpdateCurrency;
    }
}
