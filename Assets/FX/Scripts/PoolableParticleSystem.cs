using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableParticleSystem : PoolableObject {

    [SerializeField]
    private new ParticleSystem particleSystem;
    public ParticleSystem ParticleSystem => particleSystem;

    private void OnParticleSystemStopped() {
        ReturnToPool();
    }

    public override void OnGetFromPool() {

    }

    public override void OnReturnToPool() {

    }
}