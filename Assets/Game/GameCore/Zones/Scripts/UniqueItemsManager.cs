using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TeamTheDream.Delivery;
using UnityEngine;

namespace _TeamTheDream._Delivery
{
    public class UniqueItemsManager : MonoBehaviour
    {
        [ShowInInspector, HideInEditorMode]
        private List<string> _pickedUniqueItems;

        private void Awake()
        {
            _pickedUniqueItems = new List<string>();
        }

        private void OnEnable()
        {
            PickableJustOnce.OnUniqueItemPicked += PickableJustOnce_OnUniqueItemPicked;
        }

        private void OnDisable()
        {
            PickableJustOnce.OnUniqueItemPicked -= PickableJustOnce_OnUniqueItemPicked;
        }

        private void PickableJustOnce_OnUniqueItemPicked(string limitedItemId)
        {
            Debug.Log($"Picked limited item with id: {limitedItemId}");
            _pickedUniqueItems.Add(limitedItemId);
        }

        public void DeleteAlreadyPickedUniqueItems()
        {
            
            foreach (var limitedItemId in _pickedUniqueItems)
            {
                var go = GameObject.Find(limitedItemId);
                if(go != null)
                    Destroy(go);
            }
        }
    }
}