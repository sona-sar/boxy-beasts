using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isMoving = false;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gridMoveDistance = 2f;

    AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.FindWithTag("AudioManager")?.GetComponent<AudioManager>();
    
        if (CharacterManager.instance != null && !string.IsNullOrEmpty(CharacterManager.instance.selectedCharacterName))
        {
            GameObject prefab = Resources.Load<GameObject>("Players/" + CharacterManager.instance.selectedCharacterName);
            if (prefab != null)
            {
                Destroy(transform.GetChild(0).gameObject);
                GameObject character = Instantiate(prefab, transform);
                character.transform.localPosition = Vector3.zero;
            }

            string goalName = CharacterManager.instance.GetGoalNameForSelectedCharacter();
            if (!string.IsNullOrEmpty(goalName))
            {
                GameObject goalPrefab = Resources.Load<GameObject>("Goals/" + goalName);
                if (goalPrefab != null)
                {
                    GameObject[] existingGoals = GameObject.FindGameObjectsWithTag("Goal");
                    foreach (GameObject oldGoal in existingGoals)
                    {
                        Vector3 goalPosition = oldGoal.transform.position;
                        Quaternion goalRotation = oldGoal.transform.rotation;
                        Destroy(oldGoal);
                        GameObject newGoal = Instantiate(goalPrefab, goalPosition, goalRotation);
                        newGoal.tag = "Goal";
                    }

                }
            }
        }
    }

    void Update()
    {
        if(isMoving) return;
        var movementVector = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movementVector = Vector3.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movementVector = Vector3.back;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movementVector = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movementVector = Vector3.right;
        }
        
        if(movementVector != Vector3.zero)
        {
            TryToMove(movementVector);
        }
    }

    private void TryToMove(Vector3 direction)
    {
        transform.forward = direction;
        
        Vector3 normalizedDirection = direction.normalized;
        Vector3 scaledDirection = normalizedDirection * gridMoveDistance;
        Vector3 targetPosition = transform.position + scaledDirection;

        if(!Physics.Raycast(transform.position, direction, out RaycastHit hit, gridMoveDistance + 0.1f, blockLayer))
        {
            StartCoroutine(MoveToTarget(targetPosition));
        }
        else if(hit.collider.CompareTag("Box"))
        {
            var box = hit.collider.GetComponent<BoxController>();
            if(box != null && box.TryToPushBox(direction, moveSpeed))
            {
                StartCoroutine(MoveToTarget(targetPosition));
            }
        }
    }
    
    private IEnumerator MoveToTarget(Vector3 target)
    {
        isMoving = true;
        if (audioManager != null)
        {
            audioManager.PlayMoveSound();
        }
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
            yield return null;
        }
        transform.position = target;
        isMoving = false;
    }
}
