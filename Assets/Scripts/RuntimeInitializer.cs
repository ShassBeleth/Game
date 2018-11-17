using UnityEngine;
using Repositories;
using System.Collections.Generic;
using Repositories.Models;
using Services.Scenes;

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

		// SceneService開始
		SceneService.GetInstance();

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
		List<Parameter> parameters = ParameterRepository.GetInstance().Rows;
		foreach( Parameter parameter in parameters ) {
			Logger.Warning( $"{parameter.id}:{parameter.name}." );
		}
		Logger.Debug( "End" );
	}

}
