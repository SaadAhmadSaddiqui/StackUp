using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MovingCube : MonoBehaviour
{
    public Color32[] gameColors = new Color32[10];
    public Material stackMat;

    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }
    public MoveDirection MoveDirection { get; set; }

    [SerializeField]
    private float moveSpeed = 1f;

    private float boundSize = 5f;

    private bool dirRight = true;

    private float colorTransition;
    private Color startColor;
    private Color endColor;
    private int lastColorIndex;

    public float time;

    private float H;
    private float S;
    private float V;

    private void Start()
    {
        for (int i = 0; i < gameColors.Length; i++)
        {
            gameColors[i] = Random.ColorHSV();
        }

        startColor = gameColors[0];
        endColor = gameColors[1];
    }

    private void OnEnable()
    {

        if (LastCube == null)
        {
            LastCube = GameObject.Find("Start").GetComponent<MovingCube>();
        }
        CurrentCube = this;

        transform.localScale = LastCube.transform.localScale;
    }

    void Update()
    {

        if (MoveDirection == MoveDirection.Z)
        {
            if (dirRight)
            {
                transform.position += transform.forward * Time.deltaTime * moveSpeed;
            }
            else
            {
                transform.position -= transform.forward * Time.deltaTime * moveSpeed;
            }

            if (transform.position.z >= boundSize)
            {
                dirRight = false;
            }

            if (transform.position.z <= -boundSize)
            {
                dirRight = true;
            }
        }

        if (MoveDirection == MoveDirection.X)
        {
            if (dirRight)
            {
                transform.position += transform.right * Time.deltaTime * moveSpeed;
            }
            else
            {
                transform.position -= transform.right * Time.deltaTime * moveSpeed;
            }

            if (transform.position.x >= boundSize)
            {
                dirRight = false;
            }

            if (transform.position.x <= -boundSize)
            {
                dirRight = true;
            }
        }
    }

    internal void Stop()
    {
        moveSpeed = 0;
        float hangover = GetHangOver();

        float max = MoveDirection == MoveDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;

        if (Mathf.Abs(hangover) >= max)
        {
            LastCube = null;
            CurrentCube = null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        float direction = hangover > 0 ? 1f : -1f;

        if (MoveDirection == MoveDirection.Z)
        {
            SplitCubeOnZ(hangover, direction);
        }
        else
        {
            SplitCubeOnX(hangover, direction);
        }


        LastCube = this;
        Debug.Log(hangover);
    }

    private float GetHangOver()
    {
        if (MoveDirection == MoveDirection.Z)
        {
            return transform.position.z - LastCube.transform.position.z;
        }
        else
        {
            return transform.position.x - LastCube.transform.position.x;
        }
    }

    private void SplitCubeOnX(float hangover, float direction)
    {
        float newXSize = LastCube.transform.localScale.x - Math.Abs(hangover);
        float FallingBlockSize = transform.localScale.x - newXSize;
        float NewXPosition = LastCube.transform.position.x + (hangover / 2);
        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(NewXPosition, transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newXSize / 2f * direction);
        float fallingBlockXPosition = cubeEdge + FallingBlockSize / 2f * direction;

        SpawnDropCube(fallingBlockXPosition, FallingBlockSize);
    }

    private void SplitCubeOnZ(float hangover, float direction)
    {
        float newZSize = LastCube.transform.localScale.z - Math.Abs(hangover);
        float FallingBlockSize = transform.localScale.z - newZSize;
        float NewZPosition = LastCube.transform.position.z + (hangover / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, NewZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2f * direction);
        float fallingBlockZPosition = cubeEdge + FallingBlockSize / 2f * direction;

        SpawnDropCube(fallingBlockZPosition, FallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockPosition, float fallingBlockSize)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (MoveDirection == MoveDirection.Z)
        {
            go.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            go.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockPosition);
        }
        else
        {
            go.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            go.transform.position = new Vector3(fallingBlockPosition, transform.position.y, transform.position.z);
        }
        go.AddComponent<Rigidbody>();
        Destroy(go.gameObject, 2f);
    }
}
