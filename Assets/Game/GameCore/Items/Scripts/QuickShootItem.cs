﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    [CreateAssetMenu(menuName = "_Delivery/Items/QuickShootItem")]
    public class QuickShootItem : SerializedScriptableObject, IPickable, IMaxedItem
    {
        [SerializeField, BoxGroup("Quicker Shoot")] 
        private int _maxItemAmount = 2;

        [ShowInInspector, HideInEditorMode, BoxGroup("Picked")]
        private int _pickedAmount = 0;

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
    }
}