using Support;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerWidget : NetworkBehaviour
    {
        private readonly NetworkVariable<NetworkString> _playerName = new NetworkVariable<NetworkString>();

        private bool _overlaySet = false;
        
        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                return;
            }

            _playerName.Value = $"Player {OwnerClientId}";
        }

        private void SetOverlay()
        {
            var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            localPlayerOverlay.text = _playerName.Value;
            Debug.Log(_playerName.Value);
        }

        private void Update()
        {
            if (!_overlaySet && !string.IsNullOrEmpty(_playerName.Value))
            {
                SetOverlay();
                _overlaySet = true;
            }
        }
    }
}