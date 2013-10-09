using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GestureWorksConfiguration
{
	private Dictionary<string, string> entries = new Dictionary<string, string>();
	
	public const string ConfigurationFileName = "gestureworks-configuration.txt";
	
	private static GestureWorksConfiguration instance = null;
	public static GestureWorksConfiguration Instance
	{
		get
		{
			if(instance == null)
				instance = new GestureWorksConfiguration();
			
			return instance;
		}
	}
	
	private GestureWorksConfiguration()
	{
		
	}
	
	public bool Initialize()
	{
		string filePath;
		
		if(Application.isEditor)
		{
			filePath = GestureWorksUnity.Instance.ApplicationDataPath + 
						GestureWorksUnity.Instance.DllFilePathEditor + ConfigurationFileName;
		} 
		else 
		{
			filePath = GestureWorksUnity.Instance.ApplicationDataPath + "\\" + ConfigurationFileName;
		}
	
		if(!File.Exists(filePath))
		{
			if(!Application.isEditor)
			{
				Debug.LogError("Could not find read gestureworks configuration file: " + filePath);
			}
			
			return false;
		}
		
		StreamReader textStream = new StreamReader(filePath);
		
		string line = textStream.ReadLine();
		while(line != null)
		{
			string[] lines = line.Split('=');
			
			if(lines.Length >= 2)
			{
				string key = lines[0].Trim();
				string configValue = lines[1].Trim();
				
				if(key == "#")
					continue;
				
				if(key.Length > 0 && configValue.Length > 0)
				{
					entries[key] = configValue;
				}
			}
			
			line = textStream.ReadLine();	
		}
		
		textStream.Close();
		
		return true;
	}
	
	public static void WriteConfigurationFile(string filePath, Dictionary<string, string> configEntries)
	{
		string outputPath = filePath + "\\" + ConfigurationFileName;
		
		StreamWriter streamWriter = new StreamWriter(outputPath);
		
		if(streamWriter == null)
		{
			Debug.LogError("Could not write configuration file: " + outputPath);
			return;
		}
		
		foreach(string key in configEntries.Keys)
		{
			string entry = key + " = " + configEntries[key];
			streamWriter.WriteLine(entry);
		}
		
		streamWriter.Close();
	}
	
	public string KeyValue(string key)
	{
		if(!entries.ContainsKey(key))
		{
			return "";	
		}
		
		return entries[key];
	}
	
	public float KeyValueFloat(string key)
	{
		string config = KeyValue(key);
		
		float configValue;
		if(float.TryParse(config, out configValue))
		{
			return configValue;	
		}
		
		return 0.0f;
	}
	
	public int KeyValueInt(string key)
	{
		string config = KeyValue(key);
		
		int configValue;
		if(int.TryParse(config, out configValue))
		{
			return configValue;	
		}
		
		return 0;
	}
	
	public bool KeyValueBoolean(string key)
	{
		string config = KeyValue(key);
		
		bool configValue;
		if(bool.TryParse(config, out configValue))
		{
			return configValue;	
		}
		
		return false;
	}
}