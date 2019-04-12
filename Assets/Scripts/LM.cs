using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LM : MonoBehaviour {
    public static LM lessonManagerInstance;

    [System.Serializable]
    public struct MoveList {
        public List<Move> list;
    }

    [System.Serializable]
    public struct SetList {
        public List<MoveList> list;
    }

    [SerializeField]
    // A list of Sets, each set contains a list of moves
    private SetList setList;

    [SerializeField]
    // Anchor used to determine where to place the move in front of the player
    private GameObject playerAnchor;

    [SerializeField]
    private GameObject playerCamera;

    [SerializeField]
    private List<Animator> targetsControllerList;

    private int currentSet = 0;
    private int currentMove = 0;

    private float currentClipTime;
    private float currentClipLength;
    private int clipLoops = 1;
    private bool isAnimationComplete = false;
    private bool isMoveComplete = false;

    private void Awake() {
        if (!lessonManagerInstance) {
            lessonManagerInstance = this;
        } else {
            Destroy(lessonManagerInstance);
        }
    }

    // Start is called before the first frame update
    void Start() {
        //InitializeTargetsControllers();
        PopulateSetsWithMoves();
        HideLessonAnchor();

        //StartLesson();
    }

    public void StartLesson() {
        ShowMove(setList.list[currentSet].list[currentMove]);
        StartCoroutine(CheckMoves());

        CenterLessonAnchor();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Space)) {
            CenterLessonAnchor();
        }
    }

    public void ResetLoops() {
        clipLoops = 1;
    }

    public void SetAnimationComplete() {
        isAnimationComplete = true;
    }

    private IEnumerator CheckMoves() {
        yield return new WaitForSeconds(0.2f);
        if (isAnimationComplete) {
            isAnimationComplete = false;

            if (isMoveComplete) {
                StartNextMove();
            }
        }
        StartCoroutine(CheckMoves());
    }

    private float GetCurrentClipTime() {
        AnimatorStateInfo animatorState = targetsControllerList[0].GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] myAnimatorClip = targetsControllerList[0].GetCurrentAnimatorClipInfo(0);
        currentClipTime = myAnimatorClip[0].clip.length * animatorState.normalizedTime;

        return currentClipTime;
    }

    private float GetCurrentClipLengthOld() {

        AnimatorStateInfo animatorState = targetsControllerList[0].GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] myAnimatorClip = targetsControllerList[0].GetCurrentAnimatorClipInfo(0);
        currentClipLength = myAnimatorClip[0].clip.length * clipLoops;

        return currentClipLength;
    }

    public void CenterLessonAnchor() {
        playerAnchor.transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y - 0.3f, playerCamera.transform.position.z - 0.1f);
        playerAnchor.transform.Rotate(new Vector3(0, playerCamera.transform.rotation.eulerAngles.y, 0));
    }

    public void HideLessonAnchor() {
        playerAnchor.transform.position = new Vector3(0, -10, 0);
    }

    private void InitializeTargetsControllers() {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Targets");

        Debug.LogWarning("Targets length: " + targets.Length);

        for (int i = 0; i < targets.Length; i++) {
            Debug.Log(targets[i].GetComponent<Animator>());
            targetsControllerList.Add(targets[i].GetComponent<Animator>());
        }
    }

    private void PopulateSetsWithMoves() {
        Vector3 rotation = new Vector3(0, 180, 0);
        for (int i = 0; i < GetNumberOfSets(); i++) {
            for (int j = 0; j < GetNumberOfMovesInSet(i); j++) {
                Move move = Instantiate(setList.list[i].list[j], playerAnchor.transform.position, transform.rotation, playerAnchor.transform);
                setList.list[i].list[j] = move;
                HideMove(move);
            }
        }
    }

    public void CompleteMove() {
        clipLoops = 1;
        isMoveComplete = true;
    }

    private void StartNextMove() {
        isMoveComplete = false;
        HideMove(GetCurrentMove());

        currentMove++;

        if (AreAllMovesInSetComplete()) {
            Debug.Log("SET COMPLETE!");
            CompleteSet();
        } else {
            ShowMove(GetCurrentMove());
        }
    }

    public void CompleteSet() {
        currentSet++;

        if (AreAllSetsComplete()) {
            Debug.Log("All sets complete!");
            HideLessonAnchor();
            Player.playerInstance.SetPromptText("Thanks for playing!");
        }

        //GetCurrentSet();
    }

    public void ShowMove(Move move) {
        // Show move in front of player
        move.Show();

        UpdateTargetController();
    }

    public void HideMove(Move move) {
        // Hide move from the player
        move.Hide();
    }

    private bool AreAllMovesInSetComplete() {
        return currentMove >= setList.list[currentSet].list.Count;
    }

    private bool AreAllSetsComplete() {
        return currentSet >= setList.list.Count;
    }

    private MoveList GetCurrentSet() {
        return setList.list[currentSet];
    }

    private Move GetCurrentMove() {
        return setList.list[currentSet].list[currentMove];
    }

    public void ShowMessage(string message) {
        Debug.Log("[LessonManager] Received message: " + message);
    }

    // Returns the number of sets in set list
    private int GetNumberOfSets() {
        return setList.list.Count;
    }

    // Returns the number of moves in a set given the set index
    private int GetNumberOfMovesInSet(int setIndex) {
        return setList.list[currentSet].list.Count;
    }

    private void UpdateTargetController() {
        Debug.Log("New move: " + GetCurrentMove().GetMoveType());

        switch (GetCurrentMove().GetMoveType()) {
            case Move.TYPE.FLAPPYBIRD:
                //targetsController.SetTrigger("FlappyBirdTrigger");
                SetTargetsTrigger("FlappyBirdTrigger");
                break;
            case Move.TYPE.FRONTALWAVE:
                //targetsController.SetTrigger("FrontalWaveTrigger");
                SetTargetsTrigger("FrontalWaveTrigger");
                break;
            case Move.TYPE.TREEHUG:
                //targetsController.SetTrigger("TreeHugTrigger");
                SetTargetsTrigger("TreeHugTrigger");
                break;
            case Move.TYPE.BEACHBALL:
                //targetsController.SetTrigger("BeachBallTrigger");
                SetTargetsTrigger("BeachBallTrigger");
                break;
            case Move.TYPE.SIDEPUSHLEFT:
                //targetsController.SetTrigger("BeachBallTrigger");
                SetTargetsTrigger("SidePushLeft");
                break;
            case Move.TYPE.SIDEPUSHRIGHT:
                //targetsController.SetTrigger("BeachBallTrigger");
                SetTargetsTrigger("SidePushRight");
                break;
            default:
                break;
        }


        GetCurrentClipLength();
    }

    private void SetTargetsTrigger(string triggerName) {
        for (int i = 0; i < targetsControllerList.Count; i++) {
            targetsControllerList[i].SetTrigger(triggerName);
        }
    }

    public float GetCurrentClipLength() {
        float length = targetsControllerList[0].GetCurrentAnimatorStateInfo(0).length;

        if (GetCurrentMove().GetMoveType() == Move.TYPE.SIDEPUSHLEFT || GetCurrentMove().GetMoveType() == Move.TYPE.SIDEPUSHRIGHT) {
            length = 4;
        } else {
            length = 2.5f;
        }

        Debug.Log("Current clip length: " + length);

        return length;
    }
}
