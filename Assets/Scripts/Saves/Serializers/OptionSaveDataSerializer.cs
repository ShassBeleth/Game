using SavesTemp.Models;

namespace SavesTemp.Serializers {

	public class OptionSaveDataSerializer : SaveDataSerializerBase{

		/// <summary>
		/// ファイルパス
		/// </summary>
		private static string FilePath = "option.json";

		/// <summary>
		/// オプションセーブデータの読み込み
		/// </summary>
		public static OptionSaveDataModel LoadOptionSaveData() {
			Logger.Debug( "Start" );
			OptionSaveDataModel data = SaveDataSerializerBase.LoadSaveData<OptionSaveDataModel>( 
				FilePath 
			);
			Logger.Debug( "End" );
			return data;

		}

		/// <summary>
		/// オプションセーブデータの書き込み
		/// </summary>
		/// <param name="optionSaveDataModel">オプションセーブデータモデル</param>
		public static void WriteOptionSaveData( OptionSaveDataModel optionSaveDataModel ) {
			Logger.Debug( "Start" );
			SaveDataSerializerBase.WriteSaveData<OptionSaveDataModel>( 
				FilePath , 
				optionSaveDataModel 
			);
			Logger.Debug( "End" );
		}

		/// <summary>
		/// オプションセーブデータの削除
		/// </summary>
		public static void DeleteOptionSaveData() {
			Logger.Debug( "Start" );
			SaveDataSerializerBase.DeleteSaveData( FilePath );
			Logger.Debug( "End" );
		}

	}

}
