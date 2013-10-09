////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Core
//
//  File:    PostBuildProcessor.cs
//  Authors:  Ideum
//
//  NOTICE: Ideum permits you to use, modify, and distribute this file only
//  in accordance with the terms of the license agreement accompanying it.
//
////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class PostBuildProcessor {
	
	[PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
		if(target.ToString()=="StandaloneWindows"){
			
			string gmlFileName = "my_gestures.gml";
			string coreDllFileName = "GestureworksCore64.dll";
		
			string pathToNewDataFolder = "";
			string pathToAssetsFolder = UnityEngine.Application.dataPath;
			pathToAssetsFolder = pathToAssetsFolder.Replace("/", "\\");

			//destination /Bin folder
			string[] pathPieces = pathToBuiltProject.Split("/".ToCharArray() );
			string exeName = pathPieces[pathPieces.Length-1];
			
			exeName = exeName.Trim(".exe".ToCharArray()); // extract the name of the exe to use with the name of the data folder
			for(int i=1; i<pathPieces.Length; i++){
				pathToNewDataFolder += pathPieces[i-1]+"\\"; // this will grab everything except for the last
			}
			pathToNewDataFolder += exeName+"_Data\\";
			//Debug.Log("pathToAssetsFolder: "+pathToAssetsFolder);
	        //Debug.Log("pathToNewDataFolder: "+pathToNewDataFolder);
			
			//PostBuildProcessor window = EditorWindow.GetWindow<PostBuildProcessor> ("Post Build Options");
			FileUtil.CopyFileOrDirectory(pathToAssetsFolder+"\\MyScripts\\"+gmlFileName, pathToNewDataFolder+gmlFileName);
			FileUtil.CopyFileOrDirectory(pathToAssetsFolder+"\\Plugins\\Gestureworks\\Core\\"+coreDllFileName, pathToNewDataFolder+coreDllFileName);
			
		} else {
			//Debug.Log("Only the Windows platform is supported only with Gestureworks");
		}

    }

}