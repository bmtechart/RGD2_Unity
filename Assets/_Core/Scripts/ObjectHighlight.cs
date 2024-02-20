using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectHighlight : MonoBehaviour
{
    public LayerMask rayMask;
    public GameObject highlightObject;
    public Material highlightMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastForHighlight();
    }

    private void RaycastForHighlight()
    {
        RaycastHit hit;
        bool hitSuccessful = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, rayMask);

        if (!hitSuccessful)
        {
            if (highlightObject)
            {
                RemoveHighlight(highlightObject);
                highlightObject = null;
                return;
            }

            if (!highlightObject) return;
        }

        if (hitSuccessful)
        {
            GameObject newHighlightObject = hit.transform.gameObject;
            if (highlightObject)
            {
                //if looking at an already highlighted object
                if (highlightObject == newHighlightObject) return;

                //if looking at a new highlighted object
                if (highlightObject != newHighlightObject)
                {
                    RemoveHighlight(highlightObject);
                    AddHighlight(newHighlightObject);
                    highlightObject = newHighlightObject;
                }
            }

            if (!highlightObject)
            {
                highlightObject = newHighlightObject;
                AddHighlight(highlightObject);
            }
        }
    }

    private void AddHighlight(GameObject obj)
    {
        Debug.Log("Highlight added!");
        List<Material> mats = new List<Material>();
        obj.GetComponent<MeshRenderer>().GetMaterials(mats);
        mats.Add(highlightMaterial);
        obj.GetComponent<MeshRenderer>().SetMaterials(mats);
    }

    private void RemoveHighlight(GameObject obj)
    {
        Debug.Log("Highlight Removed");
        List<Material> mats = new List<Material>();
        obj.GetComponent<MeshRenderer>().GetMaterials(mats);
        mats.RemoveAt(1);
        obj.GetComponent<MeshRenderer>().SetMaterials(mats);
    }
}
