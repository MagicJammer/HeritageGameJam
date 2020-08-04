using System.Collections.Generic;
using UnityEngine;
//using Deus.Ex;
public delegate void DeusButtonDelegate();
public delegate void DeusEvent(object sender, params object[] args);
public delegate void DeusOnDestroy(float t = 0.0f);
public delegate void DeusAwake(object sender);
//namespace Deus
//{
//    namespace Ex
//    {
        public enum Order
        {
            KillDaJedi = 66,
            Ex0GE1 = 0,
            RegisterPlayer,
            RegisterEnemy,
            RegisterSolipsismObject,
            RegisterCameraSection,
        }
    //}
    public static partial class OrderCor
    {
        static Dictionary<int,DeusAwake> _awakeBag = new Dictionary<int,DeusAwake>();
        public static void Sub(int CastOrder, DeusAwake awake)
        {
            if (_awakeBag.TryGetValue(CastOrder, out DeusAwake existing))
                existing += awake;
            else
                existing = awake;
                _awakeBag[CastOrder] = existing;
        }
        public static void FirstOrder(object master)
        {
            int lim = _awakeBag.Count;
            for (int i = 0; i < lim; i++)
                _awakeBag[i]?.Invoke(master);
            _awakeBag.Clear();
        }
        static Dictionary<Order, DeusEvent> _eventBag = new Dictionary<Order, DeusEvent>();
        public static void Sub(Order ordername, DeusEvent ge)
        {
            if (_eventBag.TryGetValue(ordername, out DeusEvent existing))
                existing += ge;
            else
                existing = ge;
            _eventBag[ordername] = existing;
        }
        public static void UnSub(Order ordername, DeusEvent ge)
        {
            if (_eventBag.TryGetValue(ordername, out DeusEvent existing))
                existing -= ge;
            if (existing == null)
                _eventBag.Remove(ordername);
            else
                _eventBag[ordername] = existing;
        }
        public static void Execute(Order ordername, object sender, params object[] args)
        {
            if (_eventBag.TryGetValue(ordername, out DeusEvent existing))
                existing(sender, args);
        }
        static Dictionary<GameObject, DeusOnDestroy> _destroyBag = new Dictionary<GameObject, DeusOnDestroy>();
        public static void Sub(GameObject sender, DeusOnDestroy OnDestroy)
        {
            if (_destroyBag.TryGetValue(sender, out DeusOnDestroy existing))
                existing += OnDestroy;
            else
                existing = OnDestroy;
            _destroyBag[sender] = existing;
        }
        public static void DestroyInGame(this MonoBehaviour toDestroy, float t = 0.0f)
        {
            GameObject go = toDestroy.gameObject;
            if (_destroyBag.TryGetValue(go, out DeusOnDestroy existing))
            {
                existing(t);
                _destroyBag.Remove(go);
            }
            //ComponentCor<C>.Registry.Remove(go);
            Object.Destroy(go, t);
        }
        public static void FinalOrder()
        {
            _eventBag.Clear();
            _destroyBag.Clear();
            //ComponentCor<C>.FinalOrder();
        }
    }
//}