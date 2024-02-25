using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    protected Rigidbody _rigidbody;
    protected Collider _collider;

    [SerializeField] protected GameObject collisionParticle;
    public bool isThrown = false;


    protected virtual void Start()
    {
        //get rigidbody
        _rigidbody = GetComponent<Rigidbody>();

        //get collider
        _collider = GetComponent<Collider>();
    }

    //pick up object
    public void HoldObject()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _collider.enabled = false;
    }

    //drop object
    public void DropObject()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.constraints = RigidbodyConstraints.None;
        _collider.enabled = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!isThrown) return;

        //spawn particle at collision point
        if(collisionParticle)
        {
            GameObject newParticle = Instantiate(collisionParticle);
            newParticle.transform.position = collision.contacts[0].point;
        }

        IDamageable isDamageable = collision.gameObject.GetComponent<IDamageable>();
        if(isDamageable != null)
        {
            isDamageable.Damage(100.0f);
        }
        isThrown = false;
    }
}