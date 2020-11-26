using UnityEditor;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathCreation.Examples;

public static class MeshSaverEditor {

	[MenuItem("CONTEXT/RoadMeshCreator/Save Mesh...")]
	public static void SaveMeshInPlace (MenuCommand menuCommand) {
		RoadMeshCreator rc = menuCommand.context as RoadMeshCreator;
		Mesh m = rc.Mesh;
		SaveMesh(m, m.name, false, true);
	}

	[MenuItem("CONTEXT/RoadMeshCreator/Save Mesh As New Instance...")]
	public static void SaveMeshNewInstanceItem (MenuCommand menuCommand) {
		RoadMeshCreator rc = menuCommand.context as RoadMeshCreator;
		Mesh m = rc.Mesh;
		SaveMesh(m, m.name, true, true);
	}

	public static void SaveMesh (Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh) {
		string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
		if (string.IsNullOrEmpty(path)) return;
        
		path = FileUtil.GetProjectRelativePath(path);

		Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;
		
		if (optimizeMesh)
		     MeshUtility.Optimize(meshToSave);
        
		AssetDatabase.CreateAsset(meshToSave, path);
		AssetDatabase.SaveAssets();
	}
	
}
