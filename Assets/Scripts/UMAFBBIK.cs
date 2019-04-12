using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion;
using RootMotion.FinalIK;
 
public class UMAFBBIK : MonoBehaviour {
 
    [SerializeField] public GameObject umaCharacter;

    [SerializeField] public GameObject LeftHandTarget;
    [SerializeField] public GameObject RightHandTarget;

    [SerializeField] public GameObject LeftShoulder;
    [SerializeField] public GameObject RightShoulder;

    [SerializeField] public GameObject LeftFootTarget;
    [SerializeField] public GameObject RightFootTarget;

    [SerializeField] public GameObject TorsoTarget;

    [Range(0.0f, 1.0f)] public float LeftHandPositionWeight;
    [Range(0.0f, 1.0f)] public float LeftHandRotationWeight;
    [Range(0.0f, 1.0f)] public float RightHandPositionWeight;
    [Range(0.0f, 1.0f)] public float RightHandRotationWeight;
    [Range(0.0f, 1.0f)] public float LeftShoulderPositionWeight;
    [Range(0.0f, 1.0f)] public float RightShoulderPositionWeight;
    [Range(0.0f, 1.0f)] public float LeftFootPositionWeight;
    [Range(0.0f, 1.0f)] public float LeftFootRotationWeight;
    [Range(0.0f, 1.0f)] public float RightFootPositionWeight;
    [Range(0.0f, 1.0f)] public float RightFootRotationWeight;
    [Range(0.0f, 1.0f)] public float TorsoPositionWeight;
    [Range(0.0f, 1.0f)] public float HeadMaintainRotationWeight;


    public float lookWeight = 0f;
    public float bodyWeight = 1f;
    public float headWeight = 1f;
    public float eyesWeight = 1f;
 
    public bool retarget = false;
    public GameObject target;
 
 
    public BipedReferences references;
 
    private FullBodyBipedIK ik;
    
    void Start () {

    }

    // Update is called once per frame
    void Update () {
    if (umaCharacter != null) {
 
            BipedReferences.AutoDetectReferences(ref references, umaCharacter.transform, BipedReferences.AutoDetectParams.Default);

            ik = umaCharacter.GetComponent<FullBodyBipedIK>();

            if (ik == null) { //errors here
                ik = umaCharacter.AddComponent<FullBodyBipedIK>();
            }              
            
 
            // lookIk = umaCharacter.AddComponent<LookAtIK>();
 
            ik.SetReferences(references, null);

            ik.solver.leftHandEffector.target = LeftHandTarget.transform;
            ik.solver.leftHandEffector.positionWeight = LeftHandPositionWeight; 
            ik.solver.leftHandEffector.rotationWeight = LeftHandRotationWeight;

            ik.solver.rightHandEffector.target = RightHandTarget.transform;
            ik.solver.rightHandEffector.positionWeight = RightHandPositionWeight; 
            ik.solver.rightHandEffector.rotationWeight = RightHandRotationWeight;

            ik.solver.leftShoulderEffector.target = LeftShoulder.transform;
            ik.solver.leftShoulderEffector.positionWeight = LeftShoulderPositionWeight; 

            ik.solver.rightShoulderEffector.target = RightShoulder.transform;
            ik.solver.rightShoulderEffector.positionWeight = RightShoulderPositionWeight;

            ik.solver.leftFootEffector.target = LeftFootTarget.transform;
            ik.solver.leftFootEffector.positionWeight = LeftFootPositionWeight;
            ik.solver.leftFootEffector.rotationWeight = LeftFootRotationWeight;

            ik.solver.rightFootEffector.target = RightFootTarget.transform;
            ik.solver.rightFootEffector.positionWeight = RightFootPositionWeight;
            ik.solver.rightFootEffector.rotationWeight = RightFootRotationWeight;

            ik.solver.bodyEffector.target = TorsoTarget.transform;
            ik.solver.bodyEffector.positionWeight = TorsoPositionWeight;
            ik.solver.headMapping.maintainRotationWeight = HeadMaintainRotationWeight;

            // lookIk.solver.SetChain(references.spine, references.head, references.eyes, references.root);
 
            // lookIk.solver.bodyWeight = bodyWeight;
            // lookIk.solver.headWeight = headWeight;
            // lookIk.solver.eyesWeight = eyesWeight;
 
            ik.solver.SetLimbOrientations(BipedLimbOrientations.UMA);
 
            umaCharacter = null;
        }
 
        // lookIk.solver.IKPositionWeight = lookWeight;
 
//         if (retarget == true){
//             lookIk.solver.IKPosition = target.transform.position;
// //          retarget = false;
//         }
    }
    
 
    public void setUmaCharacter(GameObject character){
 
        umaCharacter = character;
 
    }

}