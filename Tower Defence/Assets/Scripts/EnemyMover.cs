using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    // [SerializeField] float timeForDelay = 1f;
    [SerializeField] [Range(0f,5f)] float moveSpeed = 1f;
    [SerializeField] [Range(0f,5f)] float rotationSpeed = 1f;
    // [SerializeField] List<Tile> path = new List<Tile>();
    List<Node> path = new List<Node>();
    Enemy enemy;
    GridManager gridManager;
    PathFinder pathFinder;

    void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
    }

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
        enemy = GetComponent<Enemy>();
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();
        if(resetPath)
        {
            coordinates = pathFinder.StartCoordinates;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }
        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());

        // GameObject parent = GameObject.FindGameObjectWithTag("Path");
        // foreach(Transform child in parent.transform)
        // {
        //     Tile waypoint = child.GetComponent<Tile>();
        //     if(waypoint != null)
        //     {
        //         path.Add(waypoint);
        //     }
        // }
    }

    void ReturnToStart()
    {
        // transform.position = path[0].transform.position;
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
    }

    void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);
    }

    IEnumerator FollowPath()
    {
        // foreach (Tile waypoint in path)
        for(int i = 0; i < path.Count; i++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            // Vector3 endPosition = waypoint.transform.position;

            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = Quaternion.LookRotation(endPosition - transform.position);

            float rotatePercent = 0f;
            float travelPercent = 0f;

            // transform.LookAt(endPosition);

            while (travelPercent < 1 || rotatePercent < 1f)
            {
                travelPercent += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                rotatePercent += Time.deltaTime * rotationSpeed;
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotatePercent);
                yield return new WaitForEndOfFrame();
            }
        }
        FinishPath();
    }
}