using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stroke : MonoBehaviour
{
    private List<GameObject> checkpointList;

    private void Awake() {
        checkpointList = new List<GameObject>();

        float transparency = 0.8f;
        float increment = transparency / transform.transform.childCount;

        for (int i = 0; i < transform.childCount; i++) {
            checkpointList.Add(transform.GetChild(i).gameObject);
            checkpointList[i].GetComponent<MeshRenderer>().material.DOFade(transparency, 0.1f);
            transparency -= increment;
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
}
