using System;
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
		/// 決定ボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickDecisionButtonEventHandler { set; get; }

		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		public void OnClickDecisionButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickDecisionButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}

		/// <summary>
		/// テキストを設定
		/// </summary>
		/// <param name="text">テキスト</param>
		public void SetText( string text ) {
			Logger.Debug( "Start" );
			this.text.text = text;
			Logger.Debug( "End" );
		}
		
	}

}
