using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerFrustrationOnDeath : MonoBehaviour
{
    [SerializeField] private float deathFrustration = 10;
    [SerializeField] private AdventurerFrustrationTracker frustrationTracker;
    
    public void onKilled() {
        frustrationTracker.AddFrustrationFlat(deathFrustration);
    }
}
