using System.Collections.Generic;
using Support;
using Unity.Netcode;
using UnityEngine;

namespace Network
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        private readonly NetworkVariable<int> _playersInGame = new NetworkVariable<int>();
    
        public int PlayersInGame => _playersInGame.Value;
    
        private void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
            {
                if (!IsServer)
                {
                    return;
                }
                
                Debug.unityLogger.Log($"{id} just connected...");
                _playersInGame.Value++;
            };
        
            NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
            {
                if (!IsServer)
                {
                    return;
                }
                
                Debug.unityLogger.Log($"{id} just disconnected...");
                _playersInGame.Value--;
            };
        }
    }
}
