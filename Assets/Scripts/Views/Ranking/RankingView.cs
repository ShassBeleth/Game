using System;
using UniRx;
using UnityEngine;

namespace Views.Ranking {

	/// <summary>
	/// ランキングView
	/// </summary>
	public class RankingView : MonoBehaviour{

		/// <summary>
		/// 戻るボタン押下時イベントSubject
		/// </summary>
		private readonly Subject<Unit> OnClickedBackButtonSubject = new Subject<Unit>();

		/// <summary>
		/// 戻るボタン押下時イベント購読
		/// </summary>
		public IObservable<Unit> OnClickedBackButton => this.OnClickedBackButtonSubject;

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		public void OnClickBackButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickedBackButtonSubject.OnNext( Unit.Default );
			this.LogDebug( "End" );
		}

	}

}
