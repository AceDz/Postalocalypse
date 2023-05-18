using Sirenix.OdinInspector;
using UnityEngine;

namespace _TeamTheDream._Delivery
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class GoToLevelSpawn : SerializedMonoBehaviour
    {
        [SerializeField, BoxGroup("Level")]
        private string _levelAssetReference;
        [SerializeField, BoxGroup("Level")]
        private int _spawnIndex;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ZonesManager.OnLoadZone?.Invoke(_levelAssetReference, _spawnIndex);
            }
        }
    }
}