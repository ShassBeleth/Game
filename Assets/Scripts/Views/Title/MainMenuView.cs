using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Views.Title {

	/// <summary>
	/// メインメニューView
	/// </summary>
	public class MainMenuView : MonoBehaviour {

		/// <summary>
		/// event system
		/// </summary>
		public EventSystem eventSystem;

		#region 一人プレイボタンについて
		/// <summary>
		/// 一人プレイGameObject
		/// </summary>
		public GameObject singlePlayGameObject;

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
		/// マルチプレイGameObject
		/// </summary>
		public GameObject multiPlayGameObject;

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
		/// ギャラリーGameObject
		/// </summary>
		public GameObject galleryGameObject;

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
		/// ランキングGameObject
		/// </summary>
		public GameObject rankingGameObject;

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
		/// オプションGameObject
		/// </summary>
		public GameObject optionGameObject;
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
		/// ゲーム終了GameObject
		/// </summary>
		public GameObject exitGameObject;

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
		
		/// <summary>
		/// 強制的に選択肢を設定する
		/// </summary>
		/// <param name="selectable">選択肢</param>
		public void SetSelectedGameObject( GameObject selectable ) {
			Logger.Debug( "Start" );
			this.eventSystem.SetSelectedGameObject( selectable );
			Logger.Debug( "End" );
		}

	}

}