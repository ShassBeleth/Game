using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Saves.Serializers {

	/// <summary>
	/// セーブデータの読み書き親クラス
	/// </summary>
	public abstract class SaveDataSerializerBase {

		/// <summary>
		/// フォルダパス
		/// </summary>
#if UNITY_EDITOR
		protected static string DirectoryPath = Directory.GetCurrentDirectory() + "/SaveData/";
#else
		protected static string DirectoryPath = Application.dataPath + "/SaveData/";
#endif

		/// <summary>
		/// セーブデータの読み込み
		/// </summary>
		/// <typeparam name="T">読み込んだセーブデータの型</typeparam>
		/// <param name="filePath">ファイルパス</param>
		/// <returns>読み込んだセーブデータモデル</returns>
		protected static T LoadSaveData<T>( string filePath ) where T : class {
			Logger.Debug( "Start" );
			Logger.Debug( $"File Path is {filePath}" );

			string jsonData = "";
			try {
				using( StreamReader streamReader = new StreamReader( DirectoryPath + filePath , Encoding.UTF8 ) ) {
					jsonData = streamReader.ReadToEnd();
					streamReader.Close();
				}
			}
			// 何かしらエラーがあった場合にはnullを返す
			catch( Exception e ) {
				Logger.Warning( $"Load File is Error : {e.Message}" );
				return null;
			}
			Logger.Debug( $"Json Data is {jsonData}" );

			// 何かしらエラーの影響で取得したjsonデータが空文字だった場合nullを返す
			if( "".Equals( jsonData ) ) {
				Logger.Warning( "Load File is Empty String." );
				return null;
			}

			// jsonデータをクラスに変換
			T data = JsonUtility.FromJson<T>( jsonData );
			
			Logger.Debug( "End" );
			return data;

		}
		
		/// <summary>
		/// セーブデータの書き込み
		/// </summary>
		/// <typeparam name="T">書き込むモデルの型</typeparam>
		/// <param name="filePath">ファイルパス</param>
		/// <param name="model">モデル</param>
		protected static void WriteSaveData<T>( string filePath , T model ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"File Path is {filePath}" );

			// モデルをjsonに変換
			string jsonData = JsonUtility.ToJson( model );
			Logger.Debug( $"Json Data is {jsonData}" );

			// ディレクトリが存在するか確認
			Logger.Debug( $"Directory Path is {DirectoryPath + filePath}" );
			string directoryName = Path.GetDirectoryName( DirectoryPath + filePath );
			Logger.Debug( $"Directory Name is {directoryName}" );
			if( !Directory.Exists( directoryName ) ) {
				Logger.Debug( "Directory Doesn't Exist." );
				Directory.CreateDirectory( directoryName );
			}

			// ファイルへ書き込み
			using(
				StreamWriter streamWriter = new StreamWriter(
					new FileStream( DirectoryPath + filePath , FileMode.Create ) ,
					Encoding.UTF8
				)
			) {
				streamWriter.Write( jsonData );
				streamWriter.Close();
			}

			Logger.Debug( "End" );

		}

		/// <summary>
		/// ファイルの削除
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		protected static void DeleteSaveData( string filePath ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"File Path is {filePath}" );

			File.Delete( DirectoryPath + filePath );

			Logger.Debug( "End" );
		}

	}

}
