using System.Collections;
using System.Collections.Generic;
using System.Net;
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
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, rayMask)) 
        {
            Debug.Log("Hit highlightable object");
            highlightObject = hit.transform.gameObject;
            MeshRenderer mr = highlightObject.GetComponent<MeshRenderer>();

            List<Material> materialList = new List<Material>();

            if (mr.materials.Length == 1)
            {
                if (!highlightMaterial) return;
                
                mr.GetMaterials(materialList);
                
                materialList.Add(highlightMaterial);
                mr.SetMaterials(materialList);
                mr.materials[1].SetFloat("_OutlineBlend", 1.0f);
            }
        }
    }
}
