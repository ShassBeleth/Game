using System;
using UnityEngine;

namespace Views.ChapterSelect {

	/// <summary>
	/// チャプター選択View
	/// </summary>
	public class ChapterSelectView : MonoBehaviour {

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

		#region TODO 仮
		public Action OnClickTempButtonEventHandler { set; get; }
		public void OnClickTempButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickTempButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}
		#endregion
	}

}