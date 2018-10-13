using Saves.Models;
using Saves.Serializers;
using SceneManagers;
using SceneManagers.Parameters;
using UnityEditor;
using UnityEngine;
using Views.Title;

namespace Presenters.Title {

	/// <summary>
	/// タイトルPresenter
	/// </summary>
	public class TitlePresenter {
				
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
		/// コンストラクタ
		/// </summary>
		public TitlePresenter() : this( null ) { }
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="parameter">前画面から受け取るパラメータ</param>
		public TitlePresenter( TitleParameter parameter ) {
			Logger.Debug( "Start" );
			Logger.Debug( "Parameter Exists..." + ( parameter == null ? "NG" : "OK." ) );
			if( parameter != null ) {
				Logger.Debug( "Initial Title Part is " + parameter.InitialTitlePart.Value );
			}

			// Viewを取得
			this.TitleView = GameObject.Find( "Canvas" ).GetComponent<TitleView>();
			this.MainMenuView = GameObject.Find( "MainMenu" ).GetComponent<MainMenuView>();
			this.OptionView = GameObject.Find( "OptionMenu" ).GetComponent<OptionView>();

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

			// オプション設定値をストレージから取得
			OptionView.OptionValue optionValue = this.ConvertOptionValue( OptionSaveDataSerializer.LoadOptionSaveData() );

			// オプション設定値の設定
			this.OptionView.SetOptionValue( optionValue );

			// 遷移前画面の情報がなければShow Please Push Any Keyの表示
			if( parameter?.InitialTitlePart == null ) {
				Logger.Debug( "Initial Title Part Enum is Null." );
				this.TitleView.ShowPleasePushAnyKey();
			}
			else {
				Logger.Debug( "Initial Title Part Enum is " + parameter.InitialTitlePart.Value );
				switch( parameter.InitialTitlePart.Value ) {
					case TitleParameter.InitialTitlePartEnum.MainMenu:
						this.TitleView.ShowMainMenu();
						break;
					case TitleParameter.InitialTitlePartEnum.PleasePushAnyKey:
						this.TitleView.ShowPleasePushAnyKey();
						break;
					default:
						Logger.Warning( "Before Scene Name is Unexpected Name." );
						break;
				}
			}
			
			Logger.Debug( "End" );
		}

		/// <summary>
		/// オプションのセーブデータから描画用に値を変換する
		/// </summary>
		/// <param name="model">オプションのセーブデータ</param>
		/// <returns></returns>
		private OptionView.OptionValue ConvertOptionValue( OptionSaveDataModel model ) {
			Logger.Debug( "Start" );
			Logger.Warning( "未実装" );
			Logger.Debug( "End" );
			return new OptionView.OptionValue();
		}

		/// <summary>
		/// 何かのボタン押下時イベント
		/// </summary>
		private void ClickedAnyKeyEvent() {
			Logger.Debug( "Start" );
			this.TitleView.ShowMainMenu();
			Logger.Debug( "End" );
		}

		#region メインメニューのイベント群

		/// <summary>
		/// 一人プレイボタン押下時イベント
		/// </summary>
		private void ClickedSinglePlayButtonEvent() {
			Logger.Debug( "Start" );
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
			SceneManager.GetInstance().LoadScene( "Gallery" , null );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// ランキングボタン押下時イベント
		/// </summary>
		private void ClickedRankingButtonEvent() {
			Logger.Debug( "Start" );
			SceneManager.GetInstance().LoadScene( "Ranking" , null );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// オプションボタン押下時イベント
		/// </summary>
		private void ClickedOptionButtonEvent() {
			Logger.Debug( "Start" );
			this.TitleView.ShowOption();
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
		/// 画面上の項目をセーブデータように変換
		/// </summary>
		/// <param name="optionValue">画面上のオプションの項目</param>
		/// <returns>オプションセーブデータ</returns>
		private OptionSaveDataModel ConvertOptionSaveDataModel( OptionView.OptionValue optionValue ) {
			Logger.Debug( "Start" );
			Logger.Debug( "End" );
			return new OptionSaveDataModel();
		}

		/// <summary>
		/// オプションの戻るボタン押下時イベント
		/// </summary>
		private void ClickedOptionBackButtonEvent() {
			Logger.Debug( "Start" );

			// 設定値の取得
			OptionView.OptionValue optionValue = this.OptionView.GetOptionValue();

			// 設定値の保存
			OptionSaveDataSerializer.WriteOptionSaveData( 
				this.ConvertOptionSaveDataModel( optionValue ) 
			);

			// TODO 設定値の反映

			// GameObjectの表示切り替え
			this.TitleView.ShowMainMenu();

			Logger.Debug( "End" );
		}

	}

}