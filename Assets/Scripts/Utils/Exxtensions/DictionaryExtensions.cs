using System.Collections.Generic;

namespace Utils.Exxtensions {

	/// <summary>
	/// Dictionaryの拡張クラス
	/// </summary>
	public static class DictionaryExtensions {

		/// <summary>
		/// Valueの取得
		/// Keyが存在しない場合にデフォルト値を指定できる
		/// </summary>
		/// <typeparam name="TK">キーの型</typeparam>
		/// <typeparam name="TV">値の型</typeparam>
		/// <param name="dictionary">ディクショナリ</param>
		/// <param name="key">キー</param>
		/// <param name="defaultValue">デフォルト値</param>
		/// <returns>値　キーが存在しない場合は指定したデフォルト値かValueのデフォルト値</returns>
		public static TV GetOrDefault<TK, TV>(
			this Dictionary<TK , TV> dictionary ,
			TK key ,
			TV defaultValue = default( TV ) 
		) {
			TV result;
			return dictionary.TryGetValue( key , out result ) ? result : defaultValue;
		}

	}

}
