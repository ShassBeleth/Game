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
		
		/// <summary>
		/// セーブデータの描画
		/// </summary>
		/// <param name="saveDataList">セーブデータのリスト</param>
		public void ShowSaveData( List<SaveData> saveDataList ) {
			Logger.Debug( "Start" );
			for( int i = 0 ; i < saveDataList.Count ; i++ ) {
				Logger.Debug( "saveDataList[" + i + "]" );
				Logger.Debug( "Id is " + saveDataList[ i ].Id );
				Logger.Debug( "User Name is " + saveDataList[ i ].userName );
				Logger.Debug( "Latest Update Date Time is " + saveDataList[ i ].latestUpdateDataTime.ToString( "yyyy/MM/dd hh:mm:ss" ) );
			}
			Logger.Debug( "End" );
		}

		#region TODO セーブ機能作るまでの仮のボタン
		public Action OnClickTempButtonEventHandler { set; get; }
		public void OnClickTempButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickTempButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}
		#endregion

	}

}