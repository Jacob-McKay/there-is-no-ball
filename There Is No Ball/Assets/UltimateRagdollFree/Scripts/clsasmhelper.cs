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
    /// Demo scene helper class used to demonstrate the animation states manager
    /// </summary>
    public class clsasmhelper : MonoBehaviour
    {
        /// <summary>
        /// inspector slot for the ragdoll root rigidbody
        /// </summary>
        public Rigidbody varragdollbody;
        /// <summary>
        /// collision layer mask for the raycast to determine ragdoll facing
        /// </summary>
        public LayerMask vargamlayermask;

        private bool vartriggered = false;
        private clsurganimationstatesmanager varasm;

        void Start()
        {
            varasm = GetComponent<clsurganimationstatesmanager>();
            if (varasm == null)
            {
                enabled = false;
            }
        }

        private void metgetup()
        {
            if (vartriggered)
            {
                //check our controller speed
                if (varragdollbody.velocity.sqrMagnitude < 3)
                {
                    //we are almost still, so we can try to get up
                    Transform varbodytransform = varragdollbody.transform;
                    RaycastHit varraycasthit;
                    Animation varanimation = GetComponent<Animation>();
                    if (Physics.Raycast(varbodytransform.position, varbodytransform.forward, out varraycasthit, 1, vargamlayermask))
                    {
                        //we hit forward, so we're face down
                        varanimation["get_up"].wrapMode = WrapMode.ClampForever;
                        //pass the root asm bone to the transition animation routine (for reference, the bone's name is stored in the varasm.vargamrootname string)
                        //public static int metcrossfadetransitionanimation(Transform varpcharacter, string varpdestinationanimname, float varptransitiontime, Transform varpcontroller = null, string varpstateanimationname = "", bool varpgokinematic = true, string varpnewanimname = "transition", Animation varpanimationsystem = null, SkinnedMeshRenderer varprenderer = null, clsurganimationstatesmanager varpstatesmanager = null) {
                        clsurgutils.metcrossfadetransitionanimation(varbodytransform, "get_up", 1f, transform, "get_up", true, "transition", null, null, null);
                    }
                    else
                    {
                        //we don't hit forward, so we're face up
                        varanimation["get_up_back"].wrapMode = WrapMode.ClampForever;
                        clsurgutils.metcrossfadetransitionanimation(varbodytransform, "get_up_back", 1f, transform, "get_up_back", true, "transition", null, null, null);
                    }
                    //enabled = false;
                    vartriggered = false;
                    CancelInvoke("metgetup");
                }
            }
        }

        void OnTriggerEnter(Collider varpother)
        {
            if (!vartriggered && varpother.tag == "missile")
            {
                vartriggered = true;
                InvokeRepeating("metgetup", 4, 4);
            }
        }
    }

}