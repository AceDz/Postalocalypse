using System;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using Sirenix.OdinInspector;
using TeamTheDream.Delivery;
using Unity.VisualScripting;
using UnityEngine;

namespace _TeamTheDream._Delivery
{
    public class Zone : SerializedMonoBehaviour
    {
        [SerializeField, BoxGroup("Initial Zone")]
        private List<Transform> _spawnPoints;
        
        [SerializeField, BoxGroup("Camera")]
        private ProCamera2D _proCamera2D;
        
        [SerializeField, BoxGroup("Camera")]
        private Vector2 _playerTargetOffset;

        public List<Transform> SpawnPoints => _spawnPoints;


        public void SetPlayer(PlayerMovementController player)
        {
            _proCamera2D.AddCameraTarget(player.transform, 0.65f, 0.65f, 0, _playerTargetOffset);
            _proCamera2D.AddCameraTarget(player.AimPoint, 0.35f, 0.35f);
        }
    }
}