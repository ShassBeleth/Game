using System;
using UnityEngine;

namespace Views.Ranking {

	/// <summary>
	/// ランキングView
	/// </summary>
	public class RankingView : MonoBehaviour{

		/// <summary>
		/// 戻るボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickBackButtonEventHandler { set; get; }

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		public void OnClickBackButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickBackButtonEventHandler?.Invoke();
			this.LogDebug( "End" );
		}

	}

}
