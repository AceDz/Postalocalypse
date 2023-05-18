using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public interface IInteractable
    {

        event System.Action<PlayerController, bool> OnCanInteractChange;
        int Priority { get; }
        void CanInteract(PlayerController player, bool can);
        void Interact(PlayerController player);
    }
}