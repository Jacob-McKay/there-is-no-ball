using UnityEngine;
using System.Collections;

/// <summary>
/// 2015-04-01
/// ULTIMATE RAGDOLL GENERATOR V4.6
/// © THE ARC GAMES STUDIO 2015
/// 
/// Base projectile class, not used in the demo scene
/// </summary>
public class clscannonball : MonoBehaviour {
	public bool vargamenabled = true;
	public clscannon varcannon = null;
	
	void OnCollisionEnter() {
		if (vargamenabled) {
			if (varcannon != null)
				varcannon.metresetactor();
		}
		vargamenabled = false;
	}
}
