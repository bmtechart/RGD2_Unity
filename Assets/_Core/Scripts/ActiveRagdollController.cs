using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ActiveRagdoll
{
    public class ActiveRagdollController : MonoBehaviour
    {
        ConfigurableJoint[] _joints;
        Transform[] _animatedBones;
        Quaternion[] _initialJointRotations;

        [SerializeField]
        GameObject Head;
        [SerializeField]
        GameObject HeadForceTarget;

        [SerializeField]
        float HeadUpForce = 1.0f;

        [Header("Active Ragdoll Body")]
        [SerializeField] GameObject RagdollBody;

        [Header("Target Body")]
        [SerializeField] GameObject TargetAnimationBody;

        // Start is called before the first frame update
        void Start()
        {
            //generate list of joints
            _joints = RagdollBody?.GetComponentsInChildren<ConfigurableJoint>();

            _initialJointRotations = new Quaternion[_joints.Length];
            for(int i = 0; i < _joints.Length; i++)
            {
                _initialJointRotations[i] = _joints[i].transform.localRotation;
            }

            _animatedBones = new Transform[_joints.Length];

            for (int i = 0; i < _joints.Length; i++)
            {
                foreach(Transform child in TargetAnimationBody.GetComponentsInChildren<Transform>())
                {
                    if(child.gameObject.name == _joints[i].gameObject.name)
                    {
                        _animatedBones[i] = child;
                    }
                }
            }


            //get head joint
        }

        // Update is called once per frame
        void Update()
        {
            if (!Head) return;
            Head.GetComponent<Rigidbody>().AddForce((HeadForceTarget.transform.position - Head.transform.position) * HeadUpForce);

            for(int i=0; i < _joints.Length; i++)
            {
                ConfigurableJointExtensions.SetTargetRotationLocal(_joints[i], _animatedBones[i].localRotation, _initialJointRotations[i]);   
            }
        }
    }
}

