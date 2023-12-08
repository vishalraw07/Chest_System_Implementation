using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/* Service for our PopUp UI */

public class PopUpUIService : GenericMonoSingleton<PopUpUIService>
{
    [SerializeField] private GameObject _popUp;
    [SerializeField] private GameObject _backGroundUI;
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _unlockButton;
    [SerializeField] private Button _startTimerButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _unlockWithGemsButton;

    private TextMeshProUGUI _popUpText;
    private ChestController _currentChest;

    private void Start()
    {
        _popUpText = _popUp.GetComponentInChildren<TextMeshProUGUI>();
        _okButton = _popUp.GetComponentInChildren<Button>();
        _okButton.onClick.AddListener(ClearPopUp);
        _backButton.onClick.AddListener(ClearPopUp);
        _unlockButton.onClick.AddListener(UnlockChest);
        _unlockWithGemsButton.onClick.AddListener(UnlockChest);
        _startTimerButton.onClick.AddListener(StartUnlockTimer);
    }

    private void OnEnable() // For suubscribing to services
    {
        EventService.Instance.OnSlotsFull += OnSlotsFull;
        EventService.Instance.OnChestSpawn += ChestSpawned;
        EventService.Instance.OnChestClicked += ChestClicked;
        EventService.Instance.OnQueueFull += OnQueueFull;
    }
    private void StartUnlockTimer() // Start unlock timer
    {
        SoundService.Instance.PlayClip(SoundType.ButtonClick);
        ClearPopUp();
        ChestQueueService.Instance.EnQueueChest(_currentChest);
    }

    private void OnQueueFull() // On queue full
    {
        _popUpText.text = "Queue is full! Try again later..";
        ActivatePopUp();
        DisableAllButtons();
        _okButton.gameObject.SetActive(true);
    }

    public void UnlockChest() // For Unlocking chests
    {
        int gemCount = (int)_currentChest.GetChestModel().GEMS_TO_UNLOCK;
        if (ChestService.Instance._currentGems >= gemCount)
        {
            SoundService.Instance.PlayClip(SoundType.ChestOpen);
            EventService.Instance.InvokeOnUpdateCurrency(0, gemCount);            
            _currentChest.GetChestSM().ChangeState(ChestState.UNLOCKED);
            _currentChest.StartChestParticle();
            ClearPopUp();
        }
        else
        {
            ShowNotEnoughGems();
        }
    }

    private void ShowChestRewards(int coins, int gems) // For displaying rewards
    {
        _popUpText.text = "Chest opened!\n\nRewards:\n\nCoins: " + coins + "\nGems: " + gems;
        ActivatePopUp();
        DisableAllButtons();
        _okButton.gameObject.SetActive(true);
    }

    private void DisableAllButtons() // For disabling all buttons
    {
        _okButton.gameObject.SetActive(false);
        _backButton.gameObject.SetActive(false);
        _unlockButton.gameObject.SetActive(false);
        _startTimerButton.gameObject.SetActive(false);
        _unlockWithGemsButton.gameObject.SetActive(false);
    }
    private void ShowNotEnoughGems() // Show not enough gems window
    {
        _popUpText.text = "Not enough gems!";
        ActivatePopUp();

        DisableAllButtons();
        _okButton.gameObject.SetActive(true);
    }

    private void ChestClicked(ChestController chestController) // Chest clicked scenario
    {
        ChestState state = chestController.GetChestModel().ChestState;
        _currentChest = chestController;

        switch (state)
        {
            case ChestState.LOCKED: //Locked state click
                LockedChestPopUp(_currentChest);
                break;

            case ChestState.UNLOCKED: //Unlocked State Click
                SoundService.Instance.PlayClip(SoundType.ChestOpen);
                GetChestRewards();
                break;

            case ChestState.UNLOCKING: // Unlocking state click
                ShowUnlockWithGemsScreen();
                break;
        }
    }

