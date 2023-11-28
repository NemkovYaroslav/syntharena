using Network;
using UnityEngine;
using UnityEngine.UI;

namespace Sample
{
    public class HealthUI : MonoBehaviour
    {
        private ServerHealthReplicator _healthTracker;
        
        private Slider _healthBar;

        private void Start()
        {
            _healthTracker = GetComponentInParent<ServerHealthReplicator>();
            _healthBar = GetComponent<Slider>();
            
            _healthTracker.ReplicatedHealth.OnValueChanged += OnHealthChanged;
            OnHealthChanged(0, _healthTracker.ReplicatedHealth.Value);
        }

        private void OnHealthChanged(int previousValue, int newValue)
        {
            SetHealthBarValue(newValue);
        }

        private void SetHealthBarValue(int healthBarValue)
        {
            _healthBar.value = healthBarValue;
        }

        private void OnDestroy()
        {
            _healthTracker.ReplicatedHealth.OnValueChanged -= OnHealthChanged;
        }
    }
}
