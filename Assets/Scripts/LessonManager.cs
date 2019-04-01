using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonManager : MonoBehaviour
{
    public static LessonManager lessonManagerInstance;

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
    private Animator targetsController;

    [SerializeField]
    private List<Animator> targetsControllerList;

    private int currentSet = 0;
    private int currentMove = 0;

    private void Awake() {
        if (!lessonManagerInstance) {
            lessonManagerInstance = this;
        } else {
            Destroy(lessonManagerInstance);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //InitializeTargetsControllers();
        PopulateSetsWithMoves();
        ShowMove(setList.list[currentSet].list[currentMove]);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        HideMove(GetCurrentMove());

        currentMove++;
        
        ShowMove(GetCurrentMove());
    }

    public void CompleteSet() {
        currentSet++;

        GetCurrentSet();
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
            default:
                break;
        }
    }

    private void SetTargetsTrigger(string triggerName) {
        for (int i = 0; i < targetsControllerList.Count; i++) {
            targetsControllerList[i].SetTrigger(triggerName);
        }
    }
}
