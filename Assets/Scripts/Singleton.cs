using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[DefaultExecutionOrder(-100)]
public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T instance { get; private set; }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }


    }
}
