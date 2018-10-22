using Models;
using Models.Title;
using Saves.Models;
using Saves.Serializers;
using SceneManagers;
using SceneManagers.Parameters;
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

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TitlePresenter() : this( null ) { }
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="parameter">前画面から受け取るパラメータ</param>
		public TitlePresenter( TitleParameter parameter ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Parameter {( parameter == null ? "is Null" : "Exists" )}." );
			if( parameter != null ) {
				Logger.Debug( $"Initial Title Part is {parameter.InitialTitlePart.Value}" );
			}

			// Viewの初期設定
			this.InitialViewSetting();
			
			// Optionの設定
			{
				// オプション設定値をストレージから取得
				OptionSaveDataModel optionSaveDataModel = OptionSaveDataSerializer.LoadOptionSaveData();
				// Modelに値を入れる
				this.optionModel = new OptionModel(
					optionSaveDataModel?.isReverseVerticalCamera ?? false ,
					optionSaveDataModel?.isReverseHorizontalCamera ?? false
				);
				// Viewに渡せる形に変換してから渡す
				OptionView.OptionValue optionValue = this.ConvertOptionValue( this.optionModel );
				// オプション設定値の設定
				this.OptionView.SetOptionValue( optionValue );
			}

			// ModelのSubscribeを設定
			this.InitialModelSubscribeSetting();

			// タイトル画面遷移時パラメータから初期表示画面を決める
			this.InitialShowWindow( parameter?.InitialTitlePart );
			
			Logger.Debug( "End" );
		}

		#region 初期設定

		/// <summary>
		/// Viewの設定
		/// </summary>
		private void InitialViewSetting() {
			Logger.Debug( "Start" );

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

			Logger.Debug( "End" );
		}

		/// <summary>
		/// ModelのSubscribeを設定
		/// </summary>
		private void InitialModelSubscribeSetting() {
			Logger.Debug( "Start" );
			this.titleWindowModel.windowName.Subscribe( ( name ) => { this.ChangedWindowName( name ); } );
			this.nextSceneModel.nextSceneName.Subscribe( ( nextSceneName ) => { this.ChangedNextSceneName( nextSceneName ); } );
			this.UserControllerView.MenuButtons[ "Cancel" ].Subscribe( ( value ) => { this.ChangedCancelButton( value ); } );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// タイトル画面遷移時パラメータから初期表示画面を決める
		/// </summary>
		/// <param name="initialTitlePartEnum">初期表示画面</param>
		private void InitialShowWindow( TitleParameter.InitialTitlePartEnum? initialTitlePart ) {
			Logger.Debug( "Start" );

			// 遷移前画面の情報がなければShow Please Push Any Keyの表示
			if( !initialTitlePart.HasValue ) {
				Logger.Debug( "Initial Title Part Enum is Null." );
				this.titleWindowModel.windowName.Value = WindowNameEnum.PleasePushAnyKey;
			}
			else {
				Logger.Debug( $"Initial Title Part is {initialTitlePart.Value}" );
				switch( initialTitlePart.Value ) {
					case TitleParameter.InitialTitlePartEnum.MainMenu:
						this.titleWindowModel.windowName.Value = WindowNameEnum.MainMenu;
						break;
					case TitleParameter.InitialTitlePartEnum.PleasePushAnyKey:
						this.titleWindowModel.windowName.Value = WindowNameEnum.PleasePushAnyKey;
						break;
					default:
						Logger.Warning( "Before Scene Name is Unexpected Name." );
						break;
				}
			}

			Logger.Debug( "End" );
		}

		#endregion

		#region ModelのSubscribeによるイベント

		/// <summary>
		/// Window名変更時イベント
		/// </summary>
		/// <param name="windowName">Window名</param>
		private void ChangedWindowName( WindowNameEnum windowName ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Changed Window Name is {windowName}" );

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
					Logger.Warning( "Unexpected Window Name" );
					break;
			}
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 遷移先シーン名変更時イベント
		/// </summary>
		/// <param name="nextSceneName"></param>
		private void ChangedNextSceneName( NextSceneNameEnum nextSceneName ) {
			Logger.Debug( "Start" );

			// シーン名
			string sceneName = nextSceneName.ToString();
			Logger.Debug( $"Scene Name is {sceneName}." );

			// パラメータ設定
			switch( nextSceneName ) {
				case NextSceneNameEnum.None:
					Logger.Debug( "Next Scene Name is None." );
					break;
				case NextSceneNameEnum.SelectSaveData:
					SelectSaveDataParameter parameter = new SelectSaveDataParameter() {
						IsSinglePlayMode = this.nextSceneModel.IsSingleMode
					};
					SceneManager.GetInstance().LoadScene( sceneName , parameter );
					break;
				default:
					SceneManager.GetInstance().LoadScene( sceneName , null );
					break;
			}

			// シーンを切り替える

			Logger.Debug( "End" );
		}

		/// <summary>
		/// キャンセルボタンが押された時のイベント
		/// </summary>
		/// <param name="value"></param>
		private void ChangedCancelButton( int value ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Value is {value}." );
			if( value == 1 ) {
				if( this.titleWindowModel.windowName.Value == WindowNameEnum.Option ) {
					// GameObjectの表示切り替え
					this.titleWindowModel.windowName.Value = WindowNameEnum.MainMenu;
				}
			}
			Logger.Debug( "End" );
		}

		#endregion
		
		#region Viewイベント

		/// <summary>
		/// 何かのボタン押下時イベント
		/// </summary>
		private void ClickedAnyKeyEvent() {
			Logger.Debug( "Start" );
			this.titleWindowModel.windowName.Value = WindowNameEnum.MainMenu;
			Logger.Debug( "End" );
		}

		#region メインメニューのイベント群

		/// <summary>
		/// 一人プレイボタン押下時イベント
		/// </summary>
		private void ClickedSinglePlayButtonEvent() {
			Logger.Debug( "Start" );
			this.nextSceneModel.IsSingleMode = true;
			this.nextSceneModel.nextSceneName.Value = NextSceneNameEnum.SelectSaveData;
			Logger.Debug( "End" );
		}

		/// <summary>
		/// マルチプレイボタン押下時イベント
		/// </summary>
		private void ClickedMultiPlayButtonEvent() {
			Logger.Debug( "Start" );
			this.nextSceneModel.IsSingleMode = false;
			this.nextSceneModel.nextSceneName.Value = NextSceneNameEnum.SelectSaveData;
			Logger.Debug( "End" );
		}

		/// <summary>
		/// ギャラリーボタン押下時イベント
		/// </summary>
		private void ClickedGalleryButtonEvent() {
			Logger.Debug( "Start" );
			this.nextSceneModel.nextSceneName.Value = NextSceneNameEnum.Gallery;
			Logger.Debug( "End" );
		}

		/// <summary>
		/// ランキングボタン押下時イベント
		/// </summary>
		private void ClickedRankingButtonEvent() {
			Logger.Debug( "Start" );
			this.nextSceneModel.nextSceneName.Value = NextSceneNameEnum.Ranking;
			Logger.Debug( "End" );
		}

		/// <summary>
		/// オプションボタン押下時イベント
		/// </summary>
		private void ClickedOptionButtonEvent() {
			Logger.Debug( "Start" );
			this.titleWindowModel.windowName.Value = WindowNameEnum.Option;
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

		#endregion

		/// <summary>
		/// オプション情報のセーブ
		/// TODO 設定値の反映
		/// </summary>
		private void SaveOptionData() {
			Logger.Debug( "Start" );
			// 設定値の取得
			OptionView.OptionValue optionValue = this.OptionView.GetOptionValue();

			// 設定値の保存
			this.ConvertOptionModel( optionValue , ref this.optionModel );
			OptionSaveDataSerializer.WriteOptionSaveData(
				this.ConvertSaveDataModel( this.optionModel )
			);
			Logger.Debug( "End" );
		}

		/// <summary>
		/// オプションModelから描画用に値を変換する
		/// </summary>
		/// <param name="model">オプションModel</param>
		/// <returns></returns>
		private OptionView.OptionValue ConvertOptionValue( OptionModel model ) {
			Logger.Debug( "Start" );
			Logger.Debug( "End" );
			return new OptionView.OptionValue() {
				IsReverseVerticalCamera = model.IsReverseVerticalCamera.Value ,
				IsReverseHorizontalCamera = model.IsReverseHorizontalCamera.Value
			};
		}

		/// <summary>
		/// 画面上の項目をOptionModelに変換
		/// </summary>
		/// <param name="optionValue">画面上のオプションの項目</param>
		/// <param name="optionModel">オプションModel</param>
		private void ConvertOptionModel( OptionView.OptionValue optionValue , ref OptionModel optionModel ) {
			Logger.Debug( "Start" );
			optionModel.IsReverseVerticalCamera.Value = optionValue.IsReverseVerticalCamera;
			optionModel.IsReverseHorizontalCamera.Value = optionValue.IsReverseHorizontalCamera;
			Logger.Debug( "End" );
		}

		/// <summary>
		/// OptionModelをオプションセーブデータモデルに変換
		/// </summary>
		/// <param name="optionValue">画面上のオプションの項目</param>
		/// <param name="optionModel">オプションModel</param>
		private OptionSaveDataModel ConvertSaveDataModel ( OptionModel optionModel ) {
			Logger.Debug( "Start" );
			Logger.Debug( "End" );
			return new OptionSaveDataModel() {
				isReverseVerticalCamera = optionModel.IsReverseVerticalCamera.Value ,
				isReverseHorizontalCamera = optionModel.IsReverseHorizontalCamera.Value
			};
		}

	}

}