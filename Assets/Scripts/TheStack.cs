using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour
{
    private GameObject[] theStack;
    private int scoreCount = 0;
    private int stackIndex;

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
            PlaceTile();
            scoreCount++;
        }
    }

    private void PlaceTile()
    {
        theStack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0);
    }
}
