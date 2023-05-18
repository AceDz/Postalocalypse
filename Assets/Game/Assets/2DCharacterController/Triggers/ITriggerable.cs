using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public interface ITriggerable
    {
        event System.Action<PlayerController> OnTrigger;
        void EnterTrigger(PlayerController player);
        void ExitTrigger();
    }
}