using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework.Constraints;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

// No onlger used
public class PlayerInteract : MonoBehaviour
{
    private Rigidbody2D rb;
    private float pickUpRadius = 1.0f;
    private interactable heldItem;
    private GameObject[] inventory;

    // TO DO: MAKE SPECIFIC LAYER
    public LayerMask detectionLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inventory = new GameObject[10];
    }

    void Update()
    {
        // Find object, iteract if E pressed
        GameObject detectedObject = DetectClosestObject()?.gameObject;
        if (Input.GetKeyDown(KeyCode.E) && detectedObject && detectedObject)
        {
            Debug.Log("Interacted");
            inventory[FindFirstFreeInventorySpot()] = detectedObject;
            if (inventory != null) { Debug.Log(arrayToString(inventory)); }
            detectedObject.SetActive(false);
        }
    }

    /*public void Interactt()
    {
        interactable.Interact();
    }*/

    Collider2D DetectObject()
    {
        return Physics2D.OverlapCircle(rb.position, pickUpRadius, detectionLayer);
    }

    private Collider2D DetectClosestObject()
    {
        //Debug.Log("Detecting...");

        // Get all colliders in range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(rb.position, pickUpRadius, detectionLayer);
        
        float minDistanceSqr = float.MaxValue;
        Collider2D closestObject = null;

        foreach (var hitCollider in hitColliders)
        {
            //Debug.Log("Still detecting...");
            // Try to get the IInteractable component
            if (hitCollider)
            {
                // Calculate distance using sqrMagnitude for performance
                float distanceSqr = (hitCollider.transform.position - transform.position).sqrMagnitude;

                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    closestObject = hitCollider;
                }
                //Debug.Log("Detected!");
            }
        }

        return closestObject;
    }

    private int FindFirstFreeInventorySpot()
    {
        // Loop through index to find a free spot
        int index = -1;
        for (int i=0; i<inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    private string arrayToString(GameObject[] array)
    {
        // Print array for debyg purposes
        string output = "";
        foreach (var item in array) {
            if (item) {
                output += item.ToString() + ", ";
            }
        }

        return output;
    }
}