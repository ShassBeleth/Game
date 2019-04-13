using Models;
using Models.Title;
using Services.Scenes;
using Services.Scenes.Parameters;
using UniRx;
using UnityEditor;
using UnityEngine;
using Views.Title;
using Views.UserController;

namespace Presenters.Title {

	/// <summary>
	/// タイトルPresenter
	/// </summary>
	public class TitlePresenter {

		#region Model

		/// <summary>
		/// WindowModel
		/// </summary>
		private TitleWindowModel titleWindowModel = new TitleWindowModel( WindowNameEnum.PleasePushAnyKey );

		/// <summary>
		/// NextSceneModel
		/// </summary>
		private NextSceneModel nextSceneModel = new NextSceneModel();

		/// <summary>
		/// OptionModel
		/// </summary>
		private OptionModel optionModel;

		#endregion

		#region View

		/// <summary>
		/// UserControllerView
		/// </summary>
		private UserControllerView UserControllerView { set; get; }
		
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
		/// 何かキーを押してくださいView
		/// </summary>
		private PleasePushAnyKeyView PleasePushAnyKeyView { set; get; }

		#endregion

		#region Service

		/// <summary>
		/// シーンService
		/// </summary>
		private SceneService sceneService = SceneService.GetInstance();

		#endregion

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TitlePresenter() : this( null ) { }
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="parameter">前画面から受け取るパラメータ</param>
		public TitlePresenter( TitleParameter parameter ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Parameter {( parameter == null ? "is Null" : "Exists" )}." );
			if( parameter != null ) {
				this.LogDebug( $"Initial Title Part is {parameter.InitialTitlePart.Value}" );
			}

			// Viewの初期設定
			this.InitialViewSetting();
			
			// Optionの設定
			{
				// TODO オプション設定値をストレージから取得
				
				// Viewに渡せる形に変換してから渡す
				OptionView.OptionValue optionValue = this.ConvertOptionValue( this.optionModel );
				// オプション設定値の設定
				this.OptionView.SetOptionValue( optionValue );
			}

			// ModelのSubscribeを設定
			this.InitialModelSubscribeSetting();

			// タイトル画面遷移時パラメータから初期表示画面を決める
			this.InitialShowWindow( parameter?.InitialTitlePart );
			
			this.LogDebug( "End" );
		}

		#region 初期設定

		/// <summary>
		/// Viewの設定
		/// </summary>
		private void InitialViewSetting() {
			this.LogDebug( "Start" );

			// Viewを取得
			this.TitleView = GameObject.Find( "Canvas" ).GetComponent<TitleView>();
			this.MainMenuView = this.TitleView.transform.Find( "MainMenu" ).GetComponent<MainMenuView>();
			this.OptionView = this.TitleView.transform.Find( "OptionMenu" ).GetComponent<OptionView>();
			this.PleasePushAnyKeyView = this.TitleView.transform.Find( "PleasePushAnyKey" ).GetComponent<PleasePushAnyKeyView>();
			this.UserControllerView = GameObject.Find( "UserController" ).GetComponent<UserControllerView>();

			// PleasePushAnyKeyViewのEventHandler設定
			this.PleasePushAnyKeyView.OnClickAnyKeyEventHandler = this.ClickedAnyKeyEvent;

			// メインメニューViewのEventHandler設定
			this.MainMenuView.OnClickSinglePlayButtonEventHandler = this.ClickedSinglePlayButtonEvent;
			this.MainMenuView.OnClickMultiPlayButtonEventHandler = this.ClickedMultiPlayButtonEvent;
			this.MainMenuView.OnClickGalleryButtonEventHandler = this.ClickedGalleryButtonEvent;
			this.MainMenuView.OnClickRankingButtonEventHandler = this.ClickedRankingButtonEvent;
			this.MainMenuView.OnClickOptionButtonEventHandler = this.ClickedOptionButtonEvent;
			this.MainMenuView.OnClickExitButtonEventHandler = this.ClickedExitButtonEvent;

			this.LogDebug( "End" );
		}

