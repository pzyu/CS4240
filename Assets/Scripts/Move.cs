using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // A move consists of 5 portions: Left hand, right hand, left feet, right feet, Head (For determining knee bends)
    // Stroke should relay back to move about the status of the strokes: whether it's in progress or out of bounds
    [SerializeField]
    private Stroke leftStroke;
    [SerializeField]
    private Stroke rightStroke;
    [SerializeField]
    private Stroke leftFeet;
    [SerializeField]
    private Stroke rightFeet;
    [SerializeField]
    private Stroke head;

    [SerializeField]
    private int amountToRepeat = 2;

    private int currentReps = 0;

    public enum TYPE {
        FLAPPYBIRD,
        FRONTALWAVE,
        TREEHUG,
        BEACHBALL,
		SIDEPUSHLEFT,
		SIDEPUSHRIGHT
    }

    [SerializeField]
    private TYPE moveType;

    private bool skip = false;
    
    // Start is called before the first frame update
    void Start()
    {
		Debug.Log(gameObject.name);

        StartCoroutine(CheckStrokesCoroutine());   
    }

    // Update is called once per frame
    void Update()
    {
    }

    public TYPE GetMoveType() {
        return moveType;
    }

    private IEnumerator CheckStrokesCoroutine() {
        Debug.Log("[Move] Current rep: " + (currentReps + 1));
 
        yield return new WaitUntil(AreAllStrokesComplete);

        AudioManager.audioManagerInstance.PlaySFX(AudioManager.SFX.SUCCESS);
        Player.playerInstance.ShowPositiveMessage();

        Debug.Log(transform.name + " Adding current rep");
        currentReps++;
        
        if (currentReps >= amountToRepeat) {
            Debug.Log("[Move] Move complete!");
            LM.lessonManagerInstance.CompleteMove();
        } else {
            yield return new WaitForSeconds(0.5f);
            ResetAllStrokes();
            StartCoroutine(CheckStrokesCoroutine());
        }
    }

    private bool AreAllStrokesComplete() {
        return leftStroke.GetIsStrokeComplete() && rightStroke.GetIsStrokeComplete();
        // && leftFeet.GetIsPlayerInCheckpoint() && rightFeet.GetIsPlayerInCheckpoint() && head.GetIsPlayerInCheckpoint();
    }

    private void ResetAllStrokes() {
        leftStroke.ResetStroke();
        rightStroke.ResetStroke();
        leftFeet.ResetStroke();
        rightFeet.ResetStroke();
        head.ResetStroke();
    }

    public void CheckLeftStroke() {

    }

    public void CheckRightStroke() {

    }

    public void CheckLeftFeet() {

    }

    public void CheckRightFeet() {

    }

    public void CheckHead() {

    }

    // Sends a message to LessonManager in case there is something to notify the player
    public void SendMessageToLessonManager(string message) {
        LM.lessonManagerInstance.ShowMessage(message);
    }

    public void Hide() {
        Debug.Log("Hiding move");
        Stroke[] strokeList = GetComponentsInChildren<Stroke>();
        foreach (Stroke stroke in strokeList) {
            stroke.Hide();
        }
    }

    public void Show() {
        Debug.Log("Showing move");
        Stroke[] strokeList = GetComponentsInChildren<Stroke>();
        foreach (Stroke stroke in strokeList) {
            stroke.Show();
        }
    }
}
