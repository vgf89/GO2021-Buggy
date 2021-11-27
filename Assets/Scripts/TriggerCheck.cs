// Use this to check whether your trigger collider is colliding with anything else.
// Use Physics layers and/or Tags to influence what colliders trigger this script.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCheck : MonoBehaviour
{
    [ReadOnly][SerializeField]private int m_isColliding = 0;
    [SerializeField] public UnityEvent m_Triggered;
    [SerializeField] public UnityEvent m_Untriggered;
    enum tagFilterModeEnum{disabled, whitelist, blacklist};
    [SerializeField] private tagFilterModeEnum tagFilterMode = tagFilterModeEnum.disabled;
    [SerializeField] private List<string> tagFilter;

    List<Collider2D> colliders = new List<Collider2D>();
    private float distanceAbs = float.PositiveInfinity;

    private Collider2D selfCollider;

    [SerializeField] private bool debug = false;


    private bool disableChecksFlag = false;
    

    void Awake() {
        selfCollider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!tagFilterCheck(other.tag)) {
            return;
        }
        
        m_isColliding++;
        if (m_isColliding == 1) {
            m_Triggered.Invoke();
        }
        colliders.Add(other);
        if (debug) Debug.Log(gameObject.name + ": " + m_isColliding + "   " + colliders.Count);
    }
    void OnTriggerExit2D(Collider2D other) {
        if (!tagFilterCheck(other.tag)) {
            return;
        }

        m_isColliding--;
        if (m_isColliding == 0) {
            m_Untriggered.Invoke();
        }
        colliders.Remove(other);
        if (debug) Debug.Log(gameObject.name + ": " + m_isColliding + "   " + colliders.Count);
    }

    private bool tagFilterCheck(string tag)
    {
        switch (tagFilterMode) {
            case tagFilterModeEnum.disabled:
                return true;
            case tagFilterModeEnum.whitelist:
                if (tagFilter.Contains(tag)) {
                    return true;
                }
                return false;
            case tagFilterModeEnum.blacklist:
                if (!tagFilter.Contains(tag)) {
                    return true;
                }
                return false;
        }
        return false;
    }

    void Update() {
        distanceAbs = float.PositiveInfinity;
        foreach (Collider2D collider in colliders) {
            float dist = Vector2.Distance(selfCollider.attachedRigidbody.transform.position, collider.transform.position);
            if (dist < distanceAbs) {
                distanceAbs = dist;
            }
        }
    }
    public bool isColliding() {
        if (disableChecksFlag) return false;
        return m_isColliding > 0;
    }
    public void disableChecks() {
        disableChecksFlag = true;
    }
    public void enableChecks() {
        disableChecksFlag = false;
    }
    public float distance() {
        return distanceAbs;
    }
    public GameObject getOther() {
        if (colliders.Count > 0) {
            return colliders[0].gameObject;
        }
        return null;
    }
}
