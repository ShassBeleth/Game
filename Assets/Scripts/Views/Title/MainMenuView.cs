using System;
using UniRx;
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
		/// 一人プレイボタン押下時イベントSubject
		/// </summary>
		private readonly Subject<Unit> OnClickedSinglePlayButtonSubject = new Subject<Unit>();

		/// <summary>
		/// 一人プレイボタン押下時イベント購読
		/// </summary>
		public IObservable<Unit> OnClickedSinglePlayButton => this.OnClickedSinglePlayButtonSubject;

		/// <summary>
		/// 一人プレイボタン押下時イベント
		/// </summary>
		public void OnClickSinglePlayButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickedSinglePlayButtonSubject.OnNext( Unit.Default );
			this.LogDebug( "End" );
		}

		#endregion

		#region マルチプレイボタンについて

		/// <summary>
		/// マルチプレイGameObject
		/// </summary>
		public GameObject multiPlayGameObject;

		/// <summary>
		/// マルチプレイボタン押下時イベントSubject
		/// </summary>
		private readonly Subject<Unit> OnClickedMultiPlayButtonSubject = new Subject<Unit>();

		/// <summary>
		/// マルチプレイボタン押下時イベント購読
		/// </summary>
		public IObservable<Unit> OnClickedMultiPlayButton => this.OnClickedMultiPlayButtonSubject;

		/// <summary>
		/// マルチプレイボタン押下時イベント
		/// </summary>
		public void OnClickMultiPlayButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickedMultiPlayButtonSubject.OnNext( Unit.Default );
			this.LogDebug( "End" );
		}

		#endregion

		#region ギャラリーボタンについて

		/// <summary>
		/// ギャラリーGameObject
		/// </summary>
		public GameObject galleryGameObject;

		/// <summary>
		/// ギャラリーボタン押下時イベントSubject
		/// </summary>
		private readonly Subject<Unit> OnClickedGalleryButtonSubject = new Subject<Unit>();

		/// <summary>
		/// ギャラリーボタン押下時イベント購読
		/// </summary>
		public IObservable<Unit> OnClickedGalleryButton => this.OnClickedGalleryButtonSubject;

		/// <summary>
		/// ギャラリーボタン押下時イベント
		/// </summary>
		public void OnClickGalleryButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickedGalleryButtonSubject.OnNext( Unit.Default );
			this.LogDebug( "End" );
		}

		#endregion

		#region ランキングボタンについて

		/// <summary>
		/// ランキングGameObject
		/// </summary>
		public GameObject rankingGameObject;

		/// <summary>
		/// ランキングボタン押下時イベントSubject
		/// </summary>
		private readonly Subject<Unit> OnClickedRankingButtonSubject = new Subject<Unit>();

		/// <summary>
		/// ランキングボタン押下時イベント購読
		/// </summary>
		public IObservable<Unit> OnClickedRankingButton => this.OnClickedRankingButtonSubject;

		/// <summary>
		/// ランキングボタン押下時イベント
		/// </summary>
		public void OnClickRankingButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickedRankingButtonSubject.OnNext( Unit.Default );
			this.LogDebug( "End" );
		}

		#endregion

		#region オプションボタンについて

		/// <summary>
		/// オプションGameObject
		/// </summary>
		public GameObject optionGameObject;

		/// <summary>
		/// オプションボタン押下時イベントSubject
		/// </summary>
		private readonly Subject<Unit> OnClickedOptionButtonSubject = new Subject<Unit>();

		/// <summary>
		/// オプションボタン押下時イベント購読
		/// </summary>
		public IObservable<Unit> OnClickedOptionButton => this.OnClickedOptionButtonSubject;

		/// <summary>
		/// オプションボタン押下時イベント
		/// </summary>
		public void OnClickOptionButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickedOptionButtonSubject.OnNext( Unit.Default );
			this.LogDebug( "End" );
		}

		#endregion

		#region ゲーム終了ボタンについて

		/// <summary>
		/// ゲーム終了GameObject
		/// </summary>
		public GameObject exitGameObject;

		/// <summary>
		/// ゲーム終了ボタン押下時イベントSubject
		/// </summary>
		private readonly Subject<Unit> OnClickedExitButtonSubject = new Subject<Unit>();

		/// <summary>
		/// ゲーム終了ボタン押下時イベント購読
		/// </summary>
		public IObservable<Unit> OnClickedExitButton => this.OnClickedExitButtonSubject;

		/// <summary>
		/// ゲーム終了ボタン押下時イベント
		/// </summary>
		public void OnClickExitButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickedExitButtonSubject.OnNext( Unit.Default );
			this.LogDebug( "End" );
		}

		#endregion
		
		/// <summary>
		/// 強制的に選択肢を設定する
		/// </summary>
		/// <param name="selectable">選択肢</param>
		public void SetSelectedGameObject( GameObject selectable ) {
			this.LogDebug( "Start" );
			this.eventSystem.SetSelectedGameObject( selectable );
			this.LogDebug( "End" );
		}

	}

}