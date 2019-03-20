using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stroke : MonoBehaviour
{
    private Move move;

    [SerializeField]
    private GameObject trailRenderer;

    private List<GameObject> checkpointList;
    private int currentTarget = 0;

    private bool isStrokeComplete = false;
    private bool isReady = false;

    [SerializeField]
    private bool isCheckpointHeld = false;
    private bool isPlayerInCheckpoint = false;

    private void Awake() {
        move = GetComponentInParent<Move>();

        checkpointList = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++) {
            checkpointList.Add(transform.GetChild(i).gameObject);
            // 9 is checkpoint
            checkpointList[i].gameObject.layer = 9;
        }

        ResetStroke();

        Hide();
    }

    public void ResetStroke() {
        Show();
        currentTarget = 0;
        isStrokeComplete = false;
        isPlayerInCheckpoint = false;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide() {
        if (checkpointList == null) {
            return;
        }

        for (int i = 0; i < transform.childCount; i++) {
            //Debug.Log(checkpointList[i].transform.parent.name);
            checkpointList[i].GetComponent<MeshRenderer>().material.DOFade(0.0f, 0.1f);
        }
    }

    public void Show() {
        if (checkpointList == null) {
            return;
        }

        Debug.Log("SHOWING STROKE");

        float transparency = 0.8f;
        float increment = transparency / transform.transform.childCount;

        for (int i = 0; i < transform.childCount; i++) {
            checkpointList[i].GetComponent<MeshRenderer>().material.DOFade(transparency, 0.1f);
            transparency -= increment;
        }

        SetReady();

    }

    public void SetReady(bool ready = true) {
        isReady = ready;
    }

    public bool IsReady() {
        return isReady;
    }

    public void SetStrokeComplete(bool complete = true) {
        isStrokeComplete = complete;

        Debug.Log("[Stroke] Checkpoint complete!");
    }

    public bool GetIsStrokeComplete() {
        return isStrokeComplete;
    }

    public bool TryCheckpoint(GameObject checkpoint) {
        if (GetIsStrokeComplete()) {
            return false;
        }

        if (checkpointList[currentTarget] == checkpoint) {
            Debug.Log("[Stroke] Checkpoint met!");
            currentTarget++;

            if (currentTarget >= checkpointList.Count) {
                SetStrokeComplete();
            }

            return true;
        } else {
            Debug.Log("[Stroke] Wrong checkpoint!");
        }

        return false;
    }

    public bool GetIsCheckpointHeld() {
        return isCheckpointHeld;
    }

    public bool GetIsPlayerInCheckpoint() {
        return isPlayerInCheckpoint;
    }

    public void OnPlayerEnter() {
        if (!isPlayerInCheckpoint) {
            isPlayerInCheckpoint = true;

            Debug.Log("[Stroke] Player entered: " + transform.name);
        }
    }

    public void OnPlayerExit() {
        if (isPlayerInCheckpoint) {
            isPlayerInCheckpoint = false;

            Debug.Log("[Stroke] Player exit: " + transform.name);
        }
    }
}
