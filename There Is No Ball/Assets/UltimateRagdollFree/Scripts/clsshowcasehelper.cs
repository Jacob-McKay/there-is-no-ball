using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using U_r_g_utils;

namespace U_r_g
{
    /// <summary>
    /// 2015-04-01
    /// ULTIMATE RAGDOLL GENERATOR V4.6
    /// © THE ARC GAMES STUDIO 2015
    /// 
    /// Core manager script of the showcase, is instantiated in all scene gameobjects that interact with the camera manager, and performs actions related to demo sections.
    /// </summary>
    public class clsshowcasehelper : MonoBehaviour
    {
        public int vargamaction = 0;
        public GameObject vargamactor = null;

        private bool varself = false;
        private bool varactivated = false;
        private GameObject vartarget = null;

        /// <summary>
        /// perform the helper action. triggered by sendmessage, starting with the clscameratarget script
        /// this script is hosted by the various actors on the scene, and the clscameratarget has a case switch
        /// that is called for each showcase
        /// </summary>
        void metactivate()
        {
            Animation varanimation = null;
            varactivated = true;
            switch (vargamaction)
            {
                case 1:
                    varself = true;
                    //bomb spawn				
                    metspawn();
                    //emit spawn cloud
                    Invoke("metemit", 0);
                    Transform varbombtransform = vartarget.transform;
                    //randomize bomb rotation. NOTE: the maximum torque was raised from the default in the project settings
                    Rigidbody varbody = varbombtransform.GetComponent<Rigidbody>();
                    varbody.AddForce(new Vector3(0, 2, 0), ForceMode.VelocityChange);
                    varbody.maxAngularVelocity = U_r_g_utils.clsurgutils.cnsMaxTorque;
                    varbody.AddTorque(Random.Range(5, U_r_g_utils.clsurgutils.cnsMaxTorque), 0, Random.Range(5, U_r_g_utils.clsurgutils.cnsMaxTorque), ForceMode.VelocityChange);
                    break;
                case 2:
                    //cheesy explosion and dismember
                    varself = false;
                    Invoke("metspawnexplosion", 2);
                    break;
                case 3:
                    //bomb fuse and explosion
                    varself = true;
                    metemit();
                    break;
                case 4:
                    //zombie animation stop
                    varanimation = GetComponent<Animation>();
                    varanimation.Stop();
                    //go physic driven
                    clsurgutils.metgodriven(transform);
                    break;
                case 5:
                    //lerpz kinematic drive, used in the Basic use: kinematic ragdoll scenario
                    StartCoroutine("metDriveRagdoll", 3.0f);
                    break;
                case 6:
                    //alien animation states
                    varanimation = GetComponent<Animation>();
                    varanimation["Balance"].wrapMode = WrapMode.Loop;
                    InvokeRepeating("metAsm", 0, 8);
                    break;
                case 7:
                    //bike arbitrary dismemberment
                    break;
                case 8:
                    //ragdollimbifier cannon activation
                    clscannon varcannon = GetComponent<clscannon>();
                    if (varcannon != null)
                    {
                        varcannon.metactivate();
                    }
                    break;
                default:
                    Debug.LogError("Unmanaged action [" + vargamaction + "]");
                    break;
            }
        }

        /// local rewrite of the path finding routine, to deal with proper curve path name finding
        /// </summary>
        /// <param name='varpbone'>
        /// the transform of the bone to find the path for
        /// </param>
        /// <param name='varpstopby'>
        /// the name of an optional bone name after which the path is considered complete
        /// </param>
        private string metgetbonepath(Transform varpbone, string varpstopby = "")
        {
            string varreturnvalue = "";
            Transform varcurrentpart = varpbone;
            if (varpbone == null)
                return varreturnvalue;
            varreturnvalue = varpbone.name;
            while (varcurrentpart.parent != null && varpbone.name != varpstopby)
            {
                varcurrentpart = varcurrentpart.parent;
                varreturnvalue = varcurrentpart.name + "/" + varreturnvalue;
                if (varcurrentpart.name == varpstopby)
                {
                    break;
                }
            }
            return varreturnvalue;
        }

