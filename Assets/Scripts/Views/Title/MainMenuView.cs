using System;
using UnityEngine;

namespace Views.Title {

	/// <summary>
	/// メインメニューView
	/// </summary>
	public class MainMenuView : MonoBehaviour {

		#region 一人プレイボタンについて
		/// <summary>
		/// 一人プレイボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickSinglePlayButtonEventHandler { set; get; }

		/// <summary>
		/// 一人プレイボタン押下時イベント
		/// </summary>
		public void OnClickSinglePlayButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickSinglePlayButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}
		#endregion

		#region マルチプレイボタンについて
		/// <summary>
		/// マルチプレイボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickMultiPlayButtonEventHandler { set; get; }

		/// <summary>
		/// マルチプレイボタン押下時イベント
		/// </summary>
		public void OnClickMultiPlayButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickMultiPlayButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}
		#endregion

		#region ギャラリーボタンについて
		/// <summary>
		/// ギャラリーボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickGalleryButtonEventHandler { set; get; }

		/// <summary>
		/// ギャラリーボタン押下時イベント
		/// </summary>
		public void OnClickGalleryButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickGalleryButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}
		#endregion

		#region ランキングボタンについて
		/// <summary>
		/// ランキングボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickRankingButtonEventHandler { set; get; }

		/// <summary>
		/// ランキングボタン押下時イベント
		/// </summary>
		public void OnClickRankingButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickRankingButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}
		#endregion

		#region オプションボタンについて
		/// <summary>
		/// オプションボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickOptionButtonEventHandler { set; get; }

		/// <summary>
		/// オプションボタン押下時イベント
		/// </summary>
		public void OnClickOptionButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickOptionButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}
		#endregion

		#region ゲーム終了ボタンについて
		/// <summary>
		/// ゲーム終了ボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickExitButtonEventHandler { set; get; }

		/// <summary>
		/// ゲーム終了ボタン押下時イベント
		/// </summary>
		public void OnClickExitButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickExitButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}
		#endregion

	}

}