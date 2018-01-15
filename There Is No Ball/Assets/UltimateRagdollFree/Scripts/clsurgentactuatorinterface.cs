using UnityEngine;
using U_r_g_utils;

namespace U_r_g
{
    /// <summary>
    /// <para>2015-04-01</para>
    /// <para>ULTIMATE RAGDOLL GENERATOR V4.6</para>
    /// <para>© THE ARC GAMES STUDIO 2015</para>
    /// <para>URG-ENTITIES ACTUATOR INTERFACE CLASS FOR SPECIAL BODY PART OPERATIONS</para>
    /// 
    /// <para>Basic actuator class that will call the URGent source methods on trigger or collision, after processing own physic functions.</para>
    /// <para>All URGent body parts will have this class attached to handle collision events, whenever the URGent option is enabled in URG! interface.</para>
    /// <para>Naturally, the class can be separately added at will to any gameobject to perform any common physic event, shared with URG entities.</para>
    /// <para>Behaviors are implemented by means of inheritance or by the clsurgent public variable vargamcasemanager, which is used in the actuators as a switch variable. Basically, any URGent gameobject host can have a different vargamcasemanager value to setup its specific behavior in clsurgent collision or trigger events.</para>
    /// 
    /// <para>USAGE NOTE: URG has a hard coded reference for 'clsurgent' and 'clsdismemberator' classes. User can edit them at will, but needs be sure that these two classes always exist when creating URGent ragdolls</para>
    /// 
    /// <para>NOTE: it is expected and suggested that the game model be ragdolled SEPARATED from its possible prefab parent. Notice in fact the references to vargamurgentsource.transform as the 'root' of the object, to avoid referencing an actual parent that might not be in the original 3D model</para>
    /// </summary>
    public class clsurgentactuatorinterface : clsurgentactuator
    {
        //uncomment this section to manage urgent clicks on body parts
        /*
        void OnMouseDown() {
            metmanagekinematic();
        }
        */

        private void metmanagekinematic()
        {
            //this condition is used when the script is attached to "scenery", for example a palisade or a fence, or a weapon in the character's hand
            //that needs to become physical and detach from its parent
            Rigidbody varbody = GetComponent<Rigidbody>();
            if (vargamurgentsource == null)
            {
                varbody.isKinematic = false;
                transform.parent = null;
            }
            else
            {
                //this instead is the code for the 'death' condition that will make the game object entirely ragdoll driven
                //for a start, stop ongoing animations. we assume that our urgent source is hosted together with the Animation component
                Animation varanimation = vargamurgentsource.GetComponent<Animation>();
                varanimation.Stop();
                //this condition determines if the current part is head, spine or source, in which case the character is neutralized
                if (transform == vargamsource || vargamparttype == clsurgutils.enumparttypes.head || vargamparttype == clsurgutils.enumparttypes.spine)
                {
                    clsurgutils.metdriveurgent(vargamurgentsource, null);
                    varanimation.Stop();
                    varanimation.animatePhysics = false;
                    //if there's a character controller added to the parent gameobject, we destroy it to stop the movement
                    //note the intentional search of the character controller in the root
                    CharacterController varcontroller = transform.root.GetComponent<CharacterController>();
                    if (varcontroller != null)
                    {
                        Vector3 varforce = varcontroller.velocity;
                        Destroy(varcontroller);
                        //this adds a scenic effect to the ragdoll, to simulate inertia (for a standard 75kg ragdoll weight)
                        varbody = vargamurgentsource.vargamnodes.vargamspine[0].GetComponent<Rigidbody>();
                        varbody.AddForce(varforce * 7500);
                    }
                }
                //otherwise we just drive the body part of the actuator
                else
                {
                    clsurgutils.metdriveurgent(vargamurgentsource, this);
                    transform.parent = vargamurgentsource.transform;
                }
            }
        }

