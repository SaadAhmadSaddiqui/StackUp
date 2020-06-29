using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMGR : MonoBehaviour
{
    public GameObject currentCube;
    public GameObject lastCube;
    public TextMeshProUGUI scoreText;

    public int level;
    public bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        newBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            return;
        }

        var time = Mathf.Abs(Time.realtimeSinceStartup % 2f - 1f);

        Vector3 pos1 = lastCube.transform.position + Vector3.up * 10f;
        Vector3 pos2 = pos1 + ((level % 2 == 0) ? Vector3.left : Vector3.forward) * 120f;

        if ((level % 2 == 0))
        {
            currentCube.transform.position = Vector3.Lerp(pos2, pos1, time);
        }
        else
        {
            currentCube.transform.position = Vector3.Lerp(pos1, pos2, time);
        }

        if (Input.GetMouseButtonDown(0))
        {
            newBlock();
        }
    }

    private void newBlock()
    {
        if (lastCube != null)
        {
            currentCube.transform.position = new Vector3
                (
                    Mathf.Round(currentCube.transform.position.x),
                    Mathf.Round(currentCube.transform.position.y),
                    Mathf.Round(currentCube.transform.position.z)
                 );
            currentCube.transform.localScale = new Vector3
                (
                    lastCube.transform.localScale.x - Mathf.Abs(currentCube.transform.position.x - lastCube.transform.position.x),
                    lastCube.transform.localScale.y,
                    lastCube.transform.localScale.z - Mathf.Abs(currentCube.transform.position.z - lastCube.transform.position.z)
                );

            currentCube.transform.position = Vector3.Lerp(currentCube.transform.position, lastCube.transform.position, 0.5f) + Vector3.up * 5f;

            if (currentCube.transform.localScale.x <= 0f || currentCube.transform.localScale.z <= 0f)
            {
                gameOver = true;
                scoreText.gameObject.SetActive(true);
                scoreText.text = "Score: " + level;
                StartCoroutine(x());
                return;
            }
        }
        lastCube = currentCube;
        currentCube = Instantiate(lastCube);
        currentCube.name = level + "";
        currentCube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB((level / 100f) % 1f, 1f, 1f));
        level++;
        Camera.main.transform.position = currentCube.transform.position + new Vector3(100f, 100f, 100f);
        Camera.main.transform.LookAt(currentCube.transform.position);
    }

    IEnumerator x()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("TheStack");
    }
}
