using System;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    public class PickableJustOnce : MonoBehaviour
    {
        [Header("THIS SHOULD BE SET IN SCENE FOR EACH INSTANCE")]
        [SerializeField] private string _limitedItemId;
        
        public static Action<string> OnUniqueItemPicked;
        
        private DeliveryItem _deliveryItem;

        private void Awake()
        {
            if (string.IsNullOrEmpty(_limitedItemId))
            {
                _limitedItemId = Guid.NewGuid().ToString();
                Debug.LogError($"There is a limited Item without id configured: {gameObject.name}");
            }
            gameObject.name = _limitedItemId;
            _deliveryItem = GetComponent<DeliveryItem>();
        }

        private void OnEnable()
        {
            _deliveryItem.OnAmountChanged += DeliveryItem_OnAmountChanged;
        }
        
        private void OnDisable()
        {
            _deliveryItem.OnAmountChanged -= DeliveryItem_OnAmountChanged;
        }

        private void DeliveryItem_OnAmountChanged(int amount)
        {
            OnUniqueItemPicked?.Invoke(_limitedItemId);
        }
    }
}