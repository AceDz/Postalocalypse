using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    [CreateAssetMenu(menuName = "_Delivery/Items/SpecialKeyItem")]
    public class SpecialKeyItem : SerializedScriptableObject, IPickable , IMaxedItem, IUnlockable
    {
        [SerializeField, BoxGroup("Extra Damage")] 
        private int _maxItemAmount = 1;

        [ShowInInspector, HideInEditorMode, BoxGroup("Picked")]
        private int _pickedAmount = 0;

        public int PickedAmount => _pickedAmount;
        public int MaxAmount => _maxItemAmount;
        public event Action<int> OnAmountChanged;

        public bool CanPick()
        {
            return _pickedAmount < _maxItemAmount;
        }
        
        [Button]
        public void Pick()
        {
            if (_pickedAmount >= _maxItemAmount) return;
            
            _pickedAmount++;
            OnAmountChanged?.Invoke(_pickedAmount);
        }

        [Button]
        public void ResetPickedAmount()
        {
            _pickedAmount = 0;
            OnAmountChanged?.Invoke(_pickedAmount);
        }

        public void Unlock()
        {
            Pick();
        }
    }
}