using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ProceduralFoodObject : FoodObject
{
    private SphereCollider _sphereCollider;

    [SerializeField] public bool UseFixedRadius = true; //true = rescale mesh, false = scale collider
    [SerializeField] public float FixedColliderRadius = 0.1f;
    [SerializeField, Range(0.0f, 22.6f)] public float Density = 1.0f; // g/ml (water is 1.0, osmium is 22.6)

    private const float k_radiusToMass = 1000.0f * 4.0f / 3.0f * Mathf.PI;
    private const float k_defaultDragCoefficient = 0.47f; //0.47 is a sphere

    protected override void Start()
    {
        base.Start();

        //get sphere collider
        _sphereCollider = GetComponent<SphereCollider>();

        if (UseFixedRadius)
        {
            RescaleMesh();
        }
        else
        {
            GenerateCollider();
        }
        GeneratePhysicalProperties();
    }

    private void RescaleMesh()
    {
        float scale = FixedColliderRadius / ObjectCoreRadius;

        //rescale object
        transform.localScale = new Vector3(scale, scale, scale);

        //set collider radius to work with new scale
        _sphereCollider.radius = FixedColliderRadius / scale;
    }

    private void GenerateCollider()
    {
        //set collider radius
        _sphereCollider.radius = (_meshExtents.x + _meshExtents.y + _meshExtents.z) / 3.0f;
    }

    private void GeneratePhysicalProperties()
    {
        //set mass if based on mesh (assuming ellipsiod shape and density of water)
        _rigidbody.mass = Density * transform.localScale.x * transform.localScale.y * transform.localScale.z * k_radiusToMass * _meshExtents.x * _meshExtents.y * _meshExtents.z;

        //set drag coefficient (as a sphere)
        _rigidbody.drag = k_defaultDragCoefficient;
    }

    public void ChangeMesh(Mesh mesh)
    {
        GetComponent<MeshFilter>().mesh = mesh;

        if (UseFixedRadius)
        {
            RescaleMesh();
        }
        else
        {
            GenerateCollider();
        }
        GeneratePhysicalProperties();
    }

    public void ChangeMesh(Mesh mesh, bool useFixedRadius)
    {
        GetComponent<MeshFilter>().mesh = mesh;
        UseFixedRadius = useFixedRadius;

        if (UseFixedRadius)
        {
            RescaleMesh();
        }
        else
        {
            GenerateCollider();
        }
        GeneratePhysicalProperties();
    }

    public void ChangeMesh(Mesh mesh, float fixedRadius)
    {
        GetComponent<MeshFilter>().mesh = mesh;
        FixedColliderRadius = fixedRadius;

        if (UseFixedRadius)
        {
            RescaleMesh();
        }
        else
        {
            GenerateCollider();
        }
        GeneratePhysicalProperties();
    }

    public void ChangeMesh(Mesh mesh, float fixedRadius, bool useFixedRadius)
    {
        GetComponent<MeshFilter>().mesh = mesh;
        FixedColliderRadius = fixedRadius;
        UseFixedRadius = useFixedRadius;

        if (UseFixedRadius)
        {
            RescaleMesh();
        }
        else
        {
            GenerateCollider();
        }
        GeneratePhysicalProperties();
    }

    public void ChangeMesh(Mesh mesh, float fixedRadius, float density)
    {
        GetComponent<MeshFilter>().mesh = mesh;
        FixedColliderRadius = fixedRadius;
        Density = density;

        if (UseFixedRadius)
        {
            RescaleMesh();
        }
        else
        {
            GenerateCollider();
        }
        GeneratePhysicalProperties();
    }

    public void ChangeMesh(Mesh mesh, float fixedRadius, float density, bool useFixedRadius)
    {
        GetComponent<MeshFilter>().mesh = mesh;
        FixedColliderRadius = fixedRadius;
        Density = density;
        UseFixedRadius = useFixedRadius;

        if (UseFixedRadius)
        {
            RescaleMesh();
        }
        else
        {
            GenerateCollider();
        }
        GeneratePhysicalProperties();
    }
}