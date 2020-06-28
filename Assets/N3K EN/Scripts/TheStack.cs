using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour
{
    private const float bound_Size = 3.5f;
    private const float stack_Moving_Speed = 5.0f;
    private const float error_Margin = 0.1f;

    private GameObject[] theStack;
    private Vector2 stackBounds = new Vector2(bound_Size, bound_Size);

    private int scoreCount = 0;
    private int stackIndex;
    private int combo = 0;

    private float tileTransition = 0.0f;
    private float tileSpeed = 2.5f;
    private float secondaryPosition;

    private bool isMovingOnX = true;
    private bool gameOver = false;

    private Vector3 desiredPosition;
    private Vector3 lastTilePosition;

    // Start is called before the first frame update
    void Start()
    {
        theStack = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            theStack[i] = transform.GetChild(i).gameObject;
        }

        stackIndex = transform.childCount - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceTile())
            {
                SpawnTile();
                scoreCount++;
            }
            else
            {
                EndGame();
            }
        }

        MoveTile();

        // Move the stack
        transform.position = Vector3.Lerp(transform.position, desiredPosition, stack_Moving_Speed * Time.deltaTime);
    }

    private void MoveTile()
    {
        if (gameOver)
        {
            return;
        }

        tileTransition += Time.deltaTime * tileSpeed;
        if (isMovingOnX)
        {
            theStack[stackIndex].transform.localPosition = new Vector3(Mathf.Sin(tileTransition) * bound_Size, scoreCount, secondaryPosition);
        }
        else
        {
            theStack[stackIndex].transform.localPosition = new Vector3(secondaryPosition, scoreCount, Mathf.Sin(tileTransition) * bound_Size);
        }
    }

    private void SpawnTile()
    {
        lastTilePosition = theStack[stackIndex].transform.localPosition;
        stackIndex--;
        if (stackIndex < 0)
        {
            stackIndex = transform.childCount - 1;
        }

        desiredPosition = (Vector3.down) * scoreCount;
        theStack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0);
    }

    private bool PlaceTile()
    {
        Transform t = theStack[stackIndex].transform;

        if (isMovingOnX)
        {

            float deltaX = lastTilePosition.x - t.position.x;
            if (Mathf.Abs(deltaX) > error_Margin)
            {
                //Cut the tile
                combo = 0;
                stackBounds.x -= Mathf.Abs(deltaX);
                if (stackBounds.x <= 0)
                {
                    return false;
                }

                float middle = lastTilePosition.x + t.localPosition.x / 2;
                t.localScale = (new Vector3(stackBounds.x, 1, stackBounds.y));
                t.localPosition = new Vector3(middle - (lastTilePosition.x / 2), scoreCount, lastTilePosition.z);
            }
        }
        else
        {
            float deltaZ = lastTilePosition.z - t.position.z;
            if (Mathf.Abs(deltaZ) > error_Margin)
            {
                //Cut the tile
                combo = 0;
                stackBounds.y -= Mathf.Abs(deltaZ);
                if (stackBounds.y <= 0)
                {
                    return false;
                }

                float middle = lastTilePosition.z + t.localPosition.z / 2;
                t.localScale = (new Vector3(stackBounds.x, 1, stackBounds.y));
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - (lastTilePosition.z / 2));
            }
        }

        secondaryPosition = (isMovingOnX) ? t.localPosition.x : t.localPosition.z;

        isMovingOnX = !isMovingOnX;

        return true;
    }

    //private bool CutTile()
    //{
    //    float deltaX = lastTilePosition.x - t.position.x;
    //    if (Mathf.Abs(deltaX) > error_Margin)
    //    {
    //        //Cut the tile
    //        combo = 0;
    //        stackBounds.x -= Mathf.Abs(deltaX);
    //        if (stackBounds.x <= 0)
    //        {
    //            return false;
    //        }

    //        float middle = lastTilePosition.x + t.localPosition.x / 2;
    //        t.localScale = (new Vector3(stackBounds.x, 1, stackBounds.y));
    //        t.localPosition = new Vector3(middle - (lastTilePosition.x / 2), scoreCount, lastTilePosition.z);
    //    }
    //}

    private void EndGame()
    {
        Debug.Log("Game over!");
        gameOver = true;
        theStack[stackIndex].AddComponent<Rigidbody>();
    }
}
