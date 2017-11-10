using UnityEngine;
using UnityEditor;

namespace KCore
{

	public class HelperMenu : Editor
	{

		[MenuItem("KCore/Open Persistent DataPath", false, 1050)]
		public static void OpenPersistentDataPath()
		{
			EditorUtility.RevealInFinder(Application.persistentDataPath);
		}

	}

}
