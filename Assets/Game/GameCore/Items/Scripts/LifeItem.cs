using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    [CreateAssetMenu(menuName = "_Delivery/Items/LifeItem")]
    public class LifeItem : SerializedScriptableObject , IPickable
    {
        [SerializeField, BoxGroup("Life")] private int _healAmount = 1;

        public event Action<int> OnAmountChanged;
        
        public bool CanPick()
        {
            return true;
        }

        [Button]
        public void Pick()
        {
            OnAmountChanged?.Invoke(1);
        }
        
        
    }
}