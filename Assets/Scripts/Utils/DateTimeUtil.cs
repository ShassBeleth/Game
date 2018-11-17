using System;
using System.Globalization;

namespace Utils {

	/// <summary>
	/// 日付に関するUtil
	/// </summary>
	public static class DateTimeUtil {
		
		/// <summary>
		/// 日付フォーマット
		/// </summary>
		private static readonly string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";

		/// <summary>
		/// 現在日時文字列を返す
		/// </summary>
		/// <returns>現在日時文字列</returns>
		public static string GetNowString() {
			Logger.Debug( "Start" );
			string nowString = DateTime.Now.ToString( DateTimeFormat );
			Logger.Debug( $"Now is {nowString}." );
			Logger.Debug( "End" );
			return nowString;
		}

		/// <summary>
		/// 日付文字列からDateTimeを返す
		/// </summary>
		/// <param name="str">日付文字列</param>
		/// <returns>DateTime</returns>
		public static DateTime ConvertStringToDateTime( string str ) {
			Logger.Debug( "Start" );

			if( str == null ) {
				Logger.Debug( "Str is Null." );
				return new DateTime();
			}

			Logger.Debug( $"Str is {str}." );

			DateTime dateTime;
			try {
				dateTime = DateTime.ParseExact(
					str ,
					DateTimeFormat ,
					DateTimeFormatInfo.InvariantInfo ,
					DateTimeStyles.NoCurrentDateDefault
				);
			}
			catch( Exception ) {
				Logger.Warning( "Convert Error." );
				Logger.Debug( "End" );
				return new DateTime();
			}
			Logger.Debug( "End" );
			return dateTime;
		}

		/// <summary>
		/// DateTimeから日付文字列を返す
		/// </summary>
		/// <param name="dateTime">DateTime</param>
		/// <returns>日付文字列</returns>
		public static string ConvertDateTimeToString( DateTime dateTime ) {
			Logger.Debug( "Start" );
			string str = dateTime.ToString( DateTimeFormat );
			Logger.Debug( $"Date Time is {str}." );
			Logger.Debug( "End" );
			return str;
		}

	}

}
