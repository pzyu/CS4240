using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Checkpoint : MonoBehaviour {
    private bool isCollided = false;
    private float materialTransparency;

    private Stroke stroke;

    private Color materialColor;

    // Start is called before the first frame update
    void Awake() {
        stroke = GetComponentInParent<Stroke>();
        materialTransparency = GetComponent<MeshRenderer>().material.color.a;
        materialColor = GetComponent<MeshRenderer>().material.color;

        GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerExit(Collider other) {
        // If stroke needs player to hold position
        if (stroke.IsReady() && stroke.GetIsCheckpointHeld()) {
            stroke.OnPlayerExit();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (stroke.IsReady() && stroke.GetIsCheckpointHeld()) {
            stroke.OnPlayerEnter();
        } else {
            if (!isCollided && !stroke.GetIsStrokeComplete() && stroke.IsReady() && stroke.TryCheckpoint(gameObject)) {
                Debug.Log("Collided!!!" + transform.name + " Parent: " + transform.parent.name);
                isCollided = true;
                GetComponent<MeshRenderer>().material.DOFade(0.0f, 0.2f).OnComplete(() => {
                    //GetComponent<MeshRenderer>().material.DOFade(materialTransparency, 1.0f).SetDelay(2.0f);
                    isCollided = false;
                });
            }
        }
    }

    public void ShowCheckpoint() {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<MeshRenderer>().material.DOFade(1.0f, 0.01f);
    }
}
