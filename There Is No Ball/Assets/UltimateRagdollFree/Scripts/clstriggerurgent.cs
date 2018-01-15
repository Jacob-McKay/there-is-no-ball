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
    /// helper class to trigger the physics driven activation of a ragdolled character
    /// 1- add a collider to the scene object
    /// 2- (if the triggering object has no rigidbody) add a rigidbody to the same scene object, and set it as kinematic
    /// 3- add the clskinetify script to the scene object
    /// 4- for this example we've set the 'Collider' parameter for easiest approach.
    ///    Alternatively, the layer collision matrix can be used, from the project options and physics menu.
    /// </summary>
    public class clstriggerurgent : MonoBehaviour
    {
        Animation varanimation = null;

        void Awake()
        {
            varanimation = GetComponent<Animation>();
            varanimation.wrapMode = WrapMode.ClampForever;
        }

        void OnTriggerEnter(Collider varsource)
        {
            //trigger only with a supposed car gameobject's 'bumper' collider
            if (varsource.name == "bumper")
            {
                //playing animations interrupt the physics process so we stop ongoing animations
                varanimation.Stop();
                //go ragdoll thanks to URGent classes
                clsurgent varurgent = GetComponent<clsurgent>();
                if (varurgent != null)
                {
                    clsurgutils.metdriveurgent(varurgent, null);
                }
            }
        }
    }
}