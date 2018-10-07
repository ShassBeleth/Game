using System;
using UnityEngine;

namespace Views.Title {

	/// <summary>
	/// オプションView
	/// </summary>
	public class OptionView : MonoBehaviour {

		/// <summary>
		/// 設定値
		/// </summary>
		public class OptionValue {

		}

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