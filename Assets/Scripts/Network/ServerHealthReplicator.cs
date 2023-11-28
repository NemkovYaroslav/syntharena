using Unity.Netcode;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(NetworkObject))]
    public class ServerHealthReplicator : NetworkBehaviour
    {
        private readonly NetworkVariable<int> _replicatedHealth = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
        public NetworkVariable<int> ReplicatedHealth => _replicatedHealth; 
    
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            _replicatedHealth.Value = 100;
            
            if (!IsServer)
            {
                enabled = false;
                return;
            }
        }
    
        public int Health
        {
            get => _replicatedHealth.Value;
            set => _replicatedHealth.Value = value;
        }
    }
}
