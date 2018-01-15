using UnityEngine;
using U_r_g_utils;

namespace U_r_g
{
    /// <summary>
    /// 2015-04-01
    /// ULTIMATE RAGDOLL GENERATOR V4.6
    /// © THE ARC GAMES STUDIO 2015
    ///
    /// special class for the ASM Tester scene. implements prefab spawning, animation and mecanim to ragdoll and ragdoll to animation calls
    /// REFER TO THE README FILE FOR USAGE SPECIFICATIONS
    /// <para>Basic class to test a controllable mecanim character against the animation states manager utility, to determine if compilation and asm transition complete correctly.</para>
    /// <para>Please refer to the in-scene information in play mode for usage specifications.</para>
    /// </summary>
    public class clsasmtester : MonoBehaviour
    {
        public GameObject vargamsource = null;
        public float vargamtransitiontime = 0.5f;
        private bool vartargetisragdoll = false;
        /// <summary>
        /// Spawn position for the reinstantiation
        /// </summary>
        public Vector3 vargamspawnposition = new Vector3(0, 0, 0);

        /// <summary>
        /// The rigidbody array that's used to detach ragdolled parts
        /// </summary>
        private Rigidbody[] varDbodies = new Rigidbody[0];
        /// <summary>
        /// The instanced model
        /// </summary>
        private GameObject vargamasmtarget;

        void Start()
        {
            if (vargamsource != null)
            {
                if (vargamsource.transform.root == transform.root)
                {
                    Debug.LogError("Can't host the tester on the target.\nPlease host the tester in a persistent scene object (for example the main camera).", gameObject);
                    return;
                }
                metinstantiatemodel();
                varDbodies = vargamasmtarget.GetComponentsInChildren<Rigidbody>();
                if (varDbodies.Length == 0)
                {
                    Debug.LogError("There's no rigidbodies to test in the chosen target: make sure it's ragdolled and prefabbed.");
                }

            }
            else
            {
                Debug.LogError("Please assign a model to be able to test its ASM transitions.", transform);
            }
        }

        bool varlegacy = false;
        Animation varanimation = null;
        Animator varanimator = null;
        clsurganimationstatesmanager varasm = null;
        void OnGUI()
        {
            if (GUILayout.Button("Go ragdoll"))
            {
                if (vargamasmtarget == null)
                {
                    Debug.Log("Please assign a mecanim game character to the vargamasmtarget slot of this script", gameObject);
                    return;
                }
                varlegacy = true;
                varanimation = vargamasmtarget.GetComponentInChildren<Animation>();
                if (varanimation == null)
                {
                    varlegacy = false;
                    varanimator = vargamasmtarget.GetComponentInChildren<Animator>();
                    if (varanimator == null)
                    {
                        Debug.Log("No animation nor animator found on source. Need one of the two components to perform ASM operations.", vargamasmtarget);
                        return;
                    }
                }
                varasm = vargamasmtarget.GetComponentInChildren<clsurganimationstatesmanager>();
                if (varasm == null)
                {
                    Debug.Log("No animationn states manager found on source. Need the ASM to perform ragdoll transitions.", vargamasmtarget);
                    return;
                }
                CharacterController varcontroller = vargamasmtarget.GetComponent<CharacterController>();
                if (varcontroller == null)
                {
                    Debug.Log("No controller found on source. A controller is suggested to fully test ragdoll transitions.", vargamasmtarget);
                }

                //disable help GUI
                clscameratarget varscenegui = GetComponent<Camera>().GetComponentInChildren<clscameratarget>();
                if (varscenegui != null)
                {
                    varscenegui.vargamcurrentscenario = 1;
                }

                Vector3 varspeed = Vector3.zero;
                if (varcontroller != null)
                {
                    varspeed = varcontroller.velocity;
                }
                if (varlegacy == true)
                {
                    //go-ragdoll: make sure the animation system is running so that we can play the new animation
                    varanimation.enabled = true;
                    varanimation.Stop();
                }
                else
                {
                    //go-ragdoll: Disable the animations so that the ragdoll can take over
                    varanimator.enabled = false;
                }
                clsurgutils.metgodriven(vargamasmtarget.transform, varspeed);

                //Set the ragdoll flag to true (merely necessary for this scripts' GUI)
                vartargetisragdoll = true;
            }
            if (GUILayout.Button("Reload scene"))
            {
#if UNITY_5
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
#else
            Application.LoadLevel (Application.loadedLevelName);
#endif
            }

            if (vartargetisragdoll == true)
            {
                foreach (string varstatename in varasm.vargamstatenames)
                {
                    if (GUILayout.Button(varstatename))
                    {
                        vartargetisragdoll = false;
                        //the actual ragdoll to mecanim function call
                        if (varlegacy == true)
                        {
                            //legacy call
                            clsurganimationstatesmanager varmanager = vargamasmtarget.GetComponentInChildren<clsurganimationstatesmanager>();
                            Transform varroot = null;
                            foreach (Transform varcurrenttransform in vargamasmtarget.GetComponentsInChildren<Transform>())
                            {
                                if (varcurrenttransform.name == varmanager.vargamrootname)
                                {
                                    varroot = varcurrenttransform;
                                }
                            }
                            if (varroot != null)
                            {
                                int varreturn = clsurgutils.metcrossfadetransitionanimation(varroot, varstatename, vargamtransitiontime);
                                if (varreturn < 0)
                                {
                                    Debug.Log("Transition failed with code " + varreturn);
                                }
                            }
                            else
                            {
                                Debug.Log("Can't perform transition since asm root was not found on target [" + varmanager.vargamrootname + "]");
                            }
                        }
                        else
                        {
                            //mecanim call
                            //PLEASE NOTE: for special, post transition actions, a special overload to metinterpolatetoanimationstate is available, which takes an ACTION as a parameter
                            //an example action function is available below, in the metasmaction function. Be sure to customize the action for proper model-animation operations
                            StartCoroutine(clsurgutils.metinterpolatetoanimationstate(vargamasmtarget.transform, varstatename, vargamtransitiontime, true));
                        }
                    }
                }
            }
            else
            {
                GUILayout.Label("Need ragdoll state for ASM");
            }
        }

        /// <summary>
        /// Instantiate the model
        /// </summary>
        private void metinstantiatemodel()
        {
            vargamasmtarget = Instantiate(vargamsource, transform.position, Quaternion.identity) as GameObject;
            Transform varinstancetransform = vargamasmtarget.transform;
            varinstancetransform.position = vargamspawnposition;
            varinstancetransform.rotation = Quaternion.identity;
            varinstancetransform.parent = null;
            clscameratarget varcamtarget = Camera.main.GetComponentInChildren<clscameratarget>();
            varcamtarget.vargamtarget = varinstancetransform;
            varcamtarget.vargamscenarios[0].proptarget = varinstancetransform;
        }

        /// <summary>
        /// Can be used as a parameter for the ragdoll to mecanim function
        /// IMPORTANT: implement user and model specific function here
        /// </summary>
        private void metasmaction(Transform varptarget)
        {
            varanimator.Play("get_up_back");
        }
    }
}