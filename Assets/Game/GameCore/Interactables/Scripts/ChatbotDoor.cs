using _TeamTheDream._Delivery;
using Sirenix.OdinInspector;
using TeamTheDream.Delivery;
using UnityEngine;

public class ChatbotDoor : MonoBehaviour , IDeliveryInteractable
{
    [SerializeField, BoxGroup("Canvas")] 
    private GameObject _itemIcon;
    
    [SerializeField, BoxGroup("Canvas")] 
    private GameObject _checkMark;
    
    [SerializeField, BoxGroup("Canvas")] 
    private GameObject _cross;
    
    [SerializeField, BoxGroup("Canvas")] 
    private SpecialKeyItem _specialKeyItem;

    [SerializeField, BoxGroup("GoToLevel")]
    private string _levelAssetReference;
    
    [SerializeField, BoxGroup("GoToLevel")]
    private int _spawnIndex;

    public void CanInteract(PlayerMovementController player, bool can)
    {
        _itemIcon.SetActive(can);
        
        _checkMark.SetActive(_specialKeyItem.PickedAmount >= _specialKeyItem.MaxAmount);
        _cross.SetActive(_specialKeyItem.PickedAmount < _specialKeyItem.MaxAmount);
    }

    public void Interact(PlayerMovementController player)
    {
        if(_specialKeyItem.PickedAmount >= _specialKeyItem.MaxAmount)
            ZonesManager.OnLoadZone?.Invoke(_levelAssetReference, _spawnIndex);
    }
}