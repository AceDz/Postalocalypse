using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    public class DeliveryItem : SerializedMonoBehaviour , IPickable
    {
        [SerializeField, BoxGroup("Pickable")] private IPickable _pickable;
        
        public event Action<int> OnAmountChanged;
        
        public bool CanPick()
        {
            return true;
        }
        public void Pick()
        {
            if (_pickable == null)
                return;

            if (_pickable.CanPick())
            {
                Debug.Log($"Picked {name}");
                OnAmountChanged?.Invoke(0);
                _pickable.Pick();
                Destroy(gameObject);
            }
        }
    }
}