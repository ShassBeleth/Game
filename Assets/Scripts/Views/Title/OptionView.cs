using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Views.Title {

	/// <summary>
	/// オプションView
	/// </summary>
	public class OptionView : MonoBehaviour {

		/// <summary>
		/// event system
		/// </summary>
		public EventSystem eventSystem;

		/// <summary>
		/// 設定値
		/// </summary>
		public class OptionValue {

		}

		#region 戻るボタンについて

		/// <summary>
		/// 戻るボタンGameObject
		/// </summary>
		public GameObject backGameObject;

		/// <summary>
		/// 戻るボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickBackButtonEventHandler { set; get; }

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		public void OnClickBackButonEvent() {
			Logger.Debug( "Start" );
			this.OnClickBackButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}

		#endregion

		/// <summary>
		/// 強制的に選択肢を設定する
		/// </summary>
		/// <param name="selectable">選択肢</param>
		public void SetSelectedGameObject( GameObject selectable ) {
			Logger.Debug( "Start" );
			this.eventSystem.SetSelectedGameObject( selectable );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 設定値を返す
		/// </summary>
		/// <returns>設定値</returns>
		public OptionValue GetOptionValue() {
			Logger.Debug( "Start" );
			Logger.Debug( "End" );
			return new OptionValue();
		}

		/// <summary>
		/// 設定値を設定する
		/// </summary>
		/// <param name="optionValue">設定値</param>
		public void SetOptionValue( OptionValue optionValue ) { 
			Logger.Debug( "Start" );
			Logger.Debug( "End" );
		}

	}

}