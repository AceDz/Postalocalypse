using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    [CreateAssetMenu(menuName = "_Delivery/Items/SuperLifeItem")]
    public class SuperLifeItem : SerializedScriptableObject , IPickable
    {
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