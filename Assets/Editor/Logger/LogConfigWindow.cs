using Configs.Logs;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.Logger {

	/// <summary>
	/// ログ設定用ウィンドウ
	/// </summary>
	public class LogConfigWindow : EditorWindow {

		/// <summary>
		/// アセットパス
		/// </summary>
		private static readonly string AssetPath = "Assets/Resources/Config/Log.asset";

		/// <summary>
		/// ログ設定情報
		/// </summary>
		private ScriptableLogConfig config;

		/// <summary>
		/// ログ設定情報のうちウィンドウで設定するチェックリスト
		/// </summary>
		[SerializeField]
		private List<ScriptableLogConfig.Check> checkList = new List<ScriptableLogConfig.Check>();

		/// <summary>
		/// ログ設定情報のうちウィンドウで設定するチェックリスト
		/// </summary>
		public List<ScriptableLogConfig.Check> CheckList {
			get => this.checkList;
			set => this.checkList = value;
		}

		/// <summary>
		/// ウィンドウの表示
		/// </summary>
		[MenuItem( "Config/Log" )]
		private static void Create() => GetWindow<LogConfigWindow>( "Log Config" );

		/// <summary>
		/// ウィンドウの描画
		/// </summary>
		private void OnGUI() {

			// 設定ファイルが読み込まれていなければ取得
			if( this.config is null ) {
				string directory = Path.GetDirectoryName( AssetPath );
				if( !Directory.Exists( directory ) ) {
					Directory.CreateDirectory( directory );
				}
				// TODO Log.assetがなかった場合に生成
				// this.config = ScriptableObject.CreateInstance<ScriptableLogConfig>();
				// AssetDatabase.CreateAsset( this.config , AssetPath );
				this.config = AssetDatabase.LoadAssetAtPath( AssetPath , typeof( ScriptableLogConfig ) ) as ScriptableLogConfig;
				this.CheckList = this.config.CheckList;
			}

			// ウィンドウに描画
			SerializedObject serializedObject = new SerializedObject( this );
			serializedObject.Update();

			EditorGUILayout.PropertyField( serializedObject.FindProperty( "checkList" ) , true );

			// ウィンドウで設定した情報をassetファイルに反映
			this.config.CheckList = this.checkList;
			serializedObject.ApplyModifiedProperties();

		}

	}

}