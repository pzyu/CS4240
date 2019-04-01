using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stroke : MonoBehaviour
{
    private Move move;

    [SerializeField]
    private GameObject trailPrefab;
    private GameObject trail;
    ParticleSystem.EmissionModule emissionModule;

    private List<GameObject> checkpointList;
    private int currentTarget = 0;

    private bool isStrokeComplete = false;
    private bool isReady = false;

    [SerializeField]
    private bool isCheckpointHeld = false;
    private bool isPlayerInCheckpoint = false;

    [SerializeField]
    private bool isOneDirection = true;

    private Tweener tweener;

    private void Awake() {
        move = GetComponentInParent<Move>();

        checkpointList = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++) {
            checkpointList.Add(transform.GetChild(i).gameObject);
            // 9 is checkpoint
            checkpointList[i].gameObject.layer = 9;
        }
        
        trail = Lean.Pool.LeanPool.Spawn(trailPrefab);
        emissionModule = trail.GetComponent<ParticleSystem>().emission;

        ResetStroke(false);

        Hide();
    }

    public void ResetStroke(bool show = true) {
        if (show) {
            Show();
        }
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

        emissionModule.rateOverTime = 0;
        tweener.Pause();
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
        ShowTrail();
    }

    public void SetReady(bool ready = true) {
        isReady = ready;
    }

    public bool IsReady() {
        return isReady;
    }

    public void SetStrokeComplete(bool complete = true) {
        isStrokeComplete = complete;
        SetReady(false);

        Debug.Log("[Stroke] Checkpoint complete!");
    }

    public bool GetIsStrokeComplete() {
        return isStrokeComplete;
    }

    public bool TryCheckpoint(GameObject checkpoint) {
        if (GetIsStrokeComplete() || !IsReady()) {
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

            //Debug.Log("[Stroke] Player entered: " + transform.name);
        }
    }

    public void OnPlayerExit() {
        if (isPlayerInCheckpoint) {
            isPlayerInCheckpoint = false;

            //Debug.Log("[Stroke] Player exit: " + transform.name);
        }
    }

    private void ShowTrail() {
        if (GetIsCheckpointHeld()) {
            //return;
        }

        int index = 0;
        int direction = 1;

        Vector3 position = checkpointList[index].transform.position;

        if (!trail) {
            trail = Lean.Pool.LeanPool.Spawn(trailPrefab);
        }

        tweener = trail.transform.DOMove(position, 0.5f).SetEase(Ease.Linear);
        tweener.OnComplete(() => {

            Vector3 originalPosition = position;
            
            index += direction;

            if (isOneDirection) {
                // If exceed count
                if (index >= transform.childCount) {
                    index = 0;
                    originalPosition = checkpointList[index].transform.position;
                    emissionModule.rateOverTime = 0;
                }
            } else {
                if (index >= transform.childCount) {
                    index = transform.childCount - 1;
                    direction = -1;
                    originalPosition = checkpointList[index].transform.position;
                    emissionModule.rateOverTime = 0;
                }

                if (index < 0) {
                    index = 0;
                    direction = 1;
                    originalPosition = checkpointList[index].transform.position;
                    emissionModule.rateOverTime = 0;
                }
            }

            position = checkpointList[index].transform.position;

            tweener.ChangeValues(originalPosition, position);
            tweener.Play();
        });

        tweener.OnUpdate(() => {
            emissionModule.rateOverTime = 20;
        });
    }
}
