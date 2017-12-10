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
		/// 根据配置表生成.json文件
		/// </summary>
		[MenuItem("KCore/生成配置数据(无结构变化)", false, 1051)]
		private static void GenExcelOnly()
		{
			YamlMappingNode mapping = GetYamlMappingNode();

			string creatorPath = mapping.Children[new YamlScalarNode("CreatorPath")].ToString();

			string projectPath = Application.dataPath + "/../..";

			ProcessUtilEditor.StartProcess("perl", creatorPath + "/gen_protobuf_excel.pl " + projectPath);
		}

		/// <summary>
		/// 根据配置表生成.json文件及.cs代码
		/// </summary>
		[MenuItem("KCore/生成配置数据及.cs代码", false, 1052)]
		private static void GenExcelAndCS()
		{
			YamlMappingNode mapping = GetYamlMappingNode();
			
			string creatorPath = mapping.Children[new YamlScalarNode("CreatorPath")].ToString();
			
			string projectPath = Application.dataPath + "/../..";

			ProcessUtilEditor.StartProcess("perl", creatorPath + "/gen_protobuf_excel.pl " + projectPath);
			ProcessUtilEditor.StartProcess("perl", creatorPath + "/gen_protobuf_cs.pl " + projectPath);
		}

		[MenuItem("KCore/打开配置数据目录", false, 1053)]
		private static void OpenExcelPath()
		{
			YamlMappingNode mapping = GetYamlMappingNode();

			string projectName = mapping.Children[new YamlScalarNode("projectName")].ToString();
			string projectPath = Application.dataPath + "/../..";

			string excelPath = projectPath + "/" + projectName + "_GameDesign/excels";
			ProcessUtilEditor.OpenURL(excelPath);
		}

		/// <summary>
		/// 加载由GameCreator生成的YAML配置文件
		/// </summary>
		private static YamlMappingNode GetYamlMappingNode()
		{
			string yamlConfigFile = Application.dataPath + "/../../config.yml";

			string yamlConfigContent = File.ReadAllText(yamlConfigFile);

			// Load the stream
			var yaml = new YamlStream();
			yaml.Load(new StringReader(yamlConfigContent));

			// Examine the stream
			var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

			return mapping;
		}

	}

}
