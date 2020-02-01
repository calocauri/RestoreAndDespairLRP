using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXSpawner : Singleton<FXSpawner> {

    [SerializeField]
    private MaterialTypeFX[] materialsFX;
    [SerializeField]
    private InteractionTypeFX[] interactionFX;

    Dictionary<MaterialType, GenericObjectPool> materialFXPools = new Dictionary<MaterialType, GenericObjectPool>();
    Dictionary<InteractionType, GenericObjectPool> interactionFXPools = new Dictionary<InteractionType, GenericObjectPool>();

    protected override void Awake() {
        base.Awake();

        foreach (var materialFX in materialsFX) {
            var newMaterialFXPool = new GenericObjectPool();
            newMaterialFXPool.Init(materialFX.particleSystem);
            materialFXPools[materialFX.type] = newMaterialFXPool;
        }
        foreach (var intertFX in interactionFX) {
            var newMaterialFXPool = new GenericObjectPool();
            newMaterialFXPool.Init(intertFX.particleSystem);
            interactionFXPools[intertFX.type] = newMaterialFXPool;
        }
    }

    public void StartMaterialFX(MaterialType materialType, Transform target) {
        var pooledObject = materialFXPools[materialType].Get();
        var pooledParticleSystem = (PoolableParticleSystem)pooledObject;
        pooledParticleSystem.ParticleSystem.Play();
        pooledParticleSystem.transform.position = target.position;
    }

    public void StartInteractionFX(InteractionType interactionType, Transform target) {
        var pooledObject = interactionFXPools[interactionType].Get();
        var pooledParticleSystem = (PoolableParticleSystem)pooledObject;
        pooledParticleSystem.ParticleSystem.Play();
        pooledParticleSystem.transform.position = target.position;
    }

}

[System.Serializable]
public class MaterialTypeFX {
    public MaterialType type;
    public PoolableParticleSystem particleSystem;
}

[System.Serializable]
public class InteractionTypeFX {
    public InteractionType type;
    public PoolableParticleSystem particleSystem;
}