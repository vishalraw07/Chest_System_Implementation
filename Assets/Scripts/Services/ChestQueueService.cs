using System.Collections.Generic;
using UnityEngine;

/* Chest queue service for queuing them on unlock request */

public class ChestQueueService : GenericMonoSingleton<ChestQueueService>
{
    [SerializeField] private int _maxQueueCount;
    private Queue<ChestController> _controllerQueue;
    private ChestController _currentChest = null;


    public ChestQueueService()
    {
        _controllerQueue = new Queue<ChestController>();
    }
    public int GetQueueCount()
    {
        return _controllerQueue.Count;
    }
    public void EnQueueChest(ChestController _chestController)
    {
        if (QueueHasSpace())
        {
            _chestController.GetChestSM().ChangeState(ChestState.QUEUED);
            _controllerQueue.Enqueue(_chestController);
            if (_currentChest == null)
                DeQueueChest();
        }
        else
        {
            EventService.Instance.InvokeOnQueueFull();
        }
    }
    public void DeQueueChest()
    {
        if(GetQueueCount() > 0)
        {
            ChestController chest = _controllerQueue.Dequeue();
            _currentChest = chest;
            _currentChest.GetChestSM().ChangeState(ChestState.UNLOCKING);
        }
        else
        {
            _currentChest = null;
        }
    }

    public bool QueueHasSpace()
    {
        return _controllerQueue.Count < _maxQueueCount;
    }
}
