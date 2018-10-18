using UnityEngine;
using SceneManagers;

/// <summary>
/// アプリ起動時初期化
/// </summary>
public class RuntimeInitializer : MonoBehaviour {

	/// <summary>
	/// アプリ起動時初期化
	/// </summary>
	[RuntimeInitializeOnLoadMethod]
	public static void Initialize() {
		Logger.Debug( "Start" );

		// SceneManager開始
		SceneManager.GetInstance();
		
		Logger.Debug( "End" );

	}

}
