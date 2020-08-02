using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { protected set; get; }

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = (T)(MonoBehaviour)this;
        else
            Destroy(gameObject);
    }
    protected virtual void OnDestroy()
    {
        Instance = null;
    }
}