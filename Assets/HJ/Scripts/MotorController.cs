using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    public class MotorController : MonoBehaviour
    {
        [SerializeField]
        private bool testMotorOn;

        private HingeJoint hinge;

        private void Awake()
        {
            hinge = GetComponent<HingeJoint>();
        }

        private void Update()
        {
            if (testMotorOn) 
            { 
                OperateMotor(); 
            }
        }

        public void OperateMotor()
        {
            hinge.useMotor = true;
        }

        public void StopMotor()
        {
            hinge.useMotor = false;
        }
    }
}

