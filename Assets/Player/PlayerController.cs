﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    public bool player1;
    private string horizontal => player1 ? "Horizontal_1" : "Horizontal_2";
    private string vertical => player1 ? "Vertical_1" : "Vertical_2";
    private string interact => player1 ? "Interact_1" : "Interact_2";
    private string scream => player1 ? "Scream_1" : "Scream_2";
    public Weapon arma = new Weapon();
    public float moveSpeed = 0.1f;
    public bool canMove = true;
    public Animator animator;

    [SerializeField]
    private AudioClip screamClip;

    [SerializeField]
    private Transform weaponSocket;
    [SerializeField]
    private Weapon[] possibleWeapons;
    [SerializeField]
    private Transform targetPropIndicator;

    private new Rigidbody rigidbody;
    private AudioSource audioSource;

    public List<BreakablePropController> collidingProps = new List<BreakablePropController>();

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        GameManager.OnGameEnded += OnGameEnded;
        RandomWeapon();
    }

    private void OnGameEnded(float result) {
        collidingProps.Clear();
        RandomWeapon();
    }

    private void RandomWeapon() {
        if (player1) return;

        foreach (var weap in possibleWeapons) {
            weap.gameObject.SetActive(false);
        }

        arma = possibleWeapons[UnityEngine.Random.Range(0, possibleWeapons.Length)];
        arma.gameObject.SetActive(true);
    }

    void Start() {
        StartCoroutine(InputListener());
    }

    private void Update() {
        if (Input.GetButtonDown(scream)) {
            audioSource.PlayOneShot(screamClip);
        }
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
                animator.SetBool("walking", true);
            }
            else animator.SetBool("walking", false);
            yield return new WaitForFixedUpdate();

            var deltaVelocity = transform.forward * Mathf.Lerp(previous, axis.sqrMagnitude, 0.5f) * moveSpeed;

            rigidbody.velocity += deltaVelocity;
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, (canMove ? moveSpeed : 0));
            previous = axis.sqrMagnitude;

            if (collidingProps.Count > 0 && !animator.GetCurrentAnimatorStateInfo(0).IsTag("attack")) {
                var sorted = collidingProps.OrderBy(p => Vector3.Distance(p.transform.position, transform.position)).ToList();
                var target = sorted.First(p => !player1 ? p.State != PropState.Destroyed : p.State == PropState.Destroyed);
                target.HandleInteraction(arma.damageType);
                targetPropIndicator.position = target.transform.position;

                if (Input.GetButtonDown(interact) )
                    animator.SetTrigger("attack");
            }
            else {
                targetPropIndicator.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider collider) {
        var temp = collider.gameObject.GetComponent<BreakablePropController>();

        if (!player1) {
            if (temp && temp.State != PropState.Destroyed) {
                collidingProps.Add(temp);
            }
        }
        else {
            if (temp && temp.State == PropState.Destroyed) {
                collidingProps.Add(temp);
            }
        }
    }
    void OnTriggerExit(Collider collider) {
        var temp = collider.gameObject.GetComponent<BreakablePropController>();
        if (temp) {
            // Debug.Log("OnTriggerExit");
            collidingProps.Remove(temp);
        }
    }

}