using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomBuild {
	[MenuItem("Build/BuildPlayer")]
	static void build(){
		/*BuildPipeline.BuildPlayer (
			EditorBuildSettings.scenes, "export.apk",
			BuildTarget.Android,
			BuildOptions.CompressWithLz4);*/
	}
}