        void OnCollisionEnter(Collision varpsource)
        {
            clsdrop vardropper;
            Rigidbody varbody;
            Vector3 varparamvelocity;
            //lines commented for release polish. uncomment to monitor actuator collision events (attention to non null URGent source)
            //Debug.LogError("actuator collision event "  + transform.name + " " + vargamurgentsource.transform.name + " " + varpsource.transform.name, varpsource.transform);

            //this switch is used to streamline the urgent integration into multiple objects, without the need to create additional scripts that inherit clsurgent
            //vargamcasemanager is a public variable which value is set into the inspector, for the clsurgent host
            switch (vargamurgentsource.vargamcasemanager)
            {
                //example cases, used in demo scenes. additional cases may be added at wish
                case -3:
                    //single urgent part breaker
                    if (vargamactuatorenabled)
                    {
                        //here is the spot for routine code: decrease hitpoints, drop weapon, etc.
                        //in this example code, for instance, we react to a collision from 'missile', to activate our desired effect
                        if (varpsource.gameObject.tag == "missile" && vargamparttype != clsurgutils.enumparttypes.spine)
                        {
                            //turn off the actuator since it performed its conversion
                            vargamactuatorenabled = false;
                            //vargamurgentities.vargamnodes.vargamarmright[0].parent = vargamurgentities.transform;
                            //drive the specific part being hit
                            transform.parent = vargamurgentsource.transform;
                            //clsurgutils.metdriveurgent(vargamurgentsource, this);
                            //we always break the part origin, to simplify interfacing with the part repair functionality
                            clsurgutils.metdrivebodypart(vargamurgentsource, vargamparttype, 0);
                            //interface with the part repairer
                            clsragdollimbifierinterface varlimbifier = transform.root.GetComponentInChildren<clsragdollimbifierinterface>();
                            if (varlimbifier != null)
                            {
                                switch (vargamparttype)
                                {
                                    case clsurgutils.enumparttypes.arm_left:
                                        varlimbifier.varla = true;
                                        break;
                                    case clsurgutils.enumparttypes.arm_right:
                                        varlimbifier.varra = true;
                                        //drop the weapon, if it's still being held
                                        vardropper = transform.root.GetComponentInChildren<clsdrop>();
                                        if (vardropper != null)
                                        {
                                            varparamvelocity = U_r_g_utils.clsurgutils.metreturncollisionforce(varpsource);
                                            vardropper.metdrop(varparamvelocity);
                                        }
                                        break;
                                    case clsurgutils.enumparttypes.leg_left:
                                        varlimbifier.varll = true;
                                        break;
                                    case clsurgutils.enumparttypes.leg_right:
                                        varlimbifier.varrl = true;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                Debug.LogError("No ragdollimbifier found. Part repair compromised.");
                            }
                            //apply the original force
                            varbody = GetComponent<Rigidbody>();
                            varparamvelocity = U_r_g_utils.clsurgutils.metreturncollisionforce(varpsource);
                            varbody.AddForceAtPosition(varparamvelocity, varpsource.contacts[0].point, ForceMode.VelocityChange);
                        }
                    }
                    break;
                case -2:
                    //impact dismemberator

                    //ignore non terrain collisions
                    if (!vargamactuatorenabled || (varpsource.gameObject.tag != "missile" && varpsource.gameObject.tag != "terrain"))
                    {
                        return;
                    }
                    clsdismemberator varD = vargamurgentsource.GetComponentInChildren<clsdismemberator>();
                    if (varD != null)
                    {
                        float varroll = Random.Range(0, 0.99f);
                        if (varroll > 0.75f)
                        {
                            clsurgutils.metdismember(transform, varD.vargamstumpmaterial, varD, varD.vargamparticleparent, varD.vargamparticlechild);
                        }
                    }
                    else
                    {
                        Debug.LogError("No Dismemberator Class in source D host.");
                    }
                    break;
                case -1:
                    //full urgent ragdoll
                    //vargamurgentsource.metcollsionentered(transform);
                    if (vargamactuatorenabled)
                    {
                        //here is the spot for routine code: decrease hitpoints, drop weapon, etc.
                        //in this example code, for instance, we react to a collision from 'missile', to activate our desired effect
                        if (varpsource.gameObject.tag == "missile")
                        {
                            //turn off the actuator since it performed its conversion
                            vargamactuatorenabled = false;
                            //drive the host
                            clsurgutils.metdriveurgent(vargamurgentsource);
                            //apply the original force
                            varbody = GetComponent<Rigidbody>();
                            varparamvelocity = U_r_g_utils.clsurgutils.metreturncollisionforce(varpsource);
                            varbody.AddForceAtPosition(varparamvelocity, varpsource.contacts[0].point, ForceMode.VelocityChange);
                        }
                    }
                    //drop the weapon, if it's still being held
                    vardropper = transform.root.GetComponentInChildren<clsdrop>();
                    if (vardropper != null)
                    {
                        Vector3 varApplyForce = Vector3.zero;
                        varApplyForce = U_r_g_utils.clsurgutils.metreturncollisionforce(varpsource);
                        vardropper.metdrop(varApplyForce);
                    }
                    break;
                default:
                    break;
            }
        }

        void OnTriggerEnter(Collider varpother)
        {
            //lines commented for release polish. uncomment to monitor actuator trigger events (attention to non null URGent source)
            //Debug.LogError("actuator trigger event " + transform.name + " " + vargamurgentsource.transform.name);
            //vargamurgentsource.metcollidertriggered(transform);

            //NOTE: what follows is an variant of the OnCollisionEnter manager (triggers don't have access to the same information as collisions)
            switch (vargamurgentsource.vargamcasemanager)
            {
                //example cases, used in demo scenes. additional cases may be added at wish
                case -2:
                    //ignore non terrain collisions
                    if (!vargamactuatorenabled || varpother.tag != "missile")
                    {
                        return;
                    }
                    metmanagekinematic();
                    clsdismemberator varD = vargamurgentsource.GetComponentInChildren<clsdismemberator>();
                    if (varD != null)
                    {
                        float varroll = Random.Range(0, 0.99f);
                        if (varroll > 0.75f)
                        {
                            clsurgutils.metdismember(transform, varD.vargamstumpmaterial, varD, varD.vargamparticleparent, varD.vargamparticlechild);
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        Debug.LogError("No Dismemberator Class in source D host.");
                    }
                    break;
                //example case
                case -1:
                    //vargamurgentsource.metcollsionentered(transform);
                    if (vargamactuatorenabled)
                    {
                        //routine code: decrease hitpoints, drop weapon, etc.

                        //this example code, for instance, would require an object tagged 'missile' to activate the collision effects
                        if (varpother.gameObject.tag == "missile")
                        {
                            //Destroy(varpsource.collider); //can be used to avoid multiple collisions, for example with bullets
                            Rigidbody varbody = GetComponent<Rigidbody>();
                            if (varbody.isKinematic == true)
                            {
                                metmanagekinematic();
                            }
                            //turn off the actuator since it performed its conversion
                            vargamactuatorenabled = false;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}