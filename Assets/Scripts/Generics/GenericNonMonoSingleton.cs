using UnityEngine;

/* Non monobehaviour singleton generic */

public class GenericNonMonoSingleton<T> where T : GenericNonMonoSingleton<T>
{
    private static T instance;
    public static T Instance { get { return instance; } }

    public GenericNonMonoSingleton()
    {
        if(instance == null)
        {
            instance = (T)this;
        }
        else
        {
            Debug.Log("Trying to create another instance of " + instance);
        }
    }
}
