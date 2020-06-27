using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private  set; }

    [SerializeField]
    private float MoveSpeed = 1f;


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
        transform.position += transform.forward * Time.deltaTime * MoveSpeed;
    }

    internal void Stop()
    {
        MoveSpeed = 0;
        float hangover = transform.position.z - LastCube.transform.position.z;
        SplitCubeOnZ(hangover);
        Debug.Log(hangover);
    }

    private void SplitCubeOnZ(float hangover)
    {
        float newZSize = LastCube.transform.localScale.z - Math.Abs(hangover);
        float FallingBlockSize = transform.localScale.z - newZSize;
        float NewZPosition = LastCube.transform.position.z + (hangover / 2);
        transform.localScale = new Vector3( transform.localScale.x, transform.localScale.y, NewZPosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, NewZPosition);
    }
}
