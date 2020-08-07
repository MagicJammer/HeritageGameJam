using UnityEngine;
//namespace Deus
//{
    public abstract class Singleton<C> : MonoBehaviour where C : MonoBehaviour
    {
        public static C Seele { private set; get; }
        public bool Perpetual;
        protected virtual void Awake()
        {
        if (Seele == null)
        {
            Seele = (C)(MonoBehaviour)this;
            if (Perpetual)
                DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        }
        protected virtual void OnDestroy()
        {
            Seele = null;
        }
    }
//}