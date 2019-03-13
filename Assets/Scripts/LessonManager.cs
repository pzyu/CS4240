using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonManager : MonoBehaviour
{
    public static LessonManager lessonManagerInstance;

    [SerializeField]
    // A list of Sets, each set contains a list of moves
    private List<List<GameObject>> setList;

    [SerializeField]
    // Anchor used to determine where to place the move in front of the player
    private GameObject playerAnchor;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteMove() {
        currentMove++;

        GetCurrentMove();
    }

    public void CompleteSet() {
        currentSet++;

        GetCurrentSet();
    }

    public void ShowMove(GameObject move) {
        // Show move in front of player
    }

    public void HideMove(GameObject move) {
        // Hide move from the player
    }

    private List<GameObject> GetCurrentSet() {
        return setList[currentSet];
    }

    private GameObject GetCurrentMove() {
        return setList[currentSet][currentMove];
    }

    public void ShowMessage(string message) {
        Debug.Log("[LessonManager] Received message: " + message);
    }
}
