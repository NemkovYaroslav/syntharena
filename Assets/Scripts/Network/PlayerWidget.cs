using System;
using Support;
using TMPro;
using Unity.Netcode;

namespace Network
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

        public void SetOverlay()
        {
            var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            localPlayerOverlay.text = _playerName.Value;
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