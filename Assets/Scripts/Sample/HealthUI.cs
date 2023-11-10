using UnityEngine;
using UnityEngine.UI;

namespace Sample
{
    public class HealthUI : MonoBehaviour
    {
        private ServerHealthReplicator m_HealthTracker;
        
        private Slider m_HealthBar;

        void Start()
        {
            m_HealthTracker = GetComponentInParent<ServerHealthReplicator>();
            m_HealthBar = GetComponent<Slider>();
            
            m_HealthTracker.m_ReplicatedHealth.OnValueChanged += OnHealthChanged;
            OnHealthChanged(0, m_HealthTracker.m_ReplicatedHealth.Value);
        }

        void OnHealthChanged(int previousValue, int newValue)
        {
            SetHealthBarValue(newValue);
        }
    
        void SetHealthBarValue(int healthBarValue)
        {
            m_HealthBar.value = healthBarValue;
        }
        
        void OnDestroy()
        {
            m_HealthTracker.m_ReplicatedHealth.OnValueChanged -= OnHealthChanged;
        }
    }
}
