using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePropController : PoolableObject, IInteractable {

    [SerializeField]
    private Vector2Int size;
    public Vector2Int Size => size;

    [SerializeField]
    private MaterialType materialType;
    public MaterialType Type => materialType;

    [SerializeField]
    private GameObject Fixed;
    [SerializeField]
    private GameObject Destroyed;
    private Rigidbody[] DestroyedPiecesRigidbodies;
    private Collider[] DestroyedPiecesColliders;

    // TODO: set real health
    private int InitialHealth;
    // {
    //     get {
    //         // return size.x * size.y;
    //     }
    // }
    private new Rigidbody rigidbody;
    private new Collider collider;

    public PropState State { get; private set; }
    public int Health { get; private set; }

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        InitialHealth = Random.Range(1, 10);
        Init();

        if (Fixed != null && Destroyed != null) {
            DestroyedPiecesRigidbodies = Destroyed.GetComponentsInChildren<Rigidbody>();
            DestroyedPiecesColliders = Destroyed.GetComponentsInChildren<Collider>();
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            DestroyProp();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            RepairProp();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            HandleInteraction(InteractionType.Blunt);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            HandleInteraction(InteractionType.Cut);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            HandleInteraction(InteractionType.Pierce);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            HandleInteraction(InteractionType.Heal);
        }
    }

    private void Init() {
        HandleState();
        Health = InitialHealth;
    }

    private void HandleState() {
        if (State == PropState.Fixed) {
            Fixed.SetActive(true);
            Destroyed.SetActive(false);
        }
        else {
            Fixed.SetActive(false);
            Destroyed.SetActive(true);
        }
    }

    private void DestroyProp() {
        State = PropState.Destroyed;
        HandleState();

        var explosionPos = transform.position;
        var explosionForce = 50f;

        for (int i = 0; i < DestroyedPiecesRigidbodies.Length; i++) {
            Rigidbody piece = DestroyedPiecesRigidbodies[i];
            piece.useGravity = true;
            piece.isKinematic = false;

            DestroyedPiecesColliders[i].enabled = true;

            piece.AddForceAtPosition(
                Random.insideUnitSphere.normalized * explosionForce,
                explosionPos,
                ForceMode.Impulse
            );
        }
    }

    private void RepairProp() {
        StartCoroutine(CRepairPropAnimation());
    }

    private IEnumerator CRepairPropAnimation() {
        State = PropState.Fixing;

        float duration = 1f;
        float elapsed = 0f;
        var initialPositions = new List<Vector3>();
        var initialRotations = new List<Vector3>();
        for (int i = 0; i < DestroyedPiecesRigidbodies.Length; i++) {
            Rigidbody piece = DestroyedPiecesRigidbodies[i];
            piece.useGravity = false;
            piece.isKinematic = true;

            DestroyedPiecesColliders[i].enabled = false;

            initialPositions.Add(piece.transform.localPosition);
            initialRotations.Add(piece.transform.localEulerAngles);
        }

        while (elapsed < duration) {
            float t = elapsed / duration;

            for (int i = 0; i < DestroyedPiecesRigidbodies.Length; i++) {
                Rigidbody piece = DestroyedPiecesRigidbodies[i];
                piece.transform.localPosition = Vector3.Lerp(
                    initialPositions[i],
                    Vector3.zero,
                    AnimationCurveManager.Shared.RestoreCurve.Evaluate(t)
                );
                piece.transform.localEulerAngles = Vector3.Lerp(
                    initialRotations[i],
                    Vector3.zero,
                    AnimationCurveManager.Shared.RestoreRotationCurve.Evaluate(t)
                );
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        FinishFixed();
    }

    private void FinishFixed() {
        foreach (var piece in DestroyedPiecesRigidbodies) {
            piece.transform.localPosition = Vector3.zero;
            piece.transform.localEulerAngles = Vector3.zero;
        }

        State = PropState.Fixed;
        HandleState();
    }

    private void HandleDeath() {
        // TODO: disable interaction
        // rigidbody.isKinematic = true;
        DestroyProp();
    }

    private void HandleFix() {
        // TODO: enable interaction
        // rigidbody.isKinematic = false;
        RepairProp();
    }

    public void HandleInteraction(InteractionType interactionType) {
        if (State == PropState.Destroyed && interactionType != InteractionType.Heal) {
            return;
        }

        if (interactionType == InteractionType.Heal && State != PropState.Fixed && State != PropState.Fixing) {
            FXSpawner.Shared.StartInteractionFX(InteractionType.Heal, transform);
        }
        else if (interactionType != InteractionType.Heal) {
            FXSpawner.Shared.StartMaterialFX(materialType, transform);
        }

        var startingHealth = Health;
        var damage = InteractionHandler.CalculateInteractionResult(materialType, interactionType);
        Health += damage;

        Health = Mathf.Clamp(Health, 0, InitialHealth);

        if (Health <= 0 && State != PropState.Destroyed) {
            HandleDeath();
        }
        else if (Health >= InitialHealth && State != PropState.Fixed && State != PropState.Fixing) {
            HandleFix();
        }
    }

    public override void OnGetFromPool() {

    }

    public override void OnReturnToPool() {
        FinishFixed();
    }

    public override void OnCreatedFromPool(GenericObjectPool pool) {
        base.OnCreatedFromPool(pool);
    }

}