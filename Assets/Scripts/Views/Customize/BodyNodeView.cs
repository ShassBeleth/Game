﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Customize {

	/// <summary>
	/// 素体一覧のNodeにつくView
	/// </summary>
	public class BodyNodeView : MonoBehaviour {

		/// <summary>
		/// Id
		/// </summary>
		private int id;

		/// <summary>
		/// テキスト
		/// </summary>
		public Text Text;

		/// <summary>
		/// 決定ボタン
		/// </summary>
		public Button DecisionButton;

		public void SetText( string text ) {
			Logger.Debug( "Start" );
			this.Text.text = text;
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 決定ボタン押下時イベントを設定する
		/// </summary>
		/// <param name="action">決定ボタン押下時イベント</param>
		public void SetOnClickDecisionButtonEventHandler( Action action ) {
			Logger.Debug( "Start" );
			this.DecisionButton.onClick.AddListener( () => action?.Invoke() );
			Logger.Debug( "End" );
		}

	}

}