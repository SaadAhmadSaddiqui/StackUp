using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    private MovingCube cubePrefab;

    [SerializeField]
    private MoveDirection moveDirection;

    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab);
        if (MovingCube.LastCube != null && MovingCube.LastCube.gameObject != GameObject.Find("Start"))
        {
            float x = moveDirection == MoveDirection.X ? transform.transform.position.x : MovingCube.LastCube.transform.position.x;
            float z = moveDirection == MoveDirection.Z ? transform.transform.position.z : MovingCube.LastCube.transform.position.z;

            cube.transform.position = new Vector3
                (
                    x,
                    MovingCube.LastCube.transform.position.y + cubePrefab.transform.localScale.y,
                    z
                );
        }
        else
        {
            cube.transform.position = transform.position;
        }

        cube.MoveDirection = moveDirection;

        Vector3 newCamPos = Camera.main.transform.position + Vector3.up;

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newCamPos, 1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
    }
}
