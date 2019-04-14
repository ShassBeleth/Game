using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Views.ChapterSelect {

	/// <summary>
	/// スクロールNodeにつくView
	/// </summary>
	public class ScrollChapterNodeView : MonoBehaviour {

		/// <summary>
		/// Id
		/// </summary>
		public int Id { set; get; }

		/// <summary>
		/// テキスト
		/// </summary>
		public Text text;

		/// <summary>
		/// ラインの座標
		/// </summary>
		public Transform lineTransform;

		/// <summary>
		/// 決定ボタン押下時イベントSubject
		/// </summary>
		public Subject<Unit> OnClickedDecisionButtonSubject = new Subject<Unit>();

		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		public void OnClickDecisionButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickedDecisionButtonSubject.OnNext( Unit.Default );
			this.LogDebug( "End" );
		}

		/// <summary>
		/// テキストを設定
		/// </summary>
		/// <param name="text">テキスト</param>
		public void SetText( string text ) {
			this.LogDebug( "Start" );
			this.text.text = text;
			this.LogDebug( "End" );
		}
		
	}

}
