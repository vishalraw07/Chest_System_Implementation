using UnityEngine;

/* Chest Controller for MVC */

public class ChestController
{
    private ChestView _chestView;
    private ChestModel _chestModel;
    private ChestSM chestSM;

    public ChestController(ChestView chestView, ChestModel chestModel)
    {
        _chestView = chestView;
        _chestModel = chestModel;
    }

    public void SetChestSM(ChestController _chestController)
    {
        chestSM = new ChestSM(_chestController);
    }

    public ChestSM GetChestSM()
    {
        return chestSM;
    }

    public void ChangeUnlockTimer(float time)
    {
        _chestModel.UNLOCK_TIME = Mathf.Max(_chestModel.UNLOCK_TIME - time, 0);
        _chestModel.GEMS_TO_UNLOCK = Mathf.Ceil((_chestModel.UNLOCK_TIME / _chestModel.MAX_UNLOCK_TIME) * _chestModel.MAX_GEMS_TO_UNLOCK);
    }

    public void StartChestParticle()
    {
        _chestView.StartParticle();
    }

    public ChestView GetChestView()
    {
        return _chestView;

    }
    public ChestModel GetChestModel()
    {
        return _chestModel;
    }
}
