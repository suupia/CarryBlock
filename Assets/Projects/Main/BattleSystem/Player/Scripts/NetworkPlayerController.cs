using Fusion;
using UnityEngine;
using System;
using Animations;
using VContainer;
using VContainer.Unity;

namespace Main
{
    /// <summary>
    /// The only NetworkBehaviour to control the character.
    /// Note: Objects to which this class is attached do not move themselves.
    /// Attachment on the inspector is done to the Info class.
    /// </summary>
    public class NetworkPlayerController : AbstractNetworkPlayerController
    {
        ReturnToMainBaseGauge _returnToMainBaseGauge;
        
        public override void Spawned()
        {
            base.Spawned();

            if (Object.HasInputAuthority)
            {
                // setup ReturnToMainBase
                _returnToMainBaseGauge = FindObjectOfType<LifetimeScope>().Container.Resolve<ReturnToMainBaseGauge>();
                _returnToMainBaseGauge.SetOnReturnToMainBase(RPC_SetToOrigin);
            }


        }

        void Update()
        {
            base.Update();
            
            if (Object.HasInputAuthority)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    _returnToMainBaseGauge.FillGauge();
                }

                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    _returnToMainBaseGauge.ResetGauge();
                }
                
                // ToDo : デバッグ用なので後で消す
                if (Input.GetKeyDown(KeyCode.H))
                {
                    Debug.Log($"hp = {PlayerStruct.Hp}");
                }

            }
        }
    }

}