using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Player : MonoBehaviour {
    public static Player playerInstance;

    [SerializeField]
    private Transform playerAnchor;

    [SerializeField]
    private SideCollider leftCollider;

    [SerializeField]
    private SideCollider rightCollider;

    [SerializeField]
    private TextMeshProUGUI promptText;

    [SerializeField]
    private Image loadingWheel;

    private float timeRequiredToOpenMenu = 2.0f;
    private float currentTime = 0.0f;

    private bool isMenuOpen = false;
    private bool isCalibrating;

    private float armLength = 0.0f;

    private int tutorialIndex = 0;

    [SerializeField]
    private List<string> positiveMessageList;

    private void Awake() {
        if (!playerInstance) {
            playerInstance = this;
        } else {
            Destroy(playerInstance);
        }
    }

    // Start is called before the first frame update
    void Start() {
        isCalibrating = true;
        loadingWheel.fillAmount = 0.0f;
        //HidePromptText(0, true);
        promptText.text = "Please stretch your arms to the side";
    }

    // Update is called once per frame
    void Update() {
        //CenterPlayerOnAnchor();
        if (isCalibrating) {
            CheckMenu();
        }
    }

    private void CheckMenu() {
        if (leftCollider.GetHasPlayerCollided() && rightCollider.GetHasPlayerCollided()) {
            currentTime += Time.deltaTime;

            if (loadingWheel.color.a == 0) {
                loadingWheel.DOFade(1.0f, 0.25f);
            }

            float ratio = currentTime / timeRequiredToOpenMenu;
            loadingWheel.fillAmount = ratio;

            if (currentTime >= timeRequiredToOpenMenu) {
                OpenMenu();
                currentTime = 0;
            }
        } else {
            if (currentTime != 0) {
                currentTime = 0;
                loadingWheel.DOFade(0.0f, 0.25f);
            }
        }
    }

    private void OpenMenu() {
        loadingWheel.DOFade(0.0f, 0.25f);
        SetPromptText("Great job!", 4.0f);
        
        Vector3 difference = leftCollider.GetColliderPosition() - transform.position;
        armLength = Mathf.Abs(difference.x);
        isCalibrating = false;

        StartCoroutine(StartTutorial());
    }

    private IEnumerator StartTutorial() {
        yield return new WaitForSeconds(2.0f);
        SetPromptText("Please put down your hands");
        yield return new WaitForSeconds(4.0f);
        SetPromptText("Try to follow the moves as shown by the instructors");
        yield return new WaitForSeconds(4.0f);
        SetPromptText("When you have completed a move successfully, you will hear this sound!");
        yield return new WaitForSeconds(0.25f);
        AudioManager.audioManagerInstance.PlaySFX(AudioManager.SFX.SUCCESS);
        yield return new WaitForSeconds(4.0f);
        SetPromptText("Have fun!");
        yield return new WaitForSeconds(4.0f);

        LessonManager.lessonManagerInstance.StartLesson();
    }

    private void CenterPlayerOnAnchor() {
        // We want to track position
        transform.position = playerAnchor.transform.position;

        transform.rotation = playerAnchor.transform.rotation;//new Quaternion(playerAnchor.transform.rotation.x, 0, playerAnchor.transform.rotation.z, playerAnchor.transform.rotation.w);
        Vector3 rotation = new Vector3(playerAnchor.transform.rotation.eulerAngles.x, playerAnchor.transform.rotation.eulerAngles.y, playerAnchor.transform.rotation.eulerAngles.z);
        transform.Rotate(rotation);// = rotation;
    }

    public void SetPromptText(string text, float duration = 2.0f) {
        promptText.alpha = 0;
        promptText.text = text;
        promptText.DOFade(1.0f, 0.0f).OnComplete(() => {
            HidePromptText(duration);
        });
    }

    private void HidePromptText(float delay = 2.0f, bool instant = false) {
        if (!instant) {
            promptText.DOFade(0.0f, 0.5f).SetDelay(delay);
        } else {
            promptText.alpha = 0;
        }
    }

    public void ShowPositiveMessage() {
        SetPromptText(GetRandomPositiveMessage());
    }

    private string GetRandomPositiveMessage() {
        return positiveMessageList[Random.Range(0, positiveMessageList.Count)];
    }
}
