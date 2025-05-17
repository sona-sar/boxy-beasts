using System.Collections;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    private bool isMoving = false;
    private float goalCount = 0f;
    public bool OnGoal => goalCount > 0;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private float gridMoveDistance = 2f;

    private void Start()
    {
        GameManager.instance.RegisterBox(this);
    }

    private void Update()
    {
        if (!isMoving && GameManager.instance != null)
        {
            GameManager.instance.UpdateBoxMaterial(this, OnGoal);
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.UnregisterBox(this);
    }

    public bool TryToPushBox(Vector3 direction, float moveSpeed)
    {
        if(isMoving) return false;
        Vector3 normalizedDirection = direction.normalized;
        Vector3 scaledDirection = normalizedDirection * gridMoveDistance;
        Vector3 targetPosition = transform.position + scaledDirection;
        if(!Physics.Raycast(transform.position, direction, out RaycastHit hit, gridMoveDistance + 0.1f, blockLayer))
        {
            StartCoroutine(MoveToTarget(targetPosition, moveSpeed));
            return true;
        }
        return false;
    }
    
    private IEnumerator MoveToTarget(Vector3 target, float moveSpeed)
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
            yield return null;
        }
        transform.position = target;
        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            goalCount++;
            Debug.Log($"Entered goal: {other.name}");
            
            Renderer goalRenderer = other.GetComponent<Renderer>();
            if (goalRenderer != null)
            {
                goalRenderer.enabled = false;
            }
            
            Renderer[] childRenderers = other.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in childRenderers)
            {
                renderer.enabled = false;
            }
            
            if (GameManager.instance != null)
            {
                GameManager.instance.CheckWinCondition();
                GameManager.instance.UpdateBoxMaterial(this, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            goalCount--;
            Debug.Log($"Exited goal: {other.name}");
            
            Renderer goalRenderer = other.GetComponent<Renderer>();
            if (goalRenderer != null)
            {
                goalRenderer.enabled = true;
                Debug.Log("Enabled main renderer");
            }
            
            Renderer[] childRenderers = other.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in childRenderers)
            {
                renderer.enabled = true;
                Debug.Log($"Enabled child renderer on {renderer.gameObject.name}");
            }
            
            if (goalCount <= 0)
            {
                goalCount = 0;
                if (GameManager.instance != null)
                {
                    GameManager.instance.UpdateBoxMaterial(this, false);
                }
            }
        }
    }
}
