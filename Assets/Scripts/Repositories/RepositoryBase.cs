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
			this.LogDebug( "Start" );
			this.LogDebug( $"File Path is {filePath}" );

			string jsonData = "";
			try {
				jsonData = File.ReadAllText( Path.Combine( this.DirectoryPath , filePath ) , Encoding.UTF8 );
			}
			catch( ArgumentNullException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( ArgumentException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( PathTooLongException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( DirectoryNotFoundException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( FileNotFoundException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( IOException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( UnauthorizedAccessException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( NotSupportedException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( SecurityException e ) {
				this.LogError( $"{e.Message}." );
			}
			
			this.LogDebug( $"Json Data is {jsonData}" );

			// 何かしらエラーの影響で取得したjsonデータが空文字だった場合nullを返す
			if( "".Equals( jsonData ) ) {
				this.LogWarning( "Load File is Empty String." );
				return null;
			}

			// jsonデータをクラスに変換
			T data = JsonUtility.FromJson<T>( jsonData );

			this.LogDebug( "End" );
			return data;
		}

		/// <summary>
		/// 書き込み
		/// </summary>
		/// <typeparam name="T">書き込むモデルの型</typeparam>
		/// <param name="filePath">ファイルパス</param>
		/// <param name="model">モデル</param>
		protected void Write<T>( string filePath , T model ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"File Path is {filePath}" );

			// モデルをjsonに変換
			string jsonData = JsonUtility.ToJson( model );
			this.LogDebug( $"Json Data is {jsonData}" );

			// ディレクトリが存在するか確認
			this.LogDebug( $"Directory Path is {this.DirectoryPath}." );
			this.LogDebug( $"File Path is {filePath}." );
			string directoryName = Path.GetDirectoryName( Path.Combine( this.DirectoryPath , filePath ) );
			this.LogDebug( $"Directory Name is {directoryName}" );
			if( !Directory.Exists( directoryName ) ) {
				this.LogDebug( "Directory Doesn't Exist." );
				Directory.CreateDirectory( directoryName );
			}

			// ファイルへ書き込み
			try {
				File.WriteAllText( Path.Combine( this.DirectoryPath , filePath ) , jsonData );
			}
			catch( ArgumentNullException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( ArgumentException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( PathTooLongException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( DirectoryNotFoundException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( IOException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( UnauthorizedAccessException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( NotSupportedException e ) {
				this.LogError( $"{e.Message}." );
			}
			catch( SecurityException e ) {
				this.LogError( $"{e.Message}." );
			}

			this.LogDebug( "End" );
		}

		/// <summary>
		/// ファイルの削除
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		protected void Delete( string filePath ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"File Path is {filePath}" );

			File.Delete( this.DirectoryPath + filePath );

			this.LogDebug( "End" );
		}

	}

}
