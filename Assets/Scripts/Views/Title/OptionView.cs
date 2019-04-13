﻿using UnityEngine;
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
			
			/// <summary>
			/// 垂直方向のカメラ移動を反転させるかどうか
			/// </summary>
			public bool IsReverseVerticalCamera;

			/// <summary>
			/// 水平方向のカメラ移動を反転させるかどうか
			/// </summary>
			public bool IsReverseHorizontalCamera;
			
		}
		
		/// <summary>
		/// 強制的に選択肢を設定する
		/// </summary>
		/// <param name="selectable">選択肢</param>
		public void SetSelectedGameObject( GameObject selectable ) {
			this.LogDebug( "Start" );
			this.eventSystem.SetSelectedGameObject( selectable );
			this.LogDebug( "End" );
		}

		/// <summary>
		/// 設定値を返す
		/// </summary>
		/// <returns>設定値</returns>
		public OptionValue GetOptionValue() {
			this.LogDebug( "Start" );
			this.LogDebug( "End" );
			return new OptionValue();
		}

		/// <summary>
		/// 設定値を設定する
		/// </summary>
		/// <param name="optionValue">設定値</param>
		public void SetOptionValue( OptionValue optionValue ) { 
			this.LogDebug( "Start" );
			this.LogDebug( "End" );
		}

	}

}