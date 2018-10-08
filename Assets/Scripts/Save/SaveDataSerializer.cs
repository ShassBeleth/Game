using System;
using System.IO;
using UnityEngine;

namespace Save {

	/// <summary>
	/// セーブデータ読み書きクラス
	/// </summary>
	public static class SaveDataSerializer {

		/// <summary>
		/// 一人プレイセーブデータの読み込み
		/// </summary>
		/// <param name="id">セーブデータID</param>
		public static SinglePlaySaveDataModel LoadSinglePlaySaveData( int id ) {
			Logger.Debug( "Start" );


			// ローカルからjsonファイルを読み込み
			string filePath = Application.dataPath + "/ActionGame/SinglePlayData/" + id + ".json";
			Logger.Debug( "File Path is " + filePath );
			string jsonData = "";
			try {
				using( StreamReader streamReader = new StreamReader( filePath ) ) {
					jsonData = streamReader.ReadToEnd();
					streamReader.Close();
				}
			}
			// 何かしらエラーがあった場合にはnullを返す
			catch( Exception e ) {
				Logger.Warning( "Load File is Error : " + e.Message );
				return null;
			}
			Logger.Debug( "Json Data is " + jsonData );

			// 何かしらエラーの影響で取得したjsonデータが空文字だった場合nullを返す
			if( "".Equals( jsonData ) ) {
				Logger.Warning( "Load File is Empty String." );
				return null;
			}

			// jsonデータをクラスに変換
			SinglePlaySaveDataModel data = JsonUtility.FromJson<SinglePlaySaveDataModel>( jsonData );
			
			Logger.Debug( "End" );
			return data;

		}
		
		/// <summary>
		/// 一人プレイセーブデータの書き込み
		/// </summary>
		/// <param name="id">セーブデータID</param>
		/// <param name="singlePlaySaveDataModel">一人プレイセーブデータモデル</param>
		public static void WriteSinglePlaySaveData( int id , SinglePlaySaveDataModel singlePlaySaveDataModel ) {



		}

	}

}
