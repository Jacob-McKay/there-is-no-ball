using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using U_r_g_utils;

namespace U_r_g
{
    [System.Serializable]
    /// <summary>
    /// 2015-04-01
    /// ULTIMATE RAGDOLL GENERATOR V4.6
    /// © THE ARC GAMES STUDIO 2015
    /// 
    /// Simple wrapper class to overwrite a target Unity4 skinned mesh renderer triangles and maintain compatibility with Unity 3.5
    /// </summary>
    public class clsdismemberatorwrapperinterface : clsdismemberatorwrapper
    {
        private Mesh varoriginalmesh;

        void Awake()
        {
            if (vargskm != null)
            {
                varoriginalmesh = vargskm.sharedMesh;
                if (varoriginalmesh != null)
                {
                    if ((varoriginalmesh.triangles.Length != proptriangles.Length) || (vargforcewrap == true))
                    {
                        metrestorewrapper();
                    }
                }
            }
        }
    }
}