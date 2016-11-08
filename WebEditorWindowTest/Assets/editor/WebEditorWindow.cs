using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

public class WebEditorWindow : EditorWindow
{
	//static string Url = "https://google.com";
	static string Url = "http://localhost:8080/accounts/create-form";

	[MenuItem ("Testing/Open web window")]
	public static void launch() {
		var window = EditorWindow.GetWindow<WebEditorWindow>(typeof(SceneView)); //dock next to sceneview
		//var window = WebEditorWindow.GetWindow<WebEditorWindow>();
		GUIContent titleContent = new GUIContent ("Analytics");
		window.titleContent = titleContent;
		//window.Show();
		OpenWebView(window);
	}

	static void OpenWebView(WebEditorWindow window)
	{
		var thisWindowGuiView = typeof(EditorWindow).GetField("m_Parent", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(window);

		Type webViewType = GetTypeFromAllAssemblies("WebView");
		var webView = ScriptableObject.CreateInstance(webViewType);

		Rect webViewRect = new Rect(0, 0, 648, window.position.height);
		webViewType.GetMethod("InitWebView").Invoke(webView, new object[]{thisWindowGuiView, (int)webViewRect.x, (int)webViewRect.y, (int)webViewRect.width, (int)webViewRect.height, true});
		webViewType.GetMethod("LoadURL").Invoke(webView, new object[]{Url});
	}

	public static Type GetTypeFromAllAssemblies(string typeName) {
		Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
		foreach(Assembly assembly in assemblies) {
			Type[] types = assembly.GetTypes();
			foreach(Type type in types) {
				if(type.Name.Equals(typeName, StringComparison.CurrentCultureIgnoreCase) || type.Name.Contains('+' + typeName)) //+ check for inline classes
					return type;
			}
		}
		return null;
	}
}

