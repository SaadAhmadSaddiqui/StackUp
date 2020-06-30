using UnityEngine;

public class GradientBackground : MonoBehaviour
{

    public Color topColor;
    public Color bottomColor = Color.white;
    public int gradientLayer = 7;
    public Shader shader;
    public float time;

    private float H;
    private float S;
    private float V;
    private Mesh mesh;
    GameObject gradientPlane;
    void Awake()
    {
        H = Random.Range(0.5f, 1f);
        S = Random.Range(0.5f, 1f);
        V = Random.Range(0.5f, 1f);
        time = Random.Range(1f, 255f);
        topColor = Random.ColorHSV();
        gradientLayer = Mathf.Clamp(gradientLayer, 0, 31);

        GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
        GetComponent<Camera>().cullingMask = GetComponent<Camera>().cullingMask & ~(1 << gradientLayer);
        Camera gradientCam = new GameObject("Gradient Cam", typeof(Camera)).GetComponent<Camera>();
        gradientCam.depth = GetComponent<Camera>().depth - 1;
        gradientCam.cullingMask = 1 << gradientLayer;

        mesh = new Mesh();
        mesh.vertices = new Vector3[4]
        {new Vector3(-100f, .577f, 1f), new Vector3(100f, .577f, 1f), new Vector3(-100f, -.577f, 1f), new Vector3(100f, -.577f, 1f)};

        mesh.colors = new Color[4] { topColor, topColor, bottomColor, bottomColor };

        mesh.triangles = new int[6] { 0, 1, 2, 1, 3, 2 };

        Material mat = new Material(shader);
        gradientPlane = new GameObject("Gradient Plane", typeof(MeshFilter), typeof(MeshRenderer));

        ((MeshFilter)gradientPlane.GetComponent(typeof(MeshFilter))).mesh = mesh;
        gradientPlane.GetComponent<MeshRenderer>().material = mat;
        gradientPlane.layer = gradientLayer;
    }

    private void Update()
    {
        time += Time.deltaTime;
        topColor = Color.HSVToRGB((time / 100f) % H, S, V);
        mesh.colors = new Color[4] { topColor, topColor, bottomColor, bottomColor };
        ((MeshFilter)gradientPlane.GetComponent(typeof(MeshFilter))).mesh = mesh;
    }
}