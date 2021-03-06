﻿using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {

    public static float fieldOfViewAngle = 110f;
    public bool playerInSight;
    public Vector3 personalLastSighting;
    
    private NavMeshAgent nav;
    private SphereCollider col;
    private Animator anim;

    [SerializeField]
    private GameObject playerObj;
    private Animator playerAnim;
    
    private Vector3 previousSighting;
    
    void Awake ()
    {
        nav = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();

        //gameController = GameObject.FindGameObjectWithTag(Tags.gameController);
        //hash = gameController.GetComponent<HashIDs>();
        //lastPlayerSighting = gameController.GetComponent<LastPlayerSighting>();

        
        Vector3 resetPosition = LastPlayerSighting.resetPosition;
        personalLastSighting = resetPosition;
        previousSighting = resetPosition;
    }

    void Start ()
    {
        playerObj = PlayerMovement.player;
        playerAnim = playerObj.GetComponent<Animator>();
    }
    void Update ()
    {
        Vector3 globalPlayerSighting = LastPlayerSighting.position;
        
        if (globalPlayerSighting != previousSighting)
        {
            personalLastSighting = globalPlayerSighting;
        }
        previousSighting = globalPlayerSighting;

        if (0 < PlayerHealth.Health)
            anim.SetBool(HashIDs.playerInSightBool, playerInSight);
        else
            anim.SetBool(HashIDs.playerInSightBool, false);
    }

    void OnTriggerStay (Collider other)
    {
        if (other.CompareTag(Tags.player))
        {
            playerInSight = false;
            if (0 >= PlayerHealth.Health)
                return;
            
            Vector3 myPosition = transform.position;
            Vector3 playerPosition = playerObj.transform.position;

            Vector3 direction = other.transform.position - myPosition;

            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;

                if (Physics.Raycast(myPosition + transform.up, direction.normalized, out hit, col.radius))
                {
                    if (hit.collider.CompareTag(Tags.player))
                    {
                        playerInSight = true;
                        LastPlayerSighting.position = playerPosition;
                    }
                }
            }

            int playerLayerZeroStateHash = playerAnim.GetCurrentAnimatorStateInfo(0).fullPathHash;
            int playerLayerOneStateHash = playerAnim.GetCurrentAnimatorStateInfo(1).fullPathHash;

            if (HashIDs.locomotionSate == playerLayerZeroStateHash || HashIDs.shoutSate == playerLayerOneStateHash)
            {
                if (CalculatePathLength(playerPosition) <= col.radius)
                {
                    personalLastSighting = playerPosition;
                }
            }
        }
    }

    void OnTriggerExit (Collider other)
    {
        if(other.CompareTag(Tags.player))
        {
            playerInSight = false;
        }
    }

    float CalculatePathLength (Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();

        if (nav.enabled)
        {
            nav.CalculatePath(targetPosition, path);
        }

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for (int i=0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for (int i=0; i < allWayPoints.Length-1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i+1]);
        }

        return pathLength;
    }
}
