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

			#region Model
			// Window名変更時イベント
			this.titleWindowModel.windowName.Subscribe( ( name ) => { this.ChangedWindowName( name ); } );
			this.UserControllerView.MenuButtons[ "Cancel" ].Subscribe( ( value ) => { this.ChangedCancelButton( value ); } );
			#endregion

			// 遷移前画面の情報がなければShow Please Push Any Keyの表示
			if( parameter?.InitialTitlePart == null ) {
				Logger.Debug( "Initial Title Part Enum is Null." );
				this.TitleView.ShowPleasePushAnyKey();
			}
			else {
				Logger.Debug( $"Initial Title Part Enum is {parameter.InitialTitlePart.Value}" );
				switch( parameter.InitialTitlePart.Value ) {
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

		/// <summary>
		/// Viewの設定
		/// </summary>
		private void InitialViewSetting() {
			Logger.Debug( "Start" );

			// Viewを取得
			this.TitleView = GameObject.Find( "Canvas" ).GetComponent<TitleView>();
			this.MainMenuView = GameObject.Find( "MainMenu" ).GetComponent<MainMenuView>();
			this.OptionView = GameObject.Find( "OptionMenu" ).GetComponent<OptionView>();
			this.PleasePushAnyKeyView = GameObject.Find( "PleasePushAnyKey" ).GetComponent<PleasePushAnyKeyView>();
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

		#region Viewイベント

		/// <summary>
		/// 何かのボタン押下時イベント
		/// </summary>
		private void ClickedAnyKeyEvent() {
			Logger.Debug( "Start" );
			// 画面を切り替える
			this.titleWindowModel.windowName.Value = WindowNameEnum.MainMenu;
			Logger.Debug( "End" );
		}

		#region メインメニューのイベント群

		/// <summary>
		/// 一人プレイボタン押下時イベント
		/// </summary>
		private void ClickedSinglePlayButtonEvent() {
			Logger.Debug( "Start" );
			// シーンを切り替える
			SceneManager.GetInstance().LoadScene( 
				"SelectSaveData" , 
				new SelectSaveDataParameter() {
					IsSinglePlayMode = true
				} 
			);
			Logger.Debug( "End" );
		}

		/// <summary>
		/// マルチプレイボタン押下時イベント
		/// </summary>
		private void ClickedMultiPlayButtonEvent() {
			Logger.Debug( "Start" );
			// シーンを切り替える
			SceneManager.GetInstance().LoadScene(
				"SelectSaveData" ,
				new SelectSaveDataParameter() {
					IsSinglePlayMode = false
				}
			);
			Logger.Debug( "End" );
		}

		/// <summary>
		/// ギャラリーボタン押下時イベント
		/// </summary>
		private void ClickedGalleryButtonEvent() {
			Logger.Debug( "Start" );
			// シーンを切り替える
			SceneManager.GetInstance().LoadScene( "Gallery" , null );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// ランキングボタン押下時イベント
		/// </summary>
		private void ClickedRankingButtonEvent() {
			Logger.Debug( "Start" );
			// シーンを切り替える
			SceneManager.GetInstance().LoadScene( "Ranking" , null );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// オプションボタン押下時イベント
		/// </summary>
		private void ClickedOptionButtonEvent() {
			Logger.Debug( "Start" );
			// 画面を切り替える
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

		#endregion

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