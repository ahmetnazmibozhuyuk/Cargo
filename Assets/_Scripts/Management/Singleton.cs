using UnityEngine;

namespace Cargo.Managers
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T instance { get; private set; }
        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this as T;
        }
    }
    // //can be used for things that won't be destroyed on level change such as an audio manager
    //public abstract class PersistentSingleton<T> : MonoBehaviour where T : Component
    //{
    //    public static T instance { get; private set; }
    //    protected virtual void Awake()
    //    {
    //        if (instance != null)
    //        {
    //            Destroy(gameObject);
    //            return;
    //        }
    //        DontDestroyOnLoad(gameObject);
    //        instance = this as T;
    //    }
    //}
}