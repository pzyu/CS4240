using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager audioManagerInstance;

    [SerializeField]
    private AudioSource bgmSource;
    [SerializeField]
    private AudioSource sfxSource;

    public enum BGM {
        CALM
    }

    public enum SFX {
        SUCCESS
    }

    [SerializeField]
    private List<AudioClip> bgmList;

    [SerializeField]
    private List<AudioClip> sfxList;



    private void Awake() {
        if (!audioManagerInstance) {
            audioManagerInstance = this;
        } else {
            Destroy(audioManagerInstance);
        }

        bgmSource.volume = 0.3f;
        bgmSource.loop = true;

        sfxSource.volume = 0.6f;
    }

    // Start is called before the first frame update
    void Start() {
        PlayBGM(BGM.CALM);
    }

    // Update is called once per frame
    void Update() {

    }

    public void PlayBGM(BGM bgmToPlay) {
        bgmSource.clip = bgmList[(int)bgmToPlay];
        bgmSource.Play();
    }

    public void PlaySFX(SFX sfxToPlay) {
        sfxSource.PlayOneShot(sfxList[(int)sfxToPlay]);
    }

}
