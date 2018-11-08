using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Repositories {
	
	/// <summary>
	/// 素体一覧
	/// </summary>
	[Serializable]
	public class Bodies {

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<Body> bodies;

		public int id;

	}

	/// <summary>
	/// 素体
	/// </summary>
	[Serializable]
	public class Body {

		/// <summary>
		/// 素体ID
		/// </summary>
		public int id;

		/// <summary>
		/// 素体名
		/// </summary>
		public string name;

		/// <summary>
		/// 素体名ルビ
		/// </summary>
		public string ruby;

		/// <summary>
		/// フレーバーテキスト
		/// </summary>
		public string flavor;

	}


	/// <summary>
	/// 素体リポジトリ
	/// </summary>
	public class BodyRepository {

		/// <summary>
		/// フォルダパス
		/// </summary>
#if UNITY_EDITOR
		protected static string DirectoryPath = Directory.GetCurrentDirectory() + "/Data/";
#else
		protected static string DirectoryPath = Application.dataPath + "/Data/";
#endif

		/// <summary>
		/// インスタンス
		/// </summary>
		private static BodyRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static BodyRepository GetInstance() {
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new BodyRepository();
			}
			Logger.Debug( "End" );
			return Instance;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private BodyRepository() {
			Logger.Debug( "Start" );

			string filePath = "bodies.json";
			Logger.Debug( $"File Path is {filePath}" );

			string jsonData = "";
			try {
				using(
					StreamReader streamReader
						= new StreamReader( DirectoryPath + filePath , Encoding.UTF8 )
					) 
			{
					jsonData = streamReader.ReadToEnd();
					streamReader.Close();
				}
			}
			// 何かしらエラーがあった場合にはnullを返す
			catch( Exception e ) {
				Logger.Warning( $"Load File is Error : {e.Message}" );
				return;
			}
			Logger.Debug( $"Json Data is {jsonData}" );

			// 何かしらエラーの影響で取得したjsonデータが空文字だった場合nullを返す
			if( "".Equals( jsonData ) ) {
				Logger.Warning( "Load File is Empty String." );
				return;
			}

			// jsonデータをクラスに変換
			this.Rows = JsonUtility.FromJson<Bodies>( jsonData ).bodies;

			Logger.Debug( "End" );

		}

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<Body> Rows {
			private set;
			get;
		}

	}

}
