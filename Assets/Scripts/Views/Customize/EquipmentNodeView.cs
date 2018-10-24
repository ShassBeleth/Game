using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Customize {

	/// <summary>
	/// 装備一覧のNodeにつくView
	/// </summary>
	public class EquipmentNodeView : MonoBehaviour {

		/// <summary>
		/// Id
		/// </summary>
		private int id;

		/// <summary>
		/// 決定ボタン
		/// </summary>
		public Button DecisionButton;

		/// <summary>
		/// 決定ボタン押下時イベントを設定する
		/// </summary>
		/// <param name="action">決定ボタン押下時イベント</param>
		public void SetOnClickDecisionButtonEventHandler( Action action ) {
			Logger.Debug( "Start" );
			this.DecisionButton.onClick.AddListener( () => action?.Invoke() );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// テキストの設定
		/// </summary>
		/// <param name="text">テキスト</param>
		public void SetText( string text ) {
			Logger.Debug( "Start" );
			this.transform.GetChild( 0 ).GetComponent<Text>().text = text ?? "None Name";
			Logger.Debug( "End" );
		}

	}

}
