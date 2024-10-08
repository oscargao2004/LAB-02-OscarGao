using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField]
    public PathManager pathManager;

    private List<Waypoint> _thePath;
    private Waypoint _target;

    public float MoveSpeed;
    public float RotateSpeed;

    public Animator animator;
    private bool isWalking;
    void Start()
    {
        _thePath = pathManager.GetPath();
        if (_thePath != null && _thePath.Count > 0)
        {
            _target = _thePath[0];
        }

        isWalking = false;
        animator.SetBool("isWalking", isWalking);
    }

    private void rotateTowardsTarget()
    {
        float stepSize = RotateSpeed * Time.deltaTime;
        Vector3 targetDir = _target.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);

    }

    private void moveForward()
    {
        float stepSize = Time.deltaTime * MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, _target.pos);
        if(distanceToTarget < stepSize)
        {
            return;
        }
        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir*stepSize);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            isWalking = !isWalking;
            animator.SetBool("isWalking", isWalking);
        }

        if (isWalking)
        {
            rotateTowardsTarget();
            moveForward();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _target = pathManager.GetNextTarget();
        if (other.gameObject.CompareTag("Wall"))
        {
            isWalking = false;
            animator.SetBool("isWalking", isWalking);
        }
    }
}
