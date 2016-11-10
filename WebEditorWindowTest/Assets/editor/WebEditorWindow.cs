using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

public class WebEditorWindow : EditorWindow
{
	////static string Url = "https://google.com";
	static string Url = "http://localhost:8080/accounts/create-form";

	private ScriptableObject webView;
	private object parent;

	private Type webViewType;

	private int m_RepeatedShow;
	private bool m_SyncingFocus;
	private bool m_IsOffline;

	private MethodInfo hideMethod;
	private MethodInfo showMethod;
	private MethodInfo setHostViewMethod;
	private MethodInfo setFocusMethod;

	[MenuItem ("Testing/Open web window")]
	public static WebEditorWindow Init(String title) {
		WebEditorWindow window = EditorWindow.GetWindow<WebEditorWindow>(title,typeof(SceneView)); //dock next to sceneview
		window.SetMinMaxSizes();
		window.Show();
		return window;
	}

	private void SetMinMaxSizes()
	{
		base.minSize = new Vector2(400f, 100f);
		base.maxSize = new Vector2(2048f, 2048f);
	}

	public void OnGUI()
	{
		Rect webViewRect = new Rect(0f, 0f, base.position.width, base.position.height);

		if(!this.webView) {
			this.InitWebView(webViewRect);
		}
		if (this.m_RepeatedShow-- > 0)
		{
			this.Refresh();
		}
		if (Event.current.type == EventType.Repaint)
		{
			//this f*cks it up
			//webViewType.GetMethod("SetSizeAndPosition").Invoke(webView, new object[]{(int)webViewRect.x, (int)webViewRect.y, (int)webViewRect.width, (int)webViewRect.height});
		}
	}

	public void Refresh()
	{
		Debug.LogError("Refresh");
		hideMethod.Invoke(webView,null);
		showMethod.Invoke(webView,null);
	}

	public void OnFocus()
	{
		this.SetFocus(true);
	}

	public void OnLostFocus()
	{
		this.SetFocus(false);
	}

	private void InitWebView(Rect webViewRect)
	{
		parent = typeof(EditorWindow).GetField("m_Parent", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);

		webViewType = GetTypeFromAllAssemblies("WebView");
		webView = ScriptableObject.CreateInstance(webViewType);

		webViewType.GetMethod("InitWebView").Invoke(webView, new object[]{parent, (int)webViewRect.x, (int)webViewRect.y, (int)webViewRect.width, (int)webViewRect.height, false});
		webViewType.GetMethod("LoadURL").Invoke(webView, new object[]{Url});
		webViewType.GetMethod("SetDelegateObject").Invoke(webView, new object[] {this});
		hideMethod = webViewType.GetMethod("Hide");
		showMethod = webViewType.GetMethod("Show");
		setHostViewMethod = webViewType.GetMethod("SetHostView");
		setFocusMethod = webViewType.GetMethod("SetFocus");

		this.SetFocus(true);

	}

	private void SetFocus(bool value)
	{
		if (this.m_SyncingFocus)
		{
			return;
		}
		this.m_SyncingFocus = true;
		if (this.webView)
		{
			if (value)
			{
				setHostViewMethod.Invoke(webView, new object[]{parent});
				showMethod.Invoke(webView,null);
				this.m_RepeatedShow = 5;
			}
			setFocusMethod.Invoke(webView, new object[]{value});
		}
		this.m_SyncingFocus = false;
	}

	public void OnBecameInvisible()
	{
		if (!this.webView)
		{
			return;
		}
		setHostViewMethod.Invoke(webView, new object[]{null});
	}

	public void OnDestroy()
	{
		UnityEngine.Object.DestroyImmediate(this.webView);
	}

	public void OnLoadError(string url)
	{
		Debug.LogError("OnLoadError");
		if (!this.webView)
		{
			return;
		}
		if (this.m_IsOffline)
		{
			Debug.LogErrorFormat("Unexpected error: Failed to load offline Asset Store page (url={0})", new object[]
				{
					url
				});
			return;
		}
		this.m_IsOffline = true;
		//this.webView.LoadFile(AssetStoreUtils.GetOfflinePath());
	}

	public void OnDownloadProgress(string id, string message, ulong bytes, ulong total)
	{
		Debug.LogError("OnDownloadProgress");
		//		this.InvokeJSMethod("document.AssetStore.pkgs", "OnDownloadProgress", new object[]
		//			{
		//				id,
		//				message,
		//				bytes,
		//				total
		//			});
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