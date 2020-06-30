using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }

    [SerializeField]
    private float MoveSpeed = 1f;

    private float boundSize = 1.5f;

    private float tileTransition = 1.0f;

    private float tileSpeed = 2.5f;

    private bool isMovingOnX = false;

    private void OnEnable()
    {
        if (LastCube == null)
        {
            LastCube = GameObject.Find("Start").GetComponent<MovingCube>();
        }
        CurrentCube = this;

    }


    void Update()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.PingPong(Time.time * MoveSpeed, boundSize) + transform.position.z);
        //tileTransition += Time.deltaTime * MoveSpeed;
        //if (isMovingOnX)
        //{
        //}
        //else
        //{
        //    transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Sin(tileTransition) * boundSize);
        //}
        //if (transform.position.z < LastCube.transform.position.z && transform.position.z <= boundSize)
        //{
        transform.position += transform.forward * Time.deltaTime * MoveSpeed;
        //}
        //else if (transform.position.z >= LastCube.transform.position.z || transform.position.z == boundSize)
        //{
        //    transform.position -= transform.forward * Time.deltaTime * MoveSpeed;
        //}
    }

    internal void Stop()
    {
        MoveSpeed = 0;
        isMovingOnX = !isMovingOnX;
        float hangover = transform.position.z - LastCube.transform.position.z;
        SplitCubeOnZ(hangover);
        Debug.Log(hangover);
    }

    private void SplitCubeOnZ(float hangover)
    {
        float newZSize = LastCube.transform.localScale.z - Math.Abs(hangover);
        float FallingBlockSize = transform.localScale.z - newZSize;
        float NewZPosition = LastCube.transform.position.z + (hangover / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, NewZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2f);
        float fallingBlockZPosition = cubeEdge + FallingBlockSize / 2f;

        SpawnDropCube(fallingBlockZPosition, FallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockZPosition, float fallingBlockSize)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
        go.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockZPosition);
    }
}
