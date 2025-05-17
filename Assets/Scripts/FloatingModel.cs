using UnityEngine;

public class FloatingModel : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float floatAmplitude = 0.25f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        foreach (var renderer in GetComponentsInChildren<Renderer>(true)) 
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = startPos + new Vector3(0f, offsetY, 0f);
    }
}
