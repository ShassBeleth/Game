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

		// 各種リポジトリの起動
		StartRepository();

		Logger.Debug( "End" );

	}

	/// <summary>
	/// 各種リポジトリの起動
	/// </summary>
	private static void StartRepository() {
		Logger.Debug( "Start" );

		BodyRepository.GetInstance();
		EquipablePlaceRepository.GetInstance();
		SaveRepository.GetInstance();

		Logger.Debug( "End" );
	}

}
