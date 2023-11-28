using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsComponent : MonoBehaviour
{
    public void OnPlayerDead(int playerId)
    {
        Analytics.CustomEvent("onPlayerDead", 
            new Dictionary<string, object>()
            {
                {"Player ID", playerId}
            }
        );
    }
    
    public void OnPlayerShot(int playerId)
    {
        Analytics.CustomEvent("onPlayerShotProjectile", 
            new Dictionary<string, object>()
            {
                {"Player ID", playerId}
            }
        );
    }
}