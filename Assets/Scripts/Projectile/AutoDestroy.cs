using Unity.Netcode;
using UnityEngine;

namespace Projectile
{
    public class AutoDestroy : NetworkBehaviour
    {
        [SerializeField] private float lifetime = 5.0f;
    
        private void Start()
        {
            AutoDestroyServerRpc();
        }

        [ServerRpc]
        private void AutoDestroyServerRpc()
        {
            Destroy(gameObject, lifetime);
            //GetComponent<NetworkObject>().Despawn();
        }
    }
}