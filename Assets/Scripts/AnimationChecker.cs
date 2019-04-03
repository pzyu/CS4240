using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChecker : MonoBehaviour
{
    private bool isAnimationComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAnimationComplete() {
        Debug.Log("Set animation complete!");
        isAnimationComplete = true;

        LessonManager.lessonManagerInstance.SetAnimationComplete();
    }
}
