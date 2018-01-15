using UnityEngine;
using U_r_g_utils;

namespace U_r_g
{
    /// <summary>
    /// <para>2015-04-01</para>
    /// <para>ULTIMATE RAGDOLL GENERATOR V4.6</para>
    /// <para>© THE ARC GAMES STUDIO 2015</para>
    /// <para>DESIGNED WITH UNITY 3.4.2f2</para>
    /// 
    /// <para>Helper class to showcase advanced ragdoll functions based on the URG entities manager</para>
    /// 
    /// <para>USAGE NOTE: the animation component's "ANIMATE PHYSICS" of the source must be checked.</para>
    /// 
    /// </summary>
    public class clsragdollimbifierinterface : clsragdollimbifier
    {
        [HideInInspector]
        public bool varla = false;
        [HideInInspector]
        public bool varra = false;
        [HideInInspector]
        public bool varll = false;
        [HideInInspector]
        public bool varrl = false;

        void Start()
        {
            //if we don't have a remote target, we search the URGent manager in our gameobject
            if (vargamurgentities == null)
                vargamurgentities = GetComponent<clsurgent>();
            //we automatically enable the physics animation, required to interact physically with kinematic objects
            Animation varanimation = transform.root.GetComponent<Animation>();
            if (varanimation != null)
                varanimation.animatePhysics = true;
        }

        void OnGUI()
        {
            if (vargamurgentities != null)
            {
                GUILayout.Label("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                if (varla == true)
                {
                    if (GUILayout.Button("- Restore left arm"))
                    {
                        //restore the parent connection
                        clsurgentactuator varactuator = vargamurgentities.vargamnodes.vargamarmleft[0].GetComponent<clsurgentactuator>();
                        vargamurgentities.vargamnodes.vargamarmleft[0].parent = varactuator.vargamparent;
                        //reanimate the limb
                        clsurgutils.metdriveanimatebodypart(vargamurgentities, clsurgutils.enumparttypes.arm_left, 0, true);
                        varla = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("Break left arm"))
                    {
                        //this first instruction disconnects the parent from the animation, so that it can
                        //react to physics
                        vargamurgentities.vargamnodes.vargamarmleft[0].parent = vargamurgentities.transform;
                        //drive the limb
                        clsurgutils.metdrivebodypart(vargamurgentities, clsurgutils.enumparttypes.arm_left, 0);
                        varla = true;
                    }
                }

                if (varra == true)
                {
                    if (GUILayout.Button("- Restore right arm"))
                    {
                        clsurgentactuator varactuator = vargamurgentities.vargamnodes.vargamarmright[0].GetComponent<clsurgentactuator>();
                        if (varactuator != null)
                        {
                            clsurgutils.metdriveanimatebodypart(vargamurgentities, clsurgutils.enumparttypes.arm_right, 0, true);
                        }
                        vargamurgentities.vargamnodes.vargamarmright[0].parent = varactuator.vargamparent;
                        varra = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("Break right arm"))
                    {
                        vargamurgentities.vargamnodes.vargamarmright[0].parent = vargamurgentities.transform;
                        clsurgutils.metdrivebodypart(vargamurgentities, clsurgutils.enumparttypes.arm_right, 0);
                        clsdrop varweapon = GetComponentInChildren<clsdrop>();
                        if (varweapon != null)
                        {
                            varweapon.metdrop(true);
                        }
                        varra = true;
                    }
                }

                if (varll == true)
                {
                    if (GUILayout.Button("- Restore left leg"))
                    {
                        clsurgentactuator varactuator = vargamurgentities.vargamnodes.vargamlegleft[0].GetComponent<clsurgentactuator>();
                        if (varactuator != null)
                        {
                            clsurgutils.metdriveanimatebodypart(vargamurgentities, clsurgutils.enumparttypes.leg_left, 0, true);
                        }
                        vargamurgentities.vargamnodes.vargamlegleft[0].parent = varactuator.vargamparent;
                        varll = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("Break left leg"))
                    {
                        vargamurgentities.vargamnodes.vargamlegleft[0].parent = vargamurgentities.transform;
                        clsurgutils.metdrivebodypart(vargamurgentities, clsurgutils.enumparttypes.leg_left, 0);
                        varll = true;
                    }
                }

                if (varrl == true)
                {
                    if (GUILayout.Button("- Restore right leg"))
                    {
                        clsurgentactuator varactuator = vargamurgentities.vargamnodes.vargamlegright[0].GetComponent<clsurgentactuator>();
                        if (varactuator != null)
                        {
                            clsurgutils.metdriveanimatebodypart(vargamurgentities, clsurgutils.enumparttypes.leg_right, 0, true);
                        }
                        vargamurgentities.vargamnodes.vargamlegright[0].parent = varactuator.vargamparent;
                        varrl = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("Break right leg"))
                    {
                        vargamurgentities.vargamnodes.vargamlegright[0].parent = vargamurgentities.transform;
                        clsurgutils.metdrivebodypart(vargamurgentities, clsurgutils.enumparttypes.leg_right, 0);
                        varrl = true;
                    }
                }

                if (GUILayout.Button("URG!"))
                {
                    clsurgutils.metdriveurgent(vargamurgentities, null);
                    Animation varanimation = vargamurgentities.transform.GetComponent<Animation>();
                    if (varanimation != null)
                    {
                        varanimation.Stop();
                        varanimation.animatePhysics = false;
                    }
                    //note the intentional search of the character controller in the root
                    CharacterController varcontroller = vargamurgentities.transform.root.GetComponent<CharacterController>();
                    Vector3 varforce = Vector3.zero;
                    if (varcontroller != null)
                    {
                        varforce = varcontroller.velocity;
                        varforce.y = 0.1f;
                        Destroy(varcontroller);
                    }
                    clsurgutils.metgodriven(this.transform, varforce);
                    clsdrop varweapon = GetComponentInChildren<clsdrop>();
                    if (varweapon != null)
                    {
                        varweapon.metdrop(varforce * 60);
                    }
                    enabled = false;
                }
            }
        }

    }
}