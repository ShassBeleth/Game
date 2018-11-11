using UnityEngine;
using SceneManagers;
using Repositories;
using System.Collections.Generic;
using Repositories.Models;

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
		
		List<Body> bodies = BodyRepository.GetInstance().Rows;
		foreach( Body body in bodies ) {
			Logger.Warning( $"{body.id}:{body.name} , {body.ruby} , {body.flavor}." );
		}
		
		Logger.Debug( "End" );

	}

}