    private void LockedChestPopUp(ChestController chestController) // For Locked chests
    {
        _popUpText.text = "Start timer or unlock with gems?";
        _unlockButton.GetComponentInChildren<TextMeshProUGUI>().text = "Unlock " + chestController.GetChestModel().MAX_GEMS_TO_UNLOCK + " <sprite name=\"gem\">";
        ActivatePopUp();

        // Modifying buttons
        DisableAllButtons();
        _unlockButton.gameObject.SetActive(true);
        _startTimerButton.gameObject.SetActive(true);
        _backButton.gameObject.SetActive(true);
    }
    private void GetChestRewards() // Update rewards for player
    {
        int randomCoins = Random.Range(_currentChest.GetChestModel().COINS_RANGE.x, _currentChest.GetChestModel().COINS_RANGE.y);
        int randomGems = Random.Range(_currentChest.GetChestModel().GEMS_RANGE.x, _currentChest.GetChestModel().GEMS_RANGE.y);
        ChestSlot slot = Array.Find(ChestSlotService.Instance.ChestSlots, i => i.ChestController == _currentChest);

        //Destroying chest object
        Destroy(_currentChest.GetChestView().gameObject);

        slot.SlotType = SlotType.EMPTY;
        slot.TimerText.text = "";
        ChestService.Instance.SetEmptyText(slot);

        ShowChestRewards(randomCoins, randomGems);
        EventService.Instance.InvokeOnUpdateCurrency(-randomCoins, -randomGems);
    }
    private void ShowUnlockWithGemsScreen() // For unlocking with gems
    {
        ActivatePopUp();
        _popUpText.text = "Unlock chest with gems?";
        DisableAllButtons();
        _backButton.gameObject.SetActive(true);
        _unlockWithGemsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Unlock " + _currentChest.GetChestModel().GEMS_TO_UNLOCK.ToString() + " <sprite name=\"gem\">";
        _unlockWithGemsButton.gameObject.SetActive(true);
    }
    private void ChestSpawned(ChestController chestController) // On chest spawned
    {
        // Setting background to non-interactable
        _backGroundUI.GetComponent<CanvasGroup>().blocksRaycasts = false;

        ChestType type = chestController.GetChestModel().ChestType;
        Vector2Int coinsRange = chestController.GetChestModel().COINS_RANGE;

        switch (type)
        {
            case ChestType.COMMON:
                _popUpText.text = "Common Chest Obtained!\nCoins range: " + coinsRange.x + " - " + coinsRange.y;
                break;
            case ChestType.RARE:
                _popUpText.text = "Rare Chest Obtained!\nCoins range: " + coinsRange.x + " - " + coinsRange.y;
                break;
            case ChestType.EPIC:
                _popUpText.text = "Epic Chest Obtained!\nCoins range: " + coinsRange.x + " - " + coinsRange.y;
                break;
            case ChestType.LEGENDARY:
                _popUpText.text = "Legendary Chest Obtained!\nCoins range: " + coinsRange.x + " - " + coinsRange.y;
                break;
        }

        // Activating pop-up
        ActivatePopUp();

        // Deactivating unused buttons
        DisableAllButtons();
        _okButton.gameObject.SetActive(true);
    }

    public void ActivatePopUp() // activate pop-up and deactivate background UI
    {
        _popUp.SetActive(true);
        _backGroundUI.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnSlotsFull() // On slots full
    {
        _popUpText.text = "Slots are full! Unlock some chest first.";
        ActivatePopUp();
    }
    public void ClearPopUp() // For clearing pop-up
    {
        SoundService.Instance.PlayClip(SoundType.ButtonClick);
        _backGroundUI.GetComponent<CanvasGroup>().blocksRaycasts = true;
        _popUp.SetActive(false);
    }
    private void OnDisable() // Un-subscribing events
    {
        EventService.Instance.OnSlotsFull -= OnSlotsFull;
        EventService.Instance.OnChestSpawn -= ChestSpawned;
        EventService.Instance.OnChestClicked -= ChestClicked;
        EventService.Instance.OnQueueFull -= OnQueueFull;
    }
}