        private IEnumerator metAsmRoutine()
        {
            yield return new WaitForSeconds(1.3f);
            Animation varanimation = GetComponent<Animation>();
            varanimation.Stop();
            clsurgutils.metgodriven(vargamactor.transform, false);
            yield return new WaitForSeconds(1.5f);
            clsurgutils.metcrossfadetransitionanimation(vargamactor.transform, "Rise", 1f, vargamactor.transform.root, "", true, "transition", null, null, null);
            yield return new WaitForSeconds(varanimation["transition"].length);
            varanimation.CrossFade("Rise");
            varanimation.CrossFadeQueued("Balance");
        }

        private void metAsm()
        {
            StartCoroutine("metAsmRoutine");
        }

        private IEnumerator metDriveRagdoll(float varpWait)
        {
            yield return new WaitForSeconds(varpWait);
            Animation varanimation = GetComponent<Animation>();
            varanimation.Stop();
            clsurgutils.metgodriven(transform);
        }

        private void metspawnexplosion()
        {
            metspawn();
            metdismember();
            Destroy(gameObject);
        }

        /// <summary>
        /// dismember the zombie
        /// </summary>
        private void metdismember()
        {
            GameObject varzombieD = GameObject.Find("/Zombie_D");
            Rigidbody[] varzombiebodies = varzombieD.GetComponentsInChildren<Rigidbody>();
            clsdismemberator varD = varzombieD.GetComponentInChildren<clsdismemberator>();
            int varsplat = varzombiebodies.Length;
            varsplat = Random.Range(varsplat / 3, varsplat / 2);
            List<Transform> varseparated = new List<Transform>();
            bool varlastcut = false;
            for (int varcounter = 0; varcounter < varsplat; varcounter++)
            {
                if (varcounter == varsplat - 1)
                {
                    varlastcut = true;
                }
                Transform varcurrentcut = clsurgutils.metdismemberpart(varzombiebodies[Random.Range(2, varzombiebodies.Length)].transform, varD.vargamstumpmaterial, varD, null, null, true, varlastcut);
                if (varcurrentcut != null)
                {
                    varseparated.Add(varcurrentcut);
                }
            }
            //varzombieD.SendMessage("metactivate");
            Vector3 varforcedirection;
            float vartorque;
            foreach (Transform varcurrentseparation in varseparated)
            {
                Rigidbody[] varbodies = varcurrentseparation.GetComponentsInChildren<Rigidbody>();
                varforcedirection = varcurrentseparation.position - transform.position;
                Debug.DrawRay(transform.position, varforcedirection, Color.yellow);
                for (int varbodycounter = 0; varbodycounter < varbodies.Length; varbodycounter++)
                {
                    varbodies[varbodycounter].AddForce(varforcedirection * 3.5f, ForceMode.VelocityChange);
                    vartorque = Random.Range(1, U_r_g_utils.clsurgutils.cnsMaxTorque);
                    varbodies[varbodycounter].maxAngularVelocity = vartorque;
                    varbodies[varbodycounter].AddTorque(new Vector3(vartorque, vartorque, vartorque), ForceMode.VelocityChange);
                }
            }
        }

        public void metspawn()
        {
            vartarget = Instantiate(vargamactor, transform.position, Quaternion.identity) as GameObject;
            vartarget.SendMessage("metactivate");
        }

        public void metemit()
        {
            if (varactivated == false)
            {
                Debug.LogError("Need to be activated.");
                return;
            }
            ParticleEmitter varemitter = null;
            if (varself)
            {
                varemitter = GetComponentInChildren<ParticleEmitter>();
            }
            else
            {
                if (vargamactor == null)
                {
                    Debug.LogError("No actor", gameObject);
                    return;
                }
                varemitter = vargamactor.GetComponentInChildren<ParticleEmitter>();
            }
            if (varemitter == null)
            {
                Debug.LogError("No emitter", gameObject);
                return;
            }
            varemitter.emit = true;
        }

    }
}