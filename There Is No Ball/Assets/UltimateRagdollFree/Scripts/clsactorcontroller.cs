using UnityEngine;
using U_r_g_utils;

namespace U_r_g
{
    /// <summary>
    /// 2015-04-01
    /// ULTIMATE RAGDOLL GENERATOR V4.6
    /// © THE ARC GAMES STUDIO 2015
    /// 
    /// helper class to move a character with a character controller. includes advanced ragdoll turning function on free fall
    /// and scene manager activation
    /// </summary>
    public class clsactorcontroller : MonoBehaviour
    {
        public float vargamspeed = 5;
        public float vargamrotspeed = 10;
        public float vargravity = 20;
        private CharacterController varcontroller;
        private Vector3 varmovement;
        private float varfallstarttime;
        private float vartimer;
        private clsragdollhelper varhelper = null;
        private Ray vargravityray = new Ray();
        private bool varfalling = false;
        private bool varfallen = false;

        void Awake()
        {
            varcontroller = GetComponent<CharacterController>();
            varhelper = GetComponent<clsragdollhelper>();
            vargravityray = new Ray(transform.position, Vector3.down);
        }

        void LateUpdate()
        {
            if (varcontroller != null)
            {
                transform.Rotate(Vector3.up * (vargamrotspeed * Time.deltaTime * Time.timeScale));
                varmovement = transform.forward * (vargamspeed * Time.deltaTime * Time.timeScale);

                if (!varcontroller.isGrounded)
                {
                    //Debug.DrawRay(transform.position,  (varcontroller.velocity + vargravityray.direction).normalized, Color.red);
                    if (!Physics.Raycast(transform.position, (varcontroller.velocity + vargravityray.direction).normalized, 1.0f))
                    {
                        varfalling = true;
                    }
                    Debug.DrawRay(transform.position, vargravityray.direction, Color.red);
                    //no acceleration
                    varmovement.y = -vargravity * Time.deltaTime;
                }
                else
                {
                    if (varfalling == true)
                    {
                        varfalling = false;
                    }
                }
                varcontroller.Move(varmovement);
                if (varhelper != null)
                {
                    if (varfalling == true && varfallen == false)
                    {
                        //we're falling. turn into ragdoll
                        Transform varragdoll = varhelper.metgoragdoll(varcontroller.velocity);
                        clscameratarget varcameramanager = Camera.main.GetComponentInChildren<clscameratarget>();
                        if (varcameramanager.vargamcurrentscenario == 1)
                        {
                            varcameramanager.vargamtarget = varragdoll.GetChild(0);
                        }
                        varcontroller.enabled = false;
                        varfallen = false;
                    }
                }
            }
        }

        public void metactivate()
        {
            clsragdollimbifier varrgd = GetComponentInChildren<clsragdollimbifier>();
            if (varrgd != null)
            {
                varrgd.enabled = true;
            }
        }

    }
}