using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCollider : MonoBehaviour
{
    private bool hasPlayerCollided = false;
    private Vector3 colliderPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other) {
        // If stroke needs player to hold position
        if (other.gameObject.layer == 10 && hasPlayerCollided) {
            hasPlayerCollided = false;
            colliderPosition = other.transform.position;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == 10 && !hasPlayerCollided) {
            hasPlayerCollided = true;
            colliderPosition = other.transform.position;
        }
    }

    public bool GetHasPlayerCollided() {
        return hasPlayerCollided;
    }

    public Vector3 GetColliderPosition() {
        return colliderPosition;
    }
}
