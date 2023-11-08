using UnityEngine;

namespace BlueGravity.Tools
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject("Singleton");
                        instance = singletonObject.AddComponent<T>();
                    }
                }
                return instance;
            }
        }
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this as T;
            }
        }
    }
}
