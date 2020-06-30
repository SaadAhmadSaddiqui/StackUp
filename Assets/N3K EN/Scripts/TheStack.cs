using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Unity.Collections;

public class TheStack : MonoBehaviour
{
    

    public Color32[] gameColors = new Color32[10];
    public Material stackMat;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;

    private const float bound_Size = 3.5f;
    private const float stack_Moving_Speed = 5.0f;
    private const float error_Margin = .25f;
    private const float stack_Bounds_Gain = 0.25f;
    private const float combo_Start_Gain = 3;

    private GameObject[] theStack;
    private Vector2 stackBounds = new Vector2(bound_Size, bound_Size);

    [HideInInspector]
    public int scoreCount = 0;
    private int stackIndex;
    private int combo = 0;

    private float tileTransition = 0.0f;
    private float tileSpeed = 2.5f;
    private float secondaryPosition;

    private bool isMovingOnX = true;
    private bool gameOver = false;

    private Vector3 desiredPosition;
    private Vector3 lastTilePosition;
    private float colorTransition;
    private Color startColor;
    private Color endColor;
    private int lastColorIndex;

    public float time;

    private float H;
    private float S;
    private float V;

    //Sound Effects
    public AudioClip[] clips; // add the sound effect clips that you want to play randomly
    private AudioSource ausrc;




    private void Awake()
    {
        ausrc = GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gameColors.Length; i++)
        {
            gameColors[i] = Random.ColorHSV();
        }

        theStack = new GameObject[transform.childCount];
        startColor = gameColors[0];
        endColor = gameColors[1];
        lastColorIndex = 1;
        for (int i = 0; i < transform.childCount; i++)
        {
            theStack[i] = transform.GetChild(i).gameObject;
            ColorMesh(theStack[i].GetComponent<MeshFilter>().mesh);
        }
        stackIndex = transform.childCount - 1;
   
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceTile())
            {
                PlaySound();
                SpawnTile();
                scoreCount++;
                scoreText.text = scoreCount.ToString();
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

    private void CreateRubble(Vector3 pos, Vector3 scale)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.AddComponent<Rigidbody>();

        go.GetComponent<MeshRenderer>().material = stackMat;
        ColorMesh(go.GetComponent<MeshFilter>().mesh);

    }

    private void MoveTile()
    {
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
        theStack[stackIndex].transform.localScale = (new Vector3(stackBounds.x, 1, stackBounds.y));

        ColorMesh(theStack[stackIndex].GetComponent<MeshFilter>().mesh);
    }

    private void ColorMesh(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Color32[] colors = new Color32[vertices.Length];
        colorTransition += 0.1f;
        if (colorTransition > 1)
        {
            colorTransition = 0.0f;
            startColor = endColor;
            int ci = lastColorIndex;
            while (ci == lastColorIndex)
                ci = Random.Range(0, gameColors.Length);
            endColor = gameColors[ci];
        }
        Color c = Color.Lerp(startColor, endColor, colorTransition);

        for (int i = 0; i < vertices.Length; i++)
            colors[i] = c;

        mesh.colors32 = colors;
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
                CreateRubble
                    (
                        new Vector3
                        (
                            (t.position.x > 0) ? t.position.x + (t.localScale.x / 2) : t.position.x - (t.localScale.x / 2),
                            t.position.y,
                            t.position.z
                        ),
                        new Vector3(Mathf.Abs(deltaX), 1, t.localScale.z)
                    );
                t.localPosition = new Vector3(middle - (lastTilePosition.x / 2), scoreCount, lastTilePosition.z);
            }
            else
            {
                if (combo > combo_Start_Gain)
                {
                    stackBounds.x += stack_Bounds_Gain;
                    if (stackBounds.x > bound_Size)
                    {
                        stackBounds.x = bound_Size;
                    }
                    float middle = lastTilePosition.x + t.localPosition.x / 2;
                    t.localScale = (new Vector3(stackBounds.x, 1, stackBounds.y));
                    t.localPosition = new Vector3(middle - (lastTilePosition.x / 2), scoreCount, lastTilePosition.z);
                }
                combo++;
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, lastTilePosition.z);
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
                CreateRubble
                    (
                        new Vector3
                        (
                            t.position.x,
                            t.position.y,
                            (t.position.z > 0) ? t.position.z + (t.localScale.z / 2) : t.position.z - (t.localScale.z / 2)
                        ),
                        new Vector3(t.localScale.x, 1, Mathf.Abs(deltaZ))
                    );
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - (lastTilePosition.z / 2));
            }
            else
            {
                if (combo > combo_Start_Gain)
                {
                    stackBounds.y += stack_Bounds_Gain;
                    if (stackBounds.y > bound_Size)
                    {
                        stackBounds.y = bound_Size;
                    }
                    float middle = lastTilePosition.z + t.localPosition.z / 2;
                    t.localScale = (new Vector3(stackBounds.x, 1, stackBounds.y));
                    t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - (lastTilePosition.z / 2));
                }
                combo++;
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, lastTilePosition.z);
            }
        }

        secondaryPosition = (isMovingOnX) ? t.localPosition.x : t.localPosition.z;

        isMovingOnX = !isMovingOnX;

        return true;
    }

    private void EndGame()
    {
        if (scoreCount > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", scoreCount);
        }
        gameOver = true;
        theStack[stackIndex].AddComponent<Rigidbody>();
        gameOverPanel.SetActive(true);
    }

    public void OnButtonClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PlaySound()
    {
        ausrc.clip = clips[Random.Range(0, clips.Length)];
        ausrc.Play();
    }


}
