using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

namespace ActiveRagdoll
{
    public class ActiveRagdollController : MonoBehaviour
    {
        ConfigurableJoint[] _joints;
        Transform[] _animatedBones;
        Quaternion[] _initialJointRotations;

        [Header("Aniamted Body")]
        [SerializeField] GameObject AnimatedBody;

        [Header("Active Ragdoll Body")]
        [SerializeField] GameObject RagdollBody;

        [SerializeField]
        GameObject Head;
        [SerializeField]
        GameObject HeadForceTarget;

        public CurveField curvecurveboi;

        [SerializeField]
        float HeadUpForce = 1.0f;

        [SerializeField] private bool _activeRagdollEnabled;

        public bool ActiveRagdollEnabled
        {
            get 
            { 
                return _activeRagdollEnabled; 
            }
            set
            {
                _activeRagdollEnabled = value;
                if(value)
                {
                    TargetAnimationBody.GetComponent<Animator>().enabled = true;
                }

                if(!value)
                {
                    AnimatedBody.GetComponentInParent<Animator>().enabled = false;
                    foreach (ConfigurableJoint cg in GetComponentsInChildren<ConfigurableJoint>())
                    {
                        JointDrive drive = cg.slerpDrive;
                        drive.positionSpring = 0.0f;
                        cg.slerpDrive = drive;

                        JointDrive xDrive = cg.xDrive;
                        JointDrive yDrive = cg.yDrive;
                        JointDrive zDrive = cg.zDrive;

                        xDrive.positionSpring = 0.0f;
                        yDrive.positionSpring = 0.0f;
                        zDrive.positionSpring = 0.0f;

                        cg.xDrive = xDrive;
                        cg.yDrive = yDrive;
                        cg.zDrive = zDrive;
                    }
                }
            }
        }

        [Header("Target Body")]
        [SerializeField] GameObject TargetAnimationBody;

        // Start is called before the first frame update
        void Start()
        {
            
            LinkRagdollToAnimatedSkeleton();
            LinkHipToReference();
            //get head joint
        }

        // Update is called once per frame
        void Update()
        {
            if (_activeRagdollEnabled)
            {
                UpdateJointRotationTargets();
                UpdateHeadForce();
            }
        }

        #region Ragdoll
        private void LinkRagdollToAnimatedSkeleton()
        {
            //generate list of joints
            _joints = RagdollBody?.GetComponentsInChildren<ConfigurableJoint>();


            _initialJointRotations = new Quaternion[_joints.Length];
            
            for (int i = 0; i < _joints.Length; i++)
            {
                _initialJointRotations[i] = _joints[i].transform.localRotation;
            }

            _animatedBones = new Transform[_joints.Length];
            for (int i = 0; i < _joints.Length; i++)
            {
                foreach(Transform t in AnimatedBody?.GetComponentsInChildren<Transform>())
                {
                    if (t.gameObject.name == _joints[i].gameObject.name)
                    {
                        _animatedBones[i] = t;
                    }
                }
            }

            
        }

        private void LinkHipToReference()
        {
            ConfigurableJoint hipJoint = RagdollBody?.AddComponent<ConfigurableJoint>();
            
            JointDrive xDrive = hipJoint.xDrive;
            xDrive.positionSpring = 50000.0f;

            JointDrive yDrive = hipJoint.yDrive;
            yDrive.positionSpring = 50000.0f;

            JointDrive zDrive = hipJoint.zDrive;
            zDrive.positionSpring = 100000.0f;

            hipJoint.xDrive = xDrive;
            hipJoint.yDrive = yDrive;
            hipJoint.zDrive = zDrive;

            hipJoint.rotationDriveMode = RotationDriveMode.Slerp;

            JointDrive slerpDrive = hipJoint.slerpDrive;
            slerpDrive.positionSpring = 50000.0f;
            hipJoint.slerpDrive = slerpDrive;

            hipJoint.connectedBody = AnimatedBody.GetComponent<Rigidbody>();
        }

        private void UpdateHeadForce()
        {
            if (!Head) return;
            Head.GetComponent<Rigidbody>().AddForce((HeadForceTarget.transform.position - Head.transform.position) * HeadUpForce);
        }

        private void UpdateJointRotationTargets()
        {
            for (int i = 0; i < _joints.Length; i++)
            {
                ConfigurableJointExtensions.SetTargetRotationLocal(_joints[i], _animatedBones[i].localRotation, _initialJointRotations[i]);
            }
        }

        #endregion
    }

    //when ragdoll gets hit, disable active ragdoll
    
}

