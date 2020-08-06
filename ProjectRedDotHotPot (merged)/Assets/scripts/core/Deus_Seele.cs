using UnityEngine;
//namespace Deus
//{
    public abstract class Singleton<C> : MonoBehaviour where C : MonoBehaviour
    {
        public static C Seele { private set; get; }
        protected virtual void Awake()
        {
            Seele = (C)(MonoBehaviour)this;
        }
        protected virtual void OnDestroy()
        {
            Seele = null;
        }
    }
//}