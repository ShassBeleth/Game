using SceneManagers;
using UnityEditor;
using UnityEngine;
using Views.Title;

namespace Presenters.Title {

	/// <summary>
	/// タイトルPresenter
	/// </summary>
	public class TitlePresenter {
		
		/// <summary>
		/// 遷移前画面名
		/// </summary>
		public static string SceneNameBeforeTransition = null;

		/// <summary>
		/// タイトルView
		/// </summary>
		private TitleView TitleView { set; get; }

		/// <summary>
		/// メインメニューView
		/// </summary>
		private MainMenuView MainMenuView { set; get; }

		/// <summary>
		/// オプションView
		/// </summary>
		private OptionView OptionView { set; get; }

		/// <summary>
		/// タイトルGameObject
		/// </summary>
		private GameObject TitleGameObject { set; get; }

		/// <summary>
		/// PleasePushAnyKeyのテキストGameObject
		/// </summary>
		private GameObject PleasePushAnyKeyTextGameObject { set; get; }

		/// <summary>
		/// メインメニューGameObject
		/// </summary>
		private GameObject MainMenuGameObject { set; get; }

		/// <summary>
		/// オプションGameObject
		/// </summary>
		private GameObject OptionGameObject { set; get; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TitlePresenter() {
			Logger.Debug( "Start" );

			// hierarchyからViewを持つGameObject取得
			this.TitleGameObject = GameObject.Find( "Canvas" );
			this.MainMenuGameObject = GameObject.Find( "MainMenu" );
			this.PleasePushAnyKeyTextGameObject = GameObject.Find( "PleasePushAnyKey" );
			this.OptionGameObject = GameObject.Find( "OptionMenu" );

			// Viewを取得
			this.TitleView = this.TitleGameObject.GetComponent<TitleView>();
			this.MainMenuView = this.MainMenuGameObject.GetComponent<MainMenuView>();
			this.OptionView = this.OptionGameObject.GetComponent<OptionView>();

			// タイトルViewのEventHandler設定
			this.TitleView.OnClickAnyKeyEventHandler = this.ClickedAnyKeyEvent;

			// メインメニューViewのEventHandler設定
			this.MainMenuView.OnClickSinglePlayButtonEventHandler = this.ClickedSinglePlayButtonEvent;
			this.MainMenuView.OnClickMultiPlayButtonEventHandler = this.ClickedMultiPlayButtonEvent;
			this.MainMenuView.OnClickGalleryButtonEventHandler = this.ClickedGalleryButtonEvent;
			this.MainMenuView.OnClickRankingButtonEventHandler = this.ClickedRankingButtonEvent;
			this.MainMenuView.OnClickOptionButtonEventHandler = this.ClickedOptionButtonEvent;
			this.MainMenuView.OnClickExitButtonEventHandler = this.ClickedExitButtonEvent;

			// オプションViewのEventHandler設定
			this.OptionView.OnClickBackButtonEventHandler = this.ClickedOptionBackButtonEvent;

			// TODO オプション設定値をストレージから取得
			OptionView.OptionValue optionValue = new OptionView.OptionValue();

			// オプション設定値の設定
			this.OptionView.SetOptionValue( optionValue );

			// ゲーム起動により遷移した場合
			if( SceneManager.BeforeSingleModeSceneName == null ) {
				Logger.Debug( "Before Scene Name is Null." );
				this.PleasePushAnyKeyTextGameObject.SetActive( true );
				this.MainMenuGameObject.SetActive( false );
				this.OptionGameObject.SetActive( false );
			}
			else {
				switch( SceneManager.BeforeSingleModeSceneName ) {
					case "Gallery":
					case "SelectSaveData":
					case "ChapterSelect":
						Logger.Debug( "Before Scene Name is Gallery" );
						this.PleasePushAnyKeyTextGameObject.SetActive( false );
						this.MainMenuGameObject.SetActive( true );
						this.OptionGameObject.SetActive( false );
						break;
					default:
						Logger.Warning( "Before Scene Name is Unexpected Name." );
						break;
				}
			}
			
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 何かのボタン押下時イベント
		/// </summary>
		private void ClickedAnyKeyEvent() {
			Logger.Debug( "Start" );

			// オブジェクトの切り替え
			this.PleasePushAnyKeyTextGameObject.SetActive( false );
			this.MainMenuGameObject.SetActive( true );

			Logger.Debug( "End" );
		}

		#region メインメニューのイベント群

		/// <summary>
		/// 一人プレイボタン押下時イベント
		/// </summary>
		private void ClickedSinglePlayButtonEvent() {
			Logger.Debug( "Start" );
			UnityEngine.SceneManagement.SceneManager.LoadScene( "SelectSaveData" );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// マルチプレイボタン押下時イベント
		/// </summary>
		private void ClickedMultiPlayButtonEvent() {
			Logger.Debug( "Start" );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// ギャラリーボタン押下時イベント
		/// </summary>
		private void ClickedGalleryButtonEvent() {
			Logger.Debug( "Start" );
			UnityEngine.SceneManagement.SceneManager.LoadScene( "Gallery" );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// ランキングボタン押下時イベント
		/// </summary>
		private void ClickedRankingButtonEvent() {
			Logger.Debug( "Start" );
			Logger.Debug( "End" );
		}
		/// <summary>
		/// オプションボタン押下時イベント
		/// </summary>
		private void ClickedOptionButtonEvent() {
			Logger.Debug( "Start" );
			this.MainMenuGameObject.SetActive( false );
			this.OptionGameObject.SetActive( true );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// ゲーム終了ボタン押下時イベント
		/// </summary>
		private void ClickedExitButtonEvent() {
			Logger.Debug( "Start" );

			// ゲーム終了
#if UNITY_EDITOR
			Logger.Debug( "UNITY EDITOR" );
			EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
			Logger.Debug( "UNITY STANDALONE" );
			Application.Quit();
#endif
			Logger.Debug( "End" );
		}

		#endregion

		/// <summary>
		/// オプションの戻るボタン押下時イベント
		/// </summary>
		private void ClickedOptionBackButtonEvent() {
			Logger.Debug( "Start" );

			// 設定値の取得
			OptionView.OptionValue optionValue = this.OptionView.GetOptionValue();

			// TODO 設定値の保存

			// TODO 設定値の反映
			
			// GameObjectの表示切り替え
			this.OptionGameObject.SetActive( false );
			this.MainMenuGameObject.SetActive( true );

			Logger.Debug( "End" );
		}

	}

}