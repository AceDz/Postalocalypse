using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    [CreateAssetMenu(menuName = "_Delivery/Player/PlayerBridge")]
    public class PlayerBridge : SerializedScriptableObject, ITalkable
    {
        private PlayerInteract _player;

        private PlayerInteract Player => _player == null ? FindObjectOfType<PlayerInteract>() : _player;
        
        public void BindPlayer(PlayerInteract player)
        {
            _player = player;
        }
        
        public void BeginTalk(string text, Action onComplete)
        {
            Player.BeginTalk(text, onComplete);
        }

        public void EndTalk(Action onComplete)
        {
            Player.EndTalk(onComplete);
        }

        public void ContinueTalk()
        {
            Player.ContinueTalk();
        }
        
        
    }
}