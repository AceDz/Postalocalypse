using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    [CreateAssetMenu(menuName = "_Delivery/Items/KeyItem")]
    public class KeyItem : SerializedScriptableObject , IPickable
    {
        [ShowInInspector, HideInEditorMode, BoxGroup("Picked")]
        private int _pickedAmount = 0;
        
        public event Action<int> OnAmountChanged;
        public int PickedAmount => _pickedAmount;

        public bool CanPick()
        {
            return true;
        }
        
        [Button]
        public void Pick()
        {
            _pickedAmount++;
            OnAmountChanged?.Invoke(_pickedAmount);
        }
    }
}