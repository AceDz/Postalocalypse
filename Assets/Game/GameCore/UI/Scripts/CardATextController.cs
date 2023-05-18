using System;
using System.Collections;
using System.Collections.Generic;
using TeamTheDream.Delivery;
using TMPro;
using UnityEngine;

public class CardATextController : MonoBehaviour
{
    [SerializeField] private KeyItem _keyItem;
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
        _keyItem.OnAmountChanged += AmountTextChange;
        _text.text = _keyItem.PickedAmount.ToString();
    }

    private void AmountTextChange(int obj)
    {
        _text.text = _keyItem.PickedAmount.ToString();
    }

    private void OnDestroy()
    {
        _keyItem.OnAmountChanged -= AmountTextChange;
    }
}
