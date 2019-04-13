using System.Diagnostics;
using System.Text.RegularExpressions;
using Configs.Logs;
using UnityEditor;
using UnityEngine;
/// <summary>
/// ログ出力拡張メソッド
/// </summary>
public static class LogExtentions {

	/// <summary>
	/// アセットパス
	/// </summary>
	private static readonly string AssetPath = "Assets/Resources/Config/Log.asset";

	/// <summary>
	/// ログレベル
	/// </summary>
	private enum LogLevel {
		Debug,
		Warning,
		Error
	};

	private static void WriteLog( LogLevel logLevel , string message ) {

		// 2つ前のスタックからメソッド名とクラス名を取得
		StackFrame callerFrame = new StackFrame( 2 );
		string methodName = callerFrame.GetMethod().Name;
		string className = callerFrame.GetMethod().ReflectedType.FullName;

		ScriptableLogConfig config = AssetDatabase.LoadAssetAtPath( AssetPath , typeof( ScriptableLogConfig ) ) as ScriptableLogConfig;
		string processedMessage = $"[{className}.{methodName}]{message}";
		foreach( ScriptableLogConfig.Check check in config.CheckList ) {
			if( !check.IsValid ) {
				continue;
			}
			if(
				( !string.IsNullOrEmpty( check.RegexClass ) && Regex.IsMatch( className , check.RegexClass ) ) ||
				( !string.IsNullOrEmpty( check.RegexMethod ) && Regex.IsMatch( methodName , check.RegexMethod ) ) ||
				( !string.IsNullOrEmpty( check.RegexMessage ) && Regex.IsMatch( message , check.RegexMessage ) )
			) {
				switch( check.Type ) {
					case ScriptableLogConfig.Check.ActionType.Hide:
						return;
					case ScriptableLogConfig.Check.ActionType.Bold:
						processedMessage = $"<b>{processedMessage}</b>";
						break;
					case ScriptableLogConfig.Check.ActionType.Color:
						string rgb = "#" + ColorUtility.ToHtmlStringRGB( check.Color );
						processedMessage = $"<color={rgb}>{processedMessage}</color>";
						break;
					default:
						break;
				}
			}
		}

		// ログ出力
		switch( logLevel ) {
			case LogLevel.Debug:
				UnityEngine.Debug.Log( processedMessage );
				break;
			case LogLevel.Warning:
				UnityEngine.Debug.LogWarning( processedMessage );
				break;
			case LogLevel.Error:
				UnityEngine.Debug.LogError( processedMessage );
				break;
			default:
				break;
		};

	}

	/// <summary>
	/// デバッグログ出力
	/// </summary>
	/// <param name="obj">拡張オブジェクト</param>
	/// <param name="message">ログメッセージ</param>
	public static void LogDebug( this object obj , string message )
		=> WriteLog( LogLevel.Debug , message );

	/// <summary>
	/// 警告ログ出力
	/// </summary>
	/// <param name="obj">拡張オブジェクト</param>
	/// <param name="message">ログメッセージ</param>
	public static void LogWarning( this object obj , string message )
		=> WriteLog( LogLevel.Warning , message );

	/// <summary>
	/// エラーログ出力
	/// </summary>
	/// <param name="obj">拡張オブジェクト</param>
	/// <param name="message">ログメッセージ</param>
	public static void LogError( this object obj , string message )
		=> WriteLog( LogLevel.Error , message );

}
