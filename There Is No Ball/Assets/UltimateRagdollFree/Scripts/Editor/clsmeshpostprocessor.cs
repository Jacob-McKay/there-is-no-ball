using UnityEngine;
using UnityEditor;

/// <summary>
/// 2015-04-01
/// ULTIMATE RAGDOLL GENERATOR V4.6
/// © THE ARC GAMES STUDIO 2015
/// 
/// Utility class used primarily to maintain compatibility with Unity4 during model import.
/// This is required in particular for dismemberment purposes
/// </summary>
class MeshPostprocessor : AssetPostprocessor {

	void OnPreprocessModel () {
		(assetImporter as ModelImporter).optimizeMesh = false;
	}

}