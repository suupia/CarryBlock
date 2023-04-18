using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace  Main.UI
{
    public class GameSceneUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _scoreText;
        


    }

    public interface IResourceAggregator
    {
        void AddResource(int amount);
    }

    public class ResourceAggregator : IResourceAggregator
    {
        int _amount;
        public int getAmount => _amount;
        
        public void AddResource(int amount)
        {
            _amount += amount;
        }
    }


}
