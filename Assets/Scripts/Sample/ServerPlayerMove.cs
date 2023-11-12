using System;
using Unity.Netcode;
using UnityEngine;

namespace Sample
{
    [DefaultExecutionOrder(0)]
    public class ServerPlayerMove : NetworkBehaviour
    {
        private ServerHealthReplicator m_HealthTracker;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            if (!IsServer)
            {
                //enabled = false;
                return;
            }

            OnServerSpawnPlayer();
        }

        private void Start()
        {
            m_HealthTracker = GetComponent<ServerHealthReplicator>();
        }

        public void OnServerRespawnPlayer()
        {
            m_HealthTracker.Health = 100;
            GetComponent<CharacterController>().enabled = false;
            GetComponent<CharacterController>().transform.position = ServerPlayerSpawnPoints.Instance.ConsumeNextSpawnPoint().transform.position;
            GetComponent<CharacterController>().enabled = true;
        }
        
        void OnServerSpawnPlayer()
        {
            var spawnPoint = ServerPlayerSpawnPoints.Instance.ConsumeNextSpawnPoint();
            var spawnPosition = spawnPoint ? spawnPoint.transform.position : Vector3.zero;
            transform.position = spawnPosition;
        }
    }
}
