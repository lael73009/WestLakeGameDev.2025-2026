using Unity.Mathematics;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Rigidbody2D rb;
    private float pickUpRadius = 1.0f;
    private interactable heldItem;

    // Do something with this at some point... maybe when tilemap is more complete
    public LayerMask detectionLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && DetectObject())
        {
            Debug.Log("Interacted");
        }
    }

    /*public void Interactt()
    {
        interactable.Interact();
    }*/

    bool DetectObject()
    {
        return Physics2D.OverlapCircle(rb.position, pickUpRadius, detectionLayer);
    }

    private void onDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(rb.position, pickUpRadius);
    }

    /*void FindClosestInteractable()
    {
        // Get all colliders in range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickUpRadius);
        
        float minDistanceSqr = float.MaxValue;
        closestInteractable = null;

        foreach (var hitCollider in hitColliders)
        {
            // Try to get the IInteractable component
            IInteractable interactable = hitCollider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                // Calculate distance using sqrMagnitude for performance
                float distanceSqr = (hitCollider.transform.position - transform.position).sqrMagnitude;

                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    closestInteractable = interactable;
                }
            }
        }
    }*/    
}