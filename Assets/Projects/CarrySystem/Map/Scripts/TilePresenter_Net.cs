using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Entity.Scripts;


namespace Carry.CarrySystem.Map.Scripts
{
    public class TilePresenter_Net : NetworkBehaviour, ITilePresenter
    {
        public struct PresentData : INetworkStruct
        {
            public NetworkBool IsGroundActive;
            public NetworkBool IsRockActive;
            public NetworkBool IsDoubleRockActive;
        }

        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();
        
        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい
        [SerializeField] GameObject groundObject;
        [SerializeField] GameObject rockObject;
        [SerializeField] GameObject doubleRockObject;
        
        EntityGridMap _map;
        public override void Render()
        {
            if (groundObject.activeSelf != PresentDataRef.IsGroundActive)
            {
                groundObject.SetActive(PresentDataRef.IsGroundActive);
            }
            if (rockObject.activeSelf != PresentDataRef.IsRockActive)
            {
                rockObject.SetActive(PresentDataRef.IsRockActive);
            }
            if (doubleRockObject.activeSelf != PresentDataRef.IsDoubleRockActive)
            {
                doubleRockObject.SetActive(PresentDataRef.IsDoubleRockActive);
            }
        }
        
        public void SetInitEntityActiveData(IEntity entity, bool isActive)
        {
            switch (entity)
            {
                case Ground _:
                    PresentDataRef.IsGroundActive = isActive;
                    break;
                case Rock _:
                    PresentDataRef.IsRockActive = isActive;
                    break;
                case null :
                    PresentDataRef.IsRockActive = false;
                    PresentDataRef.IsDoubleRockActive = false;
                    break;
                default:
                    throw new System.Exception($"想定外のEntityが渡されました entity : {entity}");
            }
        }
        public void SetEntityActiveData(IEntity entity,int count)
        {
            switch (entity)
            {
                case Ground _:
                    PresentDataRef.IsGroundActive = true;
                    break;
                case Rock _:
                    switch (count)
                    {
                        case 0:
                            PresentDataRef.IsRockActive = false;
                            PresentDataRef.IsDoubleRockActive = false;
                            break;
                        case 1:
                            PresentDataRef.IsRockActive = true;
                            PresentDataRef.IsDoubleRockActive = false;
                            break;
                        case 2:
                            PresentDataRef.IsRockActive = false;
                            PresentDataRef.IsDoubleRockActive = true;
                            break;
                        default:
                            throw new System.Exception("想定外のcountが渡されました");
                    }
                    break;
                default:
                    throw new System.Exception("想定外のEntityが渡されました");
            }
        }

    }

}
