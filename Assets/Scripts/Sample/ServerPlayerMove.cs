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
            if (!IsServer)
            {
                //enabled = false;
                return;
            }

            OnServerSpawnPlayer();
            
            //base.OnNetworkSpawn();
        }

        private void Start()
        {
            m_HealthTracker = GetComponent<ServerHealthReplicator>();
        }

        /*
        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.T))
            {
                m_HealthTracker.Health -= 10;
                if (m_HealthTracker.Health <= 0)
                {
                    m_HealthTracker.Health = 100;
                    GetComponent<CharacterController>().enabled = false;
                    GetComponent<CharacterController>().transform.position = ServerPlayerSpawnPoints.Instance.ConsumeNextSpawnPoint().transform.position;
                    GetComponent<CharacterController>().enabled = true;
                }
            }
        }
        */

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
