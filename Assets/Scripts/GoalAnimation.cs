using UnityEngine;

public class GoalAnimation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 45f; 
    [SerializeField] private float floatSpeed = 2.5f;
    [SerializeField] private float floatAmplitude = 0.25f; 

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
