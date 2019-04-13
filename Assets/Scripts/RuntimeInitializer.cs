using UnityEngine;
using Repositories;
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
		
		// SceneService開始
		SceneService.GetInstance();

		// 各種リポジトリの起動
		StartRepository();

	}

	/// <summary>
	/// 各種リポジトリの起動
	/// </summary>
	private static void StartRepository() {

		BodyEffectRepository.GetInstance();
		BodyEquipablePlaceRepository.GetInstance();
		BodyFreeSquareRepository.GetInstance();
		BodyRepository.GetInstance();
		ChapterClearStatusRepository.GetInstance();
		ChapterRepository.GetInstance();
		DesignatedPlaceToEquipmentByEffectRepository.GetInstance();
		EquipablePlaceRepository.GetInstance();
		EquipmentEffectRepository.GetInstance();
		EquipmentEquipableInEquipablePlaceRepository.GetInstance();
		EquipmentFreeSquareRepository.GetInstance();
		EquipmentRepository.GetInstance();
		EquippedWhenIncreasingEquipablePlaceRepository.GetInstance();
		EquippedWhenUnequippingEquipablePlaceRepository.GetInstance();
		HavingBodyRepository.GetInstance();
		HavingEquipmentRepository.GetInstance();
		HavingParameterChipRepository.GetInstance();
		ParameterChipEffectRepository.GetInstance();
		ParameterChipRepository.GetInstance();
		ParameterChipSquareRepository.GetInstance();
		ParameterRepository.GetInstance();
		SaveRepository.GetInstance();

	}

}