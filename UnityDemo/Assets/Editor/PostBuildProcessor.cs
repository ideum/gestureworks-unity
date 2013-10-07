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
	
	private const string GmlFileName = "";
	private const string DLLFileName = "";
	
	[PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
		
		
		if(target.ToString()=="StandaloneWindows"){
			
			string gmlFileName = GestureWorksUnity.Instance.GmlFileName;
			string coreDllFileName = GestureWorksUnity.Instance.DllFileName;
		
			string pathToNewDataFolder = "";
			string pathToAssetsFolder = UnityEngine.Application.dataPath;
			pathToAssetsFolder = pathToAssetsFolder.Replace("/", "\\");

			//destination /Bin folder
			string[] pathPieces = pathToBuiltProject.Split("/".ToCharArray() );
			
			if(pathPieces.Length == 0) {
				return;	
			}
			
			string exeName = pathPieces[pathPieces.Length-1];
			
			exeName = exeName.Replace(".exe", ""); // extract the name of the exe to use with the name of the data folder
			for(int i=1; i<pathPieces.Length; i++) {
				pathToNewDataFolder += pathPieces[i-1] + "\\"; // this will grab everything except for the last
			}
			pathToNewDataFolder += exeName + "_Data\\";
			
			FileUtil.CopyFileOrDirectory(pathToAssetsFolder + GestureWorksUnity.Instance.GmlFilePathEditor + gmlFileName, 
				pathToNewDataFolder + gmlFileName);
			
			FileUtil.CopyFileOrDirectory(pathToAssetsFolder + GestureWorksUnity.Instance.DllFilePathEditor + coreDllFileName, 
				pathToNewDataFolder + coreDllFileName);			
		}
    }
}