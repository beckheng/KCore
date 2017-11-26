using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

using KCore;

namespace KCoreEditor
{

	[CustomEditor(typeof(KUIPropertyDefine))]
	public class KUIPropertyDefineEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("生成所属窗体的所有代码"))
			{
				GenAllCode(target as KUIPropertyDefine);
			}
		}

		private void GenAllCode(KUIPropertyDefine target)
		{
			//win用于实时的取得各个定义的组件,没有则生成一个文件
			KUIWindow win = target.GetComponentInParent<KUIWindow>();
			if (win != null)
			{
				//读取对应的目录
				string path = AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(win).GetInstanceID());

				//生成Auto目录
				var fileInfo = new FileInfo(path);

				string autoGenPath = fileInfo.DirectoryName + "/AutoGen";
				string autoGenFileName = win.GetType().Name + ".autogen.cs";

				if (!Directory.Exists(autoGenPath))
				{
					KLogger.Log("KUI|需要创建目录|path|" + autoGenPath);
					Directory.CreateDirectory(autoGenPath);
				}

				string outputName = autoGenPath + "/" + autoGenFileName;
				KLogger.Log("KUI|输出目录|outputName|" + outputName);

				string codeContent = GenCodeFromTemplate(win.transform, win.GetType().Name, KCoreEditorUtil.kuiEditorPath + "/AutoGenCodeTemplate.txt");

				File.WriteAllText(outputName, codeContent);
			}
			else
			{
				//生成一个文件
				string theClassName = target.transform.root.name;
				
				string autoGenPath = KCoreEditorUtil.kuiDefAutoGenPath + "/AutoGen";
				string autoGenFileName = theClassName + ".autogen.cs";

				if (!Directory.Exists(autoGenPath))
				{
					KLogger.Log("KUI|需要创建目录|path|" + autoGenPath);
					Directory.CreateDirectory(autoGenPath);
				}

				string outputName = autoGenPath + "/" + autoGenFileName;
				KLogger.Log("KUI|输出目录|outputName|" + outputName);

				string codeContent = GenCodeFromTemplate(target.transform.root, theClassName, KCoreEditorUtil.kuiEditorPath + "/AutoGenCodeTemplate.txt");

				File.WriteAllText(outputName, codeContent);

				//同时生成主的class
				string mainClassFileName = KCoreEditorUtil.kuiDefAutoGenPath + "/" + theClassName + ".cs";
				if (!File.Exists(mainClassFileName))
				{
					KLogger.Log("KUI|输出目录|mainClassFileName|" + mainClassFileName);
					string mainCodeContent = GenCodeFromTemplate(target.transform.root, theClassName, KCoreEditorUtil.kuiEditorPath + "/UIMainCodeTemplate.txt");
					File.WriteAllText(mainClassFileName, mainCodeContent);
				}
			}
			
			AssetDatabase.Refresh();
		}

		private string GenCodeFromTemplate(Transform tran, string className, string tmplPath)
		{
			string template = File.ReadAllText(tmplPath);


			System.Text.StringBuilder paramsDeclaredStr = new System.Text.StringBuilder();
			System.Text.StringBuilder paramsAssignmentStr = new System.Text.StringBuilder();

			var defines = tran.GetComponentsInChildren<KUIPropertyDefine>(true);
			for (int i = 0; i < defines.Length; i++)
			{
				defines[i].enabled = false;

				string typeName = defines[i].varType;
				string varName = defines[i].varName;
				string summaryStr = defines[i].varSummary;
				string childPath = AnimationUtility.CalculateTransformPath(defines[i].transform, tran);
				
				if (string.IsNullOrEmpty(typeName))
				{
					//在这里逐步补充相应的类型
					if (defines[i].GetComponent<Button>())
					{
						typeName = typeof(Button).Name;
					}
					else if (defines[i].GetComponent<Image>())
					{
						typeName = typeof(Image).Name;
					}
					else if (defines[i].GetComponent<Text>())
					{
						typeName = typeof(Text).Name;
					}
					else
					{
						KLogger.LogError("没有添加要处理的类型,请补充一下咯", defines[i].gameObject);
					}
				}
				
				paramsDeclaredStr.AppendFormat("\r\n		/// <summary>\r\n		/// {2}\r\n		/// </summary>\r\n		protected {0} {1};\r\n", typeName, varName, summaryStr);
				paramsAssignmentStr.AppendFormat("			{1} = tran.GetComponentByName<{0}>(\"{2}\");\r\n", typeName, varName, childPath);
			}
			
			template = template.Replace("__CLASSNAME__", className);
			template = template.Replace("__PARAMS_DECLARED__", paramsDeclaredStr.ToString());
			template = template.Replace("__PARAMS_ASSIGNMENT__", paramsAssignmentStr.ToString());

			return template;
		}
	}
	
}
