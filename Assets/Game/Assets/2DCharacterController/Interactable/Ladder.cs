using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public class Ladder : WorldActor, IInteractable
    {
        [SerializeField]
        private int _priority;

        [SerializeField]
        private Vector2 _minLocalPosition;

        [SerializeField]
        private Vector2 _maxLocalPosition;

        [ShowInInspector]
        public RelativeJoint2D _joint;

        public int Priority => _priority;

        [ShowInInspector]
        public Vector2 MinPosition => _minLocalPosition;

        [ShowInInspector]
        public Vector2 MaxPosition => _maxLocalPosition;

        public event System.Action<PlayerController, bool> OnCanInteractChange;

        public void CanInteract(PlayerController player, bool can)
        {
            // View can listen when a player can interact and trigger some feedback
            OnCanInteractChange?.Invoke(player, can);
        }

        public void Interact(PlayerController player)
        {
            player.SwitchClimb(this);
        }

        public void Attach(Rigidbody2D rigidbody)
        {
            _joint.connectedBody = rigidbody;
        }

        public void Detach()
        {
            _joint.connectedBody = null;
        }
    }
}