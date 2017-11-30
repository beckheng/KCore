using UnityEngine;
using UnityEditor;
using System.Collections;

public class KAssetPostprocessor : AssetPostprocessor
{

	/// <summary>
	/// 设置音频文件导入后的推荐参数
	/// </summary>
	/// <param name="ac"></param>
	private void OnPostprocessAudio(AudioClip audioClip)
	{
		if (assetPath.StartsWith("Assets/Arts/Sounds/"))
		{
			AudioImporter audioImporter = assetImporter as AudioImporter;

			AudioImporterSampleSettings sampleSettings = audioImporter.defaultSampleSettings;
			sampleSettings.loadType = AudioClipLoadType.Streaming;
			
			audioImporter.defaultSampleSettings = sampleSettings;
		}
	}

	/// <summary>
	/// 设置模型导入后的推荐参数,并且有编写的规则, XXX@action这样是表示有动作的
	/// </summary>
	private void OnPostprocessModel(GameObject go)
	{
		if (assetPath.StartsWith("Assets/Arts/Models/"))
		{
			ModelImporter modelImporter = assetImporter as ModelImporter;

			if (assetPath.Contains("@"))
			{
				//包含动作的文件
				modelImporter.animationType = ModelImporterAnimationType.Generic;

				modelImporter.sourceAvatar = null;
			}
			else
			{
				//模型本身
				modelImporter.animationType = ModelImporterAnimationType.Generic;
				modelImporter.importAnimation = false;

				modelImporter.sourceAvatar = null;
			}
		}
	}

}
