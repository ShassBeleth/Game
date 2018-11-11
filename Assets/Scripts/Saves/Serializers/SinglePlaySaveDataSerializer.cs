using SavesTemp.Models;

namespace SavesTemp.Serializers {

	/// <summary>
	/// 一人プレイセーブデータ読み書きクラス
	/// </summary>
	public class SinglePlaySaveDataSerializer : SaveDataSerializerBase {

		/// <summary>
		/// 一人プレイセーブデータのディレクトリパス
		/// </summary>
		private new static string DirectoryPath = "SinglePlayData/";

		/// <summary>
		/// 一人プレイセーブデータの読み込み
		/// </summary>
		/// <param name="id">セーブデータID</param>
		public static SinglePlaySaveDataModel LoadSinglePlaySaveData( int id ) {
			Logger.Debug( "Start" );
			
			string filePath = DirectoryPath + id + ".json";
			Logger.Debug( $"File Path is {filePath}" );

			SinglePlaySaveDataModel data = SaveDataSerializerBase.LoadSaveData<SinglePlaySaveDataModel>( filePath );
			
			Logger.Debug( "End" );
			return data;

		}
		
		/// <summary>
		/// 一人プレイセーブデータの書き込み
		/// </summary>
		/// <param name="id">セーブデータID</param>
		/// <param name="singlePlaySaveDataModel">一人プレイセーブデータモデル</param>
		public static void WriteSinglePlaySaveData(
			int id , 
			SinglePlaySaveDataModel singlePlaySaveDataModel 
		) {
			Logger.Debug( "Start" );

			Logger.Debug( $"Id is {singlePlaySaveDataModel.id}" );
			Logger.Debug( $"User Name is {singlePlaySaveDataModel.userName}" );
			
			string filePath = DirectoryPath + id + ".json";
			Logger.Debug( $"File Path is {filePath}" );

			SaveDataSerializerBase.WriteSaveData<SinglePlaySaveDataModel>(
				filePath , 
				singlePlaySaveDataModel 
			);
			
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 一人プレイセーブデータの削除
		/// </summary>
		/// <param name="id"></param>
		public static void DeleteSinglePlaySaveData( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Id is {id}" );

			string filePath = DirectoryPath + id + ".json";
			Logger.Debug( $"File Path is {filePath}" );

			SaveDataSerializerBase.DeleteSaveData( filePath );

			Logger.Debug( "End" );
		}

	}

}
