#region Access
using System;
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
namespace MMA.Updates
{
    public static partial class Key
    {
        public static string SubscribeUpdate = "Updates_SubscribeUpdate";
    }
    public static class TimeImport
    {
        public static int FrameCount => Time.frameCount;
        //public const string AddAsyncSceen = KeyData.BASE_SCENES_Request_AddAsyncScene;
    }
    public sealed partial class Updates_Module : Module
    {
        #region References
        private readonly Dictionary<int, Action> dic_updates = new Dictionary<int, Action>();
        private int frameCount = 0;
        #endregion
        #region Events
        private void Update()
        {
            frameCount = TimeImport.FrameCount;
            foreach (var item in dic_updates)
            {
                if (frameCount % item.Key == 0) item.Value.Invoke();
            }
        }
        #endregion
        #region Reactions ( On___ )
        // Contenedor de toda las reacciones del Updates
        protected override void OnSubscription(bool condition)
        {
            Middleware<(bool condition, int framecount, Action callback)>.Subscribe_Publish(condition, Key.SubscribeUpdate, SubscribeUpdate);
        }
        #endregion
        #region Methods
        // Contenedor de toda la logica del Updates
        private void SubscribeUpdate((bool condition,int framecount, Action callback) value)
        {
            if (value.condition)
            {

                if (!dic_updates.ContainsKey(value.framecount))
                {
                    dic_updates.Add(value.framecount, default);
                }

                dic_updates[value.framecount] += value.callback;

            }
            else
            {
                dic_updates[value.framecount] -= value.callback;

                if (dic_updates[value.framecount] == null)
                {
                    dic_updates.Remove(value.framecount);
                }

            }
        }
        #endregion
        #region Request ( Coroutines )
        // Contenedor de toda la Esperas de corutinas del Updates
        #endregion
        #region Task ( async )
        // Contenedor de toda la Esperas asincronas del Updates
        #endregion
    }
}