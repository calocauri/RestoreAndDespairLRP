using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCurveManager : Singleton<AnimationCurveManager> {

    [SerializeField]
    private AnimationCurve restoreCurve;
    public AnimationCurve RestoreCurve => restoreCurve;

    [SerializeField]
    private AnimationCurve restoreRotationCurve;
    public AnimationCurve RestoreRotationCurve => restoreRotationCurve;


    protected override void Awake() {
        base.Awake();
    }

}