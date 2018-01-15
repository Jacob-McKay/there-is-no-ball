using UnityEngine;
using System.Collections;

/// <summary>
/// 2015-04-01
/// ULTIMATE RAGDOLL GENERATOR V4.6
/// © THE ARC GAMES STUDIO 2015
/// DESIGNED WITH UNITY 3.5
/// 
/// RAGDOLLIFIER UTILITIES
/// 
/// This is an utility class that will fetch all rigidbodies of its host gameobject, and apply a random force and torque to them
/// NOTE: the routine can not differentiate ragdoll rigidbodies from non ragdoll rigidbodies
public class clsragdolljolt : MonoBehaviour {
	/// <summary>
	/// jolt values
	/// </summary>
	public float vargamforcemin = -1f, vargamforcemax = 0.5f, vargamtorquemin = -200f, vargamtorquemax = 100f;
	/// <summary>
	/// force type to apply
	/// </summary>
	public ForceMode vargamforcemode = ForceMode.VelocityChange;

	void Start () {
		metjolt(vargamforcemin, vargamforcemax, vargamtorquemin, vargamtorquemax, vargamforcemode);
	}
	
	/// <summary>
	/// apply random ranged forces and rotations to the host rigidbodies
	/// </summary>
	/// <param name='varpforcemin'>
	/// minimum force
	/// </param>
	/// <param name='varpforcemax'>
	/// maximum force
	/// </param>
	/// <param name='varptorquemin'>
	/// minimum torque
	/// </param>
	/// <param name='varptorquemax'>
	/// maximum torque
	/// </param>
	/// <param name='varpforcemode'>
	/// force mode
	/// </param>
	private void metjolt(float varpforcemin, float varpforcemax, float varptorquemin, float varptorquemax, ForceMode varpforcemode) {
		//fetch all rigidbodies of the host
		Rigidbody[] varsourceelements = gameObject.GetComponentsInChildren<Rigidbody>();
		if (varsourceelements.Length < 1) {
			//there are no rigidodies. exit.
			return;
		}
		//iterate through the rigidbodies and apply the random force and torque calculated
		Vector3 varrandomforce = new Vector3(Random.Range(varpforcemin, varpforcemax), Random.Range(varpforcemin, varpforcemax), Random.Range(varpforcemin, varpforcemax));
		Vector3 varrandomtorque = new Vector3(Random.Range(varptorquemin, varptorquemax), Random.Range(varptorquemin, varptorquemax), Random.Range(varptorquemin, varptorquemax));
		for (int varsourcecounter = 0; varsourcecounter < varsourceelements.Length; varsourcecounter++) {
			varsourceelements[varsourcecounter].AddForce(varrandomforce, varpforcemode);
			varsourceelements[varsourcecounter].maxAngularVelocity = varptorquemax;
			varsourceelements[varsourcecounter].AddTorque(varrandomtorque, varpforcemode);
		}
	}
}
