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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        LessonManager.lessonManagerInstance.ShowMessage(message);
    }
}
