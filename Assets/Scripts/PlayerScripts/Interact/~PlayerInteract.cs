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
    private Rigidbody2D _rb;
    private float _pickUpRadius = 1.0f;
    private interactable _heldItem;
    private GameObject[] _inventory;

    // TO DO: MAKE SPECIFIC LAYER
    public LayerMask _detectionLayer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _inventory = new GameObject[10];
    }

    void Update()
    {
        // Find object, iteract if E pressed
        GameObject detectedObject = DetectClosestObject()?.gameObject;
        if (Input.GetKeyDown(KeyCode.E) && detectedObject && detectedObject)
        {
            Debug.Log("Interacted");
            _inventory[FindFirstFreeInventorySpot()] = detectedObject;
            if (_inventory != null) { Debug.Log(arrayToString(_inventory)); }
            detectedObject.SetActive(false);
        }
    }

    /*public void Interactt()
    {
        interactable.Interact();
    }*/

    Collider2D DetectObject()
    {
        return Physics2D.OverlapCircle(_rb.position, _pickUpRadius, _detectionLayer);
    }

    private Collider2D DetectClosestObject()
    {
        //Debug.Log("Detecting...");

        // Get all colliders in range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(_rb.position, _pickUpRadius, _detectionLayer);
        
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
        for (int i=0; i<_inventory.Length; i++)
        {
            if (_inventory[i] == null)
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