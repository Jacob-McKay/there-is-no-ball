using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
//using UnityEditor;

/// <summary>
/// 2015-04-01
/// ULTIMATE RAGDOLL GENERATOR V4.6
/// © THE ARC GAMES STUDIO 2015
///
/// Aux class functional for URG demo scene only
/// VERY IMPORTANT: THIS SCRIPT IS NOT MEANT TO BE REUSABLE NOR DEPLOYABLE IN SCENES OTHER THAN URG! DEMO as its functions are unsupported
/// </summary>
public class clslayersetter : MonoBehaviour {
	private const int conmaxlayers = 32;
	public int vargamoverridestartindex = 5;
	public string[] vargamoverridenames = new string[0];
	public clslayermatrix[] vargamnewlayermatrix = new clslayermatrix[conmaxlayers];
	public clslayermatrix[] vargamoriginallayermatrix = new clslayermatrix[conmaxlayers];

	#if UNITY_EDITOR
	void Start () {

		int varcurrentlayer;
		int varcurrentoriginallayer;
		string varcurrentlayername = "";
		string varoriginallayername = "";
		
		//init the matrix structure
		//vargamnewlayermatrix = new clslayermatrix[conmaxlayers];
		vargamoriginallayermatrix = new clslayermatrix[conmaxlayers];
		for (varcurrentlayer = 0; varcurrentlayer < conmaxlayers; varcurrentlayer++) {
			//vargamnewlayermatrix[varcurrentlayer] = new clslayermatrix();
			vargamoriginallayermatrix[varcurrentlayer] = new clslayermatrix();
			//vargamnewlayermatrix[varcurrentlayer].proplayernodes = new clslayersetter.clslayermatrixnode[conmaxlayers];
			vargamoriginallayermatrix[varcurrentlayer].proplayernodes = new clslayersetter.clslayermatrixnode[conmaxlayers];
			for (varcurrentoriginallayer = 0; varcurrentoriginallayer < conmaxlayers; varcurrentoriginallayer++) {
				//vargamnewlayermatrix[varcurrentlayer].proplayernodes[varcurrentoriginallayer] = new clslayersetter.clslayermatrixnode();
				vargamoriginallayermatrix[varcurrentlayer].proplayernodes[varcurrentoriginallayer] = new clslayersetter.clslayermatrixnode();
				//initialize all true collisions for the new layer matrix
				//vargamoriginallayermatrix[varcurrentlayer].proplayernodes[varcurrentoriginallayer].propcollide = true;
			}
		}
		
		//compile the original layer matrix
		//NOTE: the script works by setting the IGNORE flags, based on the NAMED vargamnewlayermatrix names. this means that, to override or set flags, it's necessary to compile the related vargamnewlayermatrix layer
		//which will be copied over the original layer slot during runtime
		for (varcurrentlayer = 0; varcurrentlayer < conmaxlayers; varcurrentlayer++) {
			//get the name of the layer in the new layer matrix
			varcurrentlayername = vargamnewlayermatrix[varcurrentlayer].propname;
			//layer is not named so it's not meant to be configured
			if (varcurrentlayername == " ") {
				//get the original layer name
				varcurrentlayername = UnityEditorInternal.InternalEditorUtility.GetLayerName(varcurrentlayer);
				//force a space to create a 'placeholder' for better readability. a blank space gets translated into an 'element' array name in the inspector.
				if (varcurrentlayername == "") {
					varcurrentlayername = " ";
				}
			}
			//iterate through the layers
			for (varcurrentoriginallayer = 0; varcurrentoriginallayer < conmaxlayers; varcurrentoriginallayer++) {
				varoriginallayername = UnityEditorInternal.InternalEditorUtility.GetLayerName(varcurrentoriginallayer);
				//check if the original matrix slot is used, to override its name if necessary, eventually setting it to the vargamnewlayermatrix one
				if (varoriginallayername == "") {
					//force a space to create a 'placeholder' for better readability. a blank space gets translated into an 'element' array name in the inspector.
					varoriginallayername = " ";
				}
				//save the original layer matrix state
				vargamoriginallayermatrix[varcurrentlayer].proplayernodes[varcurrentoriginallayer].propcollide = !Physics.GetIgnoreLayerCollision(varcurrentlayer, varcurrentoriginallayer);
				//set the current matrix state based on the 'template' of the vargamnewlayermatrix
				if (vargamnewlayermatrix[varcurrentlayer].proplayernodes[varcurrentoriginallayer].propcollide == false) {
					Physics.IgnoreLayerCollision(varcurrentlayer, varcurrentoriginallayer);
				}
				//set the child node name
				vargamoriginallayermatrix[varcurrentlayer].proplayernodes[varcurrentoriginallayer].propname = varoriginallayername;
			}
			//set the root node names
			vargamoriginallayermatrix[varcurrentlayer].propname = varcurrentlayername;
		}
	}
	#endif
	
	[System.SerializableAttribute]
	public class clslayermatrix {
		public string propname;
		public clslayermatrixnode[] proplayernodes = new clslayermatrixnode[conmaxlayers];
		
		public clslayermatrix() {
			propname = " ";
		}
	}
	
	[System.SerializableAttribute]
	public class clslayermatrixnode {
		public string propname;
		public bool propcollide;
		
		public clslayermatrixnode() {
			propname = " ";
			propcollide = true;
		}
	}
}
