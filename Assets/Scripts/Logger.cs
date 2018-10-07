/// <summary>
/// ログ出力クラス
/// </summary>
public static class Logger {

	/// <summary>
	/// デバッグログを出力するかどうか
	/// </summary>
	private static bool HasOutputDebugLog = true;

	/// <summary>
	/// 警告ログを出力するかどうか
	/// </summary>
	private static bool HasOutputWarningLog = true;

	/// <summary>
	/// エラーログを出力するかどうか
	/// </summary>
	private static bool HasOutputErrorLog = true;

	/// <summary>
	/// Viewのログを出力するかどうか
	/// </summary>
	private static bool HasOutputViewLog = true;

	/// <summary>
	/// Presenterのログを出力するかどうか
	/// </summary>
	private static bool HasOutputPresenterLog = true;

	/// <summary>
	/// ViewとPresenterのログを出力するかどうか
	/// </summary>
	private static bool HasOutputOtherLog = true;

	/// <summary>
	/// ログレベル
	/// </summary>
	private enum LogLevel {
		Debug ,
		Warning ,
		Error
	};
	
	/// <summary>
	/// ログ出力するかどうかの判定
	/// </summary>
	/// <param name="logLevel">ログレベル</param>
	/// <param name="className">クラス名</param>
	/// <returns>ログ出力するかどうか</returns>
	private static bool HasOutputLog( LogLevel logLevel , string className ) {

		// ログレベル毎に出力フラグを見て判定
		if( logLevel == LogLevel.Debug && !HasOutputDebugLog ) {
			return false;
		}
		if( logLevel == LogLevel.Warning && !HasOutputWarningLog ) {
			return false;
		}
		if( logLevel == LogLevel.Error && !HasOutputErrorLog ) {
			return false;
		}

		// クラス名毎に出力フラグを見て判定
		if( className.EndsWith( "View" ) && !HasOutputViewLog ) {
			return false;
		}
		if( className.EndsWith( "Presenter" ) && !HasOutputPresenterLog ) {
			return false;
		}
		if( !className.EndsWith( "View" ) && !className.EndsWith( "Presenter" ) && !HasOutputOtherLog ) {
			return false;
		}

		return true;

	}

	/// <summary>
	/// ログ出力元からメッセージを加工
	/// </summary>
	/// <param name="className">クラス名</param>
	/// <param name="methodName">メソッド名</param>
	/// <param name="message">メッセージ</param>
	/// <returns>加工されたログ</returns>
	private static string ProcessMessage(
		string className ,
		string methodName ,
		string message
	) {

		string processedMessage = "[" + className + "." + methodName + "]" + message;
		if( className.EndsWith( "View" ) ) {
			return "<color=blue>" + processedMessage + "</color>";
		}
		else if( className.EndsWith( "Presenter" ) ) {
			return "<color=green>" + processedMessage + "</color>";
		}

		return processedMessage;

	}

	/// <summary>
	/// ログレベルに応じてログを出力する
	/// </summary>
	/// <param name="logLevel">ログレベル</param>
	/// <param name="message">メッセージ</param>
	private static void OutputLog( LogLevel logLevel , string message ) {
		switch( logLevel ) {
			case LogLevel.Debug:
				UnityEngine.Debug.Log( message );
				break;
			case LogLevel.Warning:
				UnityEngine.Debug.LogWarning( message );
				break;
			case LogLevel.Error:
				UnityEngine.Debug.LogError( message );
				break;
		}
	}

	/// <summary>
	/// ログ出力
	/// </summary>
	/// <param name="logLevel">ログレベル</param>
	/// <param name="message">メッセージ</param>
	private static void WriteLog( LogLevel logLevel , string message ) {

		// 2つ前のスタックからメソッド名とクラス名を取得
		System.Diagnostics.StackFrame callerFrame = new System.Diagnostics.StackFrame( 2 );
		string methodName = callerFrame.GetMethod().Name;
		string className = callerFrame.GetMethod().ReflectedType.FullName;

		if( !HasOutputLog( logLevel , className ) ) {
			return;
		}

		message = ProcessMessage( className , methodName , message );

		OutputLog( logLevel , message );

	}

	/// <summary>
	/// デバッグログ出力
	/// </summary>
	/// <param name="message">メッセージ</param>
	public static void Debug( string message )
		=> WriteLog( LogLevel.Debug , message );

	/// <summary>
	/// 警告ログ出力
	/// </summary>
	/// <param name="message">メッセージ</param>
	public static void Warning( string message )
		=> WriteLog( LogLevel.Warning , message );

	/// <summary>
	/// エラーログ出力
	/// </summary>
	/// <param name="message">メッセージ</param>
	public static void Error( string message )
		=> WriteLog( LogLevel.Error , message );
	
}