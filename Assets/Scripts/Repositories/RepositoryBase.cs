using System;
using System.IO;
using System.Security;
using System.Text;
using UnityEngine;

namespace Repositories {

	/// <summary>
	/// リポジトリ共通部
	/// </summary>
	public abstract class RepositoryBase {

		/// <summary>
		/// フォルダパス
		/// </summary>
#if UNITY_EDITOR
		protected readonly string DirectoryPath = Directory.GetCurrentDirectory() + "/Data/";
#else
		protected static readonly string DirectoryPath = Application.dataPath + "/Data/";
#endif
		
		/// <summary>
		/// 読み込み
		/// </summary>
		/// <typeparam name="T">読み込むデータの型</typeparam>
		/// <param name="filePath">ファイルパス</param>
		/// <returns>読み込んだデータモデル</returns>
		protected T Load<T>( string filePath ) where T : class {
			Logger.Debug( "Start" );
			Logger.Debug( $"File Path is {filePath}" );

			string jsonData = "";
			try {
				jsonData = File.ReadAllText( Path.Combine( this.DirectoryPath , filePath ) , Encoding.UTF8 );
			}
			catch( ArgumentNullException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( ArgumentException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( PathTooLongException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( DirectoryNotFoundException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( FileNotFoundException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( IOException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( UnauthorizedAccessException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( NotSupportedException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( SecurityException e ) {
				Logger.Error( $"{e.Message}." );
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
		/// 書き込み
		/// </summary>
		/// <typeparam name="T">書き込むモデルの型</typeparam>
		/// <param name="filePath">ファイルパス</param>
		/// <param name="model">モデル</param>
		protected void Write<T>( string filePath , T model ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"File Path is {filePath}" );

			// モデルをjsonに変換
			string jsonData = JsonUtility.ToJson( model );
			Logger.Debug( $"Json Data is {jsonData}" );

			// ディレクトリが存在するか確認
			Logger.Debug( $"Directory Path is {this.DirectoryPath}." );
			Logger.Debug( $"File Path is {filePath}." );
			string directoryName = Path.GetDirectoryName( Path.Combine( this.DirectoryPath , filePath ) );
			Logger.Debug( $"Directory Name is {directoryName}" );
			if( !Directory.Exists( directoryName ) ) {
				Logger.Debug( "Directory Doesn't Exist." );
				Directory.CreateDirectory( directoryName );
			}

			// ファイルへ書き込み
			try {
				File.WriteAllText( Path.Combine( this.DirectoryPath , filePath ) , jsonData );
			}
			catch( ArgumentNullException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( ArgumentException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( PathTooLongException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( DirectoryNotFoundException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( IOException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( UnauthorizedAccessException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( NotSupportedException e ) {
				Logger.Error( $"{e.Message}." );
			}
			catch( SecurityException e ) {
				Logger.Error( $"{e.Message}." );
			}

			Logger.Debug( "End" );
		}

		/// <summary>
		/// ファイルの削除
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		protected void Delete( string filePath ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"File Path is {filePath}" );

			File.Delete( this.DirectoryPath + filePath );

			Logger.Debug( "End" );
		}

	}

}
