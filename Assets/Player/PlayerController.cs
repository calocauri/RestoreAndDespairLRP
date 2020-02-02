using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    public bool player1;
    private string horizontal => player1 ? "Horizontal_1" : "Horizontal_2";
    private string vertical => player1 ? "Vertical_1" : "Vertical_2";
    private string interact => player1 ? "Interact_1" : "Interact_2";
    public Weapon arma = new Weapon();
    public float moveSpeed = 0.1f;
    public bool canMove = true;
    public Animator animator;

    private new Rigidbody rigidbody;

    public BreakablePropController collidingProp;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start() {
        StartCoroutine(InputListener());
    }

    IEnumerator InputListener() {
        float previous = 0f;
        while (true) {
            var axis = new Vector3(Input.GetAxis(horizontal), 0, Input.GetAxis(vertical));
            if (axis.sqrMagnitude > 0.01f) {
                transform.Rotate(
                        Vector3.up * (
                                Vector3.SignedAngle(transform.forward, axis, Vector3.up) + 45
                        )
                );
            }
            yield return new WaitForFixedUpdate();

            var deltaVelocity = transform.forward * Mathf.Lerp(previous, axis.sqrMagnitude, 0.5f) * moveSpeed;

            rigidbody.velocity += deltaVelocity;
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, moveSpeed);
            previous = axis.sqrMagnitude;

            if (collidingProp && Input.GetButton(interact) && !animator.GetCurrentAnimatorStateInfo(0).IsTag("attack")) {
                collidingProp.HandleInteraction(arma.damageType);
            }
        }
    }

    void OnTriggerEnter(Collider collider) {
        var temp = collider.gameObject.GetComponent<BreakablePropController>();
        if (temp) {
            // Debug.Log("OnTriggerEnter");
            collidingProp = temp;
        }
    }
    void OnTriggerExit(Collider collider) {
        var temp = collider.gameObject.GetComponent<BreakablePropController>();
        if (temp && temp == collidingProp) {
            // Debug.Log("OnTriggerExit");
            collidingProp = null;
        }
    }

}