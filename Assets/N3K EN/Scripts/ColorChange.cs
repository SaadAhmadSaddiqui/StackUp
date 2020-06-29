using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    [SerializeField]
    Gradient gradient;
    [SerializeField]
    float duration;
    float t = 0f;


    // Start is called before the first frame update
    void Start()
    {
        var colorkeys = new GradientColorKey[2];
        var alphakeys = new GradientAlphaKey[2];

        colorkeys[0].color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        colorkeys[0].time = 0f;
        colorkeys[1].color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        colorkeys[1].time = 1f;

        alphakeys[0].alpha = 1f;
        alphakeys[0].time = 0f;
        alphakeys[1].alpha = 1f;
        alphakeys[1].time = 1f;

        gradient.SetKeys(colorkeys, alphakeys);
    }

    // Update is called once per frame
    void Update()
    {
        float value = Mathf.Lerp(0f, 1f, t);
        t += Time.deltaTime / duration;
        Color color = gradient.Evaluate(value);
        Camera.main.backgroundColor = color;
    }
}
