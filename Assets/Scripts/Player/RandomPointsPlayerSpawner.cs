using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sample
{
    public class RandomPointsPlayerSpawner : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> m_SpawnPoints = new List<GameObject>();
        
        void Start()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += OnConnectionApprovalCallback;
        }
 
        void OnConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            response.Approved = true;
            response.CreatePlayerObject = true;
            response.Position = GetPlayerSpawnPosition();
        }
 
        Vector3 GetPlayerSpawnPosition()
        {
            Vector3 spawnPoint = m_SpawnPoints[Random.Range(0, m_SpawnPoints.Count)].transform.position;
            return spawnPoint;
        }
    }
}
