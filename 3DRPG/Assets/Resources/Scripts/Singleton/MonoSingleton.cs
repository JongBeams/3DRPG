using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            Debug.Log(instance);
            if (null == instance)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if (null == instance)
                {
                    GameObject proxyObj = new GameObject(typeof(T).Name);
                    instance = proxyObj.AddComponent<T>();
                    DontDestroyOnLoad(proxyObj);
                }
            }
            return instance;
        }

    }
}
