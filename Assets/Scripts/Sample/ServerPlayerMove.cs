using Unity.Netcode;
using UnityEngine;

namespace Sample
{
    [DefaultExecutionOrder(0)]
    public class ServerPlayerMove : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
                return;
            }

            OnServerSpawnPlayer();
            
            base.OnNetworkSpawn();
        }
        
        void OnServerSpawnPlayer()
        {
            var spawnPoint = ServerPlayerSpawnPoints.Instance.ConsumeNextSpawnPoint();
            var spawnPosition = spawnPoint ? spawnPoint.transform.position : Vector3.zero;
            transform.position = spawnPosition;
        }
    }
}
