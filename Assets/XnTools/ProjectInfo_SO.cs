using UnityEngine;

// [CreateAssetMenu(fileName = "ProjectInfo", menuName = "ScriptableObjects/ProjectInfo", order = 1)]
public class ProjectInfo_SO : ScriptableObject {
	public Texture2D icon;
	public float iconMaxWidth = 128f;
	public string title;
	public Section[] sections;
	//public bool loadedLayout;
	public bool showDefaultInspector = false;
	
	[System.Serializable]
	public class Section {
		[TextArea(1,10)]
		public string heading, text, linkText, url;
	}
}