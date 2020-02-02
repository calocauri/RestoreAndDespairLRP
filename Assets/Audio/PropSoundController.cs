using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSoundController : Singleton<PropSoundController> {

    private AudioSource audioSource;

    protected override void Awake() {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
    }

    [SerializeField]
    private List<PropSoundType> sounds;

    public void PlaySoundForType(MaterialType type) {
        var propSound = sounds.Find(s => s.type == type);

        if (propSound != null) {
            audioSource.PlayOneShot(propSound.clip);
        }
    }

}

[System.Serializable]
public class PropSoundType {
    public MaterialType type;
    public AudioClip clip;
}