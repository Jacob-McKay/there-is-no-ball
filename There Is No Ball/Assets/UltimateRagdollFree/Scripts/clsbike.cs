using UnityEngine;
using System.Collections;
using U_r_g_utils;

namespace U_r_g
{
    /// <summary>
    /// 2015-04-01
    /// ULTIMATE RAGDOLL GENERATOR V4.6
    /// © THE ARC GAMES STUDIO 2015
    /// 
    /// Extended class to drive the demo scene bike
    /// NOTE: the dismemberation flow starts with collision detection
    /// </summary>
    public class clsbike : MonoBehaviour
    {
        /// <summary>
        /// maximum torque
        /// </summary>
        public float vargammotormax = 500;
        /// <summary>
        /// simple vehicle with a single shift, will accelerate with maximum allowed torque and reach this value, to stop accelerating further
        /// </summary>
        public float vargamspeedmax = 15;
        /// <summary>
        /// maximum antitorque
        /// </summary>
        public float vargambrakemax = 150;
        /// <summary>
        /// car main rigidbody reference
        /// </summary>
        public Rigidbody vargamchassisbody = null;
        /// <summary>
        /// used as an offset to the bike transform to make sure that raycast down for surface is consistent
        /// </summary>
        public Vector3 vargamraycastfixer = Vector3.zero;
        /// <summary>
        /// used as an offset to the center of mass for unity 4 / 5 compatibility
        /// </summary>
        public Vector3 vargamcomfixer = Vector3.zero;

        private int varwheelscount = 0;
        private WheelCollider[] varwheels;

        private const float cnsspring = 0;
        private const float cnsdamper = 0;
        private const float cnssuspension = 0;

        void Start()
        {
            if (vargamchassisbody == null)
            {
                Debug.LogError("The bike needs a rigidbody to function.");
                enabled = false;
            }
            varwheels = GetComponentsInChildren<WheelCollider>();
            varwheelscount = varwheels.Length;
            varwheels[0].attachedRigidbody.centerOfMass = vargamcomfixer;
            WheelFrictionCurve varcurve = new WheelFrictionCurve();
            varcurve.extremumSlip = 1;
            varcurve.extremumValue = 20000;
            varcurve.asymptoteSlip = 2;
            varcurve.asymptoteValue = 10000;
            varcurve.stiffness = 1;
#if UNITY_5
            if (vargammotormax > 0)
            {
                vargammotormax /= 10;
            }
            varcurve.extremumValue = 2;
            varcurve.asymptoteValue = 1;
#endif
            for (int varwheelcounter = 0; varwheelcounter < varwheelscount; varwheelcounter++)
            {
                JointSpring varspring = new JointSpring();
                varspring.spring = cnsspring;
                varspring.damper = cnsdamper;
                varspring.targetPosition = 0;
                varwheels[varwheelcounter].suspensionSpring = varspring;
                varwheels[varwheelcounter].suspensionDistance = cnssuspension;

                varwheels[varwheelcounter].forwardFriction = varcurve;
                varwheels[varwheelcounter].sidewaysFriction = varcurve;

                //full throttle
                varpower = 0.5f;
                varbrake = 0;
            }
            varspeedmax = vargamspeedmax * vargamspeedmax;
            varD = transform.root.GetComponentInChildren<clsdismemberator>();
        }

        Vector3 varcurrentraycastorigin = Vector3.zero;
        private float varpower, varsteering, varbrake, varspeedmax;
        private bool varpassenger = true;
        private Transform vargampassenger = null;
        void FixedUpdate()
        {
            varcurrentraycastorigin = transform.position + vargamraycastfixer;
            Debug.DrawRay(varcurrentraycastorigin, Vector3.down, Color.yellow);
            if (varpassenger)
            {
                if (!Physics.Raycast(varcurrentraycastorigin, Vector3.down, 1.0f))
                {
                    vargampassenger = transform.Find("Lerpz_kinematic");
                    if (vargampassenger != null)
                    {
                        Invoke("metfalling", 0.3f);
                    }
                    varpassenger = false;
                }
            }

            //toggle pedal when maximum speed is reached
            if (vargamchassisbody.velocity.sqrMagnitude > varspeedmax)
            {
                varpower = 0;
            }

            if (varwheels[0] != null)
            {
                varwheels[0].motorTorque = vargammotormax * varpower;
                varwheels[0].brakeTorque = vargambrakemax * varbrake;
            }
            if (varwheels[1] != null)
            {
                varwheels[1].motorTorque = vargammotormax * varpower;
                varwheels[1].brakeTorque = vargambrakemax * varbrake;
            }

        }

        private clsdismemberator varD;
        private Transform[] varbones;
        private Rigidbody[] varbodies;
        private Vector3 varcurrentforce = Vector3.zero;
        void OnTriggerEnter(Collider varpother)
        {
            //bike crashed!
            if (varpother.tag == "terrain" && this.enabled == true)
            {
                //IMPORTANT STEP: we make sure to execute these operations only -once- otherwise the triggerenter may trigger repeatedly
                this.enabled = false;
                GetComponent<Collider>().enabled = false;

                if (varD != null)
                {
                    //destroy the wheel colliders to disable their autonomous physics (bounce and accelleration)
                    for (int varwheelcounter = 0; varwheelcounter < varwheelscount; varwheelcounter++)
                    {
                        Destroy(varwheels[varwheelcounter]);
                    }
                    CapsuleCollider[] varbikeframecolliders = GetComponents<CapsuleCollider>();
                    foreach (CapsuleCollider varcurrentbikecapsulecolliders in varbikeframecolliders)
                    {
                        varcurrentbikecapsulecolliders.isTrigger = false;
                    }
                    //get all the rigidbodies in the bike. standard iteration simply separates each part
                    // but in the code there are a couple of hints to perform chance based separation, depending on the speed of impact
                    varbodies = GetComponentsInChildren<Rigidbody>();
                    int varparts = varbodies.Length;
                    float varspeedratio = vargamchassisbody.velocity.sqrMagnitude / (vargamspeedmax * vargamspeedmax);
                    int varpartstobreak = varparts; //Mathf.Max((int)(varparts * varspeedratio),1);
                    int varbrokenparts = 0;
                    //iterate over all the calculated parts to break
                    for (int varbreakcounter = 1; varbreakcounter < varparts; varbreakcounter++)
                    {
                        float varbreakchance = 1; //Random.Range(0, 0.99f);
                        if (varbreakchance > (1 - varspeedratio))
                        {
                            //we could use the simple D call:
                            //clsurgutils.metdismember(varbones[varbreakcounter],null,varD);
                            //but we want the detached part transform to apply force to it, and so we call metdismemberpart
                            Transform varcurrentpart = clsurgutils.metdismemberpart(varbodies[varbreakcounter].transform, null, varD);
                            //we've retrieved the current D part, so we apply a 'bounce' force to it
                            if (varcurrentpart != null)
                            {
                                Rigidbody varpartbody = varcurrentpart.GetComponent<Rigidbody>();
                                varcurrentforce = Random.insideUnitCircle * varspeedratio;
                                if (varcurrentforce.y < 0)
                                {
                                    varcurrentforce.y *= -1;
                                }
                                varpartbody.AddForce(varcurrentforce * varbodies[varbreakcounter].mass, ForceMode.Impulse);
                            }
                            varbrokenparts++;
                        }
                        if (varbrokenparts > varpartstobreak)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void metfalling()
        {
            vargampassenger.parent = null;
            clsurgutils.metgotangible(vargampassenger, true);
            clsurgutils.metgodriven(vargampassenger, GetComponent<Rigidbody>().velocity);
        }

        public void metactivate()
        {
            enabled = true;
        }
    }
}