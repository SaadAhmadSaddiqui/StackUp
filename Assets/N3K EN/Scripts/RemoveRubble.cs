using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RemoveRubble : MonoBehaviour
{
    public TheStack TheStack;

    public float time;

    private float H;
    private float S;
    private float V;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }

    private void Start()
    {
        H = Random.Range(0.5f, 1f);
        S = Random.Range(0.5f, 1f);
        V = Random.Range(0.5f, 1f);
    }

    private void Update()
    {
        time += Time.deltaTime * 10;
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB((time / 100f) % H, S, V));
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
