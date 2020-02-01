using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePropController : MonoBehaviour, IInteractable {

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
    private Rigidbody[] DestroyedPieces;

    // TODO: set real health
    private int InitialHealth;
    // {
    //     get {
    //         // return size.x * size.y;
    //     }
    // }

    public PropState State { get; private set; }
    public int Health { get; private set; }

    private void Awake() {
        InitialHealth = Random.Range(1, 10);
        Init();

        if (Fixed != null && Destroyed != null) {
            DestroyedPieces = Destroyed.GetComponentsInChildren<Rigidbody>();
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

        foreach (var piece in DestroyedPieces) {
            piece.useGravity = true;
            piece.isKinematic = false;
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
        foreach (var piece in DestroyedPieces) {
            piece.useGravity = false;
            piece.isKinematic = true;
            initialPositions.Add(piece.transform.localPosition);
            initialRotations.Add(piece.transform.localEulerAngles);
        }

        while (elapsed < duration) {
            float t = elapsed / duration;

            for (int i = 0; i < DestroyedPieces.Length; i++) {
                Rigidbody piece = DestroyedPieces[i];
                piece.transform.localPosition = Vector3.Lerp(
                    initialPositions[i],
                    Vector3.zero,
                    t
                );
                piece.transform.localEulerAngles = Vector3.Lerp(
                    initialRotations[i],
                    Vector3.zero,
                    t
                );
            }

            elapsed += Time.deltaTime;
            yield return null;
        }


        foreach (var piece in DestroyedPieces) {
            piece.transform.localPosition = Vector3.zero;
            piece.transform.localEulerAngles = Vector3.zero;
        }

        State = PropState.Fixed;
        HandleState();
    }

    private void HandleDeath() {
        // TODO: disable interaction
        DestroyProp();
    }

    private void HandleFix() {
        // TODO: enable interaction
        RepairProp();
    }

    public void HandleInteraction(InteractionType interactionType) {
        if (State == PropState.Destroyed && interactionType != InteractionType.Heal) {
            return;
        }

        var startingHealth = Health;
        var damage = InteractionHandler.CalculateInteractionResult(materialType, interactionType);
        Health += damage;
        print($"{name} : {startingHealth} + {damage} = {Health}");

        Health = Mathf.Clamp(Health, 0, InitialHealth);

        if (Health == 0) {
            HandleDeath();
        }
        else if (Health == InitialHealth) {
            HandleFix();
        }
    }
}