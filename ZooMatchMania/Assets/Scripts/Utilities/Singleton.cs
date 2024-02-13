using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 static instance similiar to singleton but instead of destroying any new instances, it overrides the current instances.
handy for resetting the state and saves you doing it manually
*/
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit() {
        Instance = null;
        Destroy(gameObject);
    }
}

/*
Transform the StaticInstance into a basic singleton. This will destroy any new versions created
leaving the original instance intact
*/
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        base.Awake();
    }
}
/// <summary>
/// survive through scene load. Perfect for system classes which require stateful, persistent data.
/// or audio sources where music plays through loading screens, etc
/// </summary>
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
