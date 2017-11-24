using UnityEngine;
using UnityEditor;

namespace KCoreEditor
{

	public class HelperMenu : Editor
	{

		[MenuItem("KCore/Open Persistent DataPath", false, 1010)]
		public static void OpenPersistentDataPath()
		{
			EditorUtility.RevealInFinder(Application.persistentDataPath);
		}

	}

}