		/// <summary>
		/// ModelのSubscribeを設定
		/// </summary>
		private void InitialModelSubscribeSetting() {
			this.LogDebug( "Start" );
			this.titleWindowModel.windowName.Subscribe( ( name ) => { this.ChangedWindowName( name ); } );
			this.nextSceneModel.nextSceneName.Subscribe( ( nextSceneName ) => { this.ChangedNextSceneName( nextSceneName ); } );
			this.UserControllerView.MenuButtons[ "Cancel" ].Subscribe( ( value ) => { this.ChangedCancelButton( value ); } );
			this.LogDebug( "End" );
		}

		/// <summary>
		/// タイトル画面遷移時パラメータから初期表示画面を決める
		/// </summary>
		/// <param name="initialTitlePartEnum">初期表示画面</param>
		private void InitialShowWindow( TitleParameter.InitialTitlePartEnum? initialTitlePart ) {
			this.LogDebug( "Start" );

			// 遷移前画面の情報がなければShow Please Push Any Keyの表示
			if( !initialTitlePart.HasValue ) {
				this.LogDebug( "Initial Title Part Enum is Null." );
				this.titleWindowModel.windowName.Value = WindowNameEnum.PleasePushAnyKey;
			}
			else {
				this.LogDebug( $"Initial Title Part is {initialTitlePart.Value}" );
				switch( initialTitlePart.Value ) {
					case TitleParameter.InitialTitlePartEnum.MainMenu:
						this.titleWindowModel.windowName.Value = WindowNameEnum.MainMenu;
						break;
					case TitleParameter.InitialTitlePartEnum.PleasePushAnyKey:
						this.titleWindowModel.windowName.Value = WindowNameEnum.PleasePushAnyKey;
						break;
					default:
						this.LogWarning( "Before Scene Name is Unexpected Name." );
						break;
				}
			}

			this.LogDebug( "End" );
		}

		#endregion

		#region ModelのSubscribeによるイベント

		/// <summary>
		/// Window名変更時イベント
		/// </summary>
		/// <param name="windowName">Window名</param>
		private void ChangedWindowName( WindowNameEnum windowName ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Changed Window Name is {windowName}" );

			// 表示する画面を切り替えて、選択肢を選択状態にしておく
			switch( windowName ) {
				case WindowNameEnum.PleasePushAnyKey:
					this.TitleView.ShowPleasePushAnyKey();
					break;
				case WindowNameEnum.MainMenu:
					this.TitleView.ShowMainMenu();
					this.MainMenuView.SetSelectedGameObject( this.MainMenuView.singlePlayGameObject );
					break;
				case WindowNameEnum.Option:
					this.TitleView.ShowOption();
					break;
				default:
					this.LogWarning( "Unexpected Window Name" );
					break;
			}
			this.LogDebug( "End" );
		}

		/// <summary>
		/// 遷移先シーン名変更時イベント
		/// </summary>
		/// <param name="nextSceneName"></param>
		private void ChangedNextSceneName( NextSceneNameEnum nextSceneName ) {
			this.LogDebug( "Start" );

			// シーン名
			string sceneName = nextSceneName.ToString();
			this.LogDebug( $"Scene Name is {sceneName}." );

			// パラメータ設定
			switch( nextSceneName ) {
				case NextSceneNameEnum.None:
					this.LogDebug( "Next Scene Name is None." );
					break;
				case NextSceneNameEnum.SelectSaveData:
					SelectSaveDataParameter parameter = new SelectSaveDataParameter() {
						IsSinglePlayMode = this.nextSceneModel.IsSingleMode
					};
					this.sceneService.LoadScene( sceneName , parameter );
					break;
				default:
					this.sceneService.LoadScene( sceneName , null );
					break;
			}

			// シーンを切り替える

			this.LogDebug( "End" );
		}

		/// <summary>
		/// キャンセルボタンが押された時のイベント
		/// </summary>
		/// <param name="value"></param>
		private void ChangedCancelButton( int value ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Value is {value}." );
			if( value == 1 ) {
				if( this.titleWindowModel.windowName.Value == WindowNameEnum.Option ) {
					// GameObjectの表示切り替え
					this.titleWindowModel.windowName.Value = WindowNameEnum.MainMenu;
				}
			}
			this.LogDebug( "End" );
		}

