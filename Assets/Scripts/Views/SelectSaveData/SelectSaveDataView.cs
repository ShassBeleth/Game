using System;
using System.Collections.Generic;
using UnityEngine;

namespace Views.SelectSaveData {

	/// <summary>
	/// セーブデータ選択View
	/// </summary>
	public class SelectSaveDataView : MonoBehaviour {

		/// <summary>
		/// 表示するセーブデータ
		/// </summary>
		public class SaveData {

			/// <summary>
			/// ID
			/// </summary>
			public int Id { set; get; }

			/// <summary>
			/// ユーザ名
			/// </summary>
			public string userName { set; get; }

			/// <summary>
			/// 最終更新日
			/// </summary>
			public DateTime latestUpdateDataTime { set; get; }
			
		}
		
		/// <summary>
		/// 戻るボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickBackButtonEventHandler { set; get; }

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		public void OnClickBackButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickBackButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}
		
	}

}