using System.Collections.Generic;
using UniRx;

namespace Models {

	/// <summary>
	/// セーブデータ選択Model
	/// </summary>
	public class SelectSaveDataModel {

		/// <summary>
		/// 選択されたセーブデータのIndex
		/// </summary>
		public ReactiveProperty<int?> selectedSaveDataIndex;

		/// <summary>
		/// セーブデータ
		/// </summary>
		public List<SaveDataModel> saveData;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="selectedSaveDataIndex">選択されたセーブデータのIndex</param>
		/// <param name="saveData">セーブデータ</param>
		public SelectSaveDataModel( int? selectedSaveDataIndex , List<SaveDataModel> saveData ) {
			Logger.Debug( "Start" );
			this.selectedSaveDataIndex = new ReactiveProperty<int?>( selectedSaveDataIndex );
			Logger.Warning( $"Selected Save Data Index is {this.selectedSaveDataIndex}" );
			this.saveData = saveData;
			Logger.Debug( "End" );
		}

	}

}
