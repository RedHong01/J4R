using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    public virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError(string.Format("{0}已存在单例！", typeof(T).ToString()));

            Destroy(Instance);
        }
        Instance = this as T;
    }

    public virtual void OnDestroy()
    {
        if (Instance != null)
            Destroy(Instance);
    }
}