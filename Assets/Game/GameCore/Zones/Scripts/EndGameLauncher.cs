using TeamTheDream.Delivery;
using UnityEngine;

namespace _TeamTheDream._Delivery
{
    public class EndGameLauncher : MonoBehaviour
    {
        [SerializeField] private NonPlayerCharacter _ai;

        public static System.Action OnEndGame;

        private void Start()
        {
            _ai.OnFirstConversationEnd += Ai_OnOnFirstConversationEnd;
        }
        
        private void OnDestroy()
        {
            _ai.OnFirstConversationEnd -= Ai_OnOnFirstConversationEnd;
        }

        private void Ai_OnOnFirstConversationEnd()
        {
            CallEndGame();
        }

        public void CallEndGame()
        {
            ZonesManager.OnEndGame();
            OnEndGame?.Invoke();
        }
    }
}