using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TeamTheDream.Delivery;
using UnityEngine;

public class PressAnyToStart : SerializedMonoBehaviour
{
    [SerializeField]
    private PlayerMovementController _player;
    
    [OdinSerialize]
    private NonPlayerCharacter _interactOnStart;

    private void Update()
    {
        if (Input.anyKey)
        {
            gameObject.SetActive(false);
            _interactOnStart.Interact(_player);
        }
    }
}