		#endregion
		
		#region Viewイベント

		/// <summary>
		/// 何かのボタン押下時イベント
		/// </summary>
		private void ClickedAnyKeyEvent() {
			this.LogDebug( "Start" );
			this.titleWindowModel.windowName.Value = WindowNameEnum.MainMenu;
			this.LogDebug( "End" );
		}

		#region メインメニューのイベント群

		/// <summary>
		/// 一人プレイボタン押下時イベント
		/// </summary>
		private void ClickedSinglePlayButtonEvent() {
			this.LogDebug( "Start" );
			this.nextSceneModel.IsSingleMode = true;
			this.nextSceneModel.nextSceneName.Value = NextSceneNameEnum.SelectSaveData;
			this.LogDebug( "End" );
		}

		/// <summary>
		/// マルチプレイボタン押下時イベント
		/// </summary>
		private void ClickedMultiPlayButtonEvent() {
			this.LogDebug( "Start" );
			this.nextSceneModel.IsSingleMode = false;
			this.nextSceneModel.nextSceneName.Value = NextSceneNameEnum.SelectSaveData;
			this.LogDebug( "End" );
		}

		/// <summary>
		/// ギャラリーボタン押下時イベント
		/// </summary>
		private void ClickedGalleryButtonEvent() {
			this.LogDebug( "Start" );
			this.nextSceneModel.nextSceneName.Value = NextSceneNameEnum.Gallery;
			this.LogDebug( "End" );
		}

		/// <summary>
		/// ランキングボタン押下時イベント
		/// </summary>
		private void ClickedRankingButtonEvent() {
			this.LogDebug( "Start" );
			this.nextSceneModel.nextSceneName.Value = NextSceneNameEnum.Ranking;
			this.LogDebug( "End" );
		}

		/// <summary>
		/// オプションボタン押下時イベント
		/// </summary>
		private void ClickedOptionButtonEvent() {
			this.LogDebug( "Start" );
			this.titleWindowModel.windowName.Value = WindowNameEnum.Option;
			this.LogDebug( "End" );
		}

		/// <summary>
		/// ゲーム終了ボタン押下時イベント
		/// </summary>
		private void ClickedExitButtonEvent() {
			this.LogDebug( "Start" );

			// ゲーム終了
#if UNITY_EDITOR
			this.LogDebug( "UNITY EDITOR" );
			EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
			this.LogDebug( "UNITY STANDALONE" );
			Application.Quit();
#endif
			this.LogDebug( "End" );
		}

		#endregion

		#endregion

		/// <summary>
		/// オプション情報のセーブ
		/// TODO 設定値の反映
		/// </summary>
		private void SaveOptionData() {
			this.LogDebug( "Start" );
			// 設定値の取得
			OptionView.OptionValue optionValue = this.OptionView.GetOptionValue();

			// TODO 設定値の保存
			
			this.LogDebug( "End" );
		}

		/// <summary>
		/// オプションModelから描画用に値を変換する
		/// </summary>
		/// <param name="model">オプションModel</param>
		/// <returns></returns>
		private OptionView.OptionValue ConvertOptionValue( OptionModel model ) {
			this.LogDebug( "Start" );
			this.LogDebug( "End" );
			return new OptionView.OptionValue() {
				IsReverseVerticalCamera = model?.IsReverseVerticalCamera?.Value ?? false ,
				IsReverseHorizontalCamera = model?.IsReverseHorizontalCamera?.Value ?? false
			};
		}

		/// <summary>
		/// 画面上の項目をOptionModelに変換
		/// </summary>
		/// <param name="optionValue">画面上のオプションの項目</param>
		/// <param name="optionModel">オプションModel</param>
		private void ConvertOptionModel( OptionView.OptionValue optionValue , ref OptionModel optionModel ) {
			this.LogDebug( "Start" );
			optionModel.IsReverseVerticalCamera.Value = optionValue.IsReverseVerticalCamera;
			optionModel.IsReverseHorizontalCamera.Value = optionValue.IsReverseHorizontalCamera;
			this.LogDebug( "End" );
		}

	}

}