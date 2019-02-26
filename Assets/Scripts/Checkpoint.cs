using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Checkpoint : MonoBehaviour
{
    private bool isCollided = false;
    private float materialTransparency;
    // Start is called before the first frame update
    void Start()
    {
        materialTransparency = GetComponent<MeshRenderer>().material.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (!isCollided) {
            isCollided = true;
            GetComponent<MeshRenderer>().material.DOFade(0.0f, 1.0f).OnComplete(() => {
                GetComponent<MeshRenderer>().material.DOFade(materialTransparency, 1.0f).SetDelay(2.0f);
                isCollided = false;
            });
        }
    }
   
}
