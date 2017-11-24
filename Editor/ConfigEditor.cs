using UnityEngine;
using UnityEditor;
using System.Collections;

using System.IO;

using YamlDotNet.RepresentationModel;

namespace KCoreEditor
{
	public class ConfigEditor : EditorWindow
	{

		/// <summary>
		/// 根据配置表生成.json文件及.cs代码
		/// </summary>
		[MenuItem("KCore/生成配置数据及.cs代码", false, 1051)]
		private static void GenExcelAndCS()
		{
			string yamlConfigFile = Application.dataPath + "/../../config.yml";
			
			string yamlConfigContent = File.ReadAllText(yamlConfigFile);
			
			// Load the stream
			var yaml = new YamlStream();
			yaml.Load(new StringReader(yamlConfigContent));

			// Examine the stream
			var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

			string creatorPath = mapping.Children[new YamlScalarNode("CreatorPath")].ToString();
			
			string projectPath = Application.dataPath + "/../..";

			ProcessUtilEditor.StartProcess("perl", creatorPath + "/gen_protobuf_excel.pl " + projectPath);
			ProcessUtilEditor.StartProcess("perl", creatorPath + "/gen_protobuf_cs.pl " + projectPath);
		}

	}

}
