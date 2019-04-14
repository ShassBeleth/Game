using System.Collections.Generic;
using System.Linq;
using Models;
using Services.Saves;
using Services.Scenes;
using Services.Scenes.Parameters;
using UniRx;
using UnityEngine;
using Views.SelectSaveData;
using Views.UserController;

namespace Presenters.SelectSaveData {

	/// <summary>
	/// セーブデータ選択Presenter
	/// </summary>
	/// <remarks>カーソルを動かしただけではイベントが発火しないので、Navigationを使わずにカーソル移動させている</remarks>
	public class SelectSaveDataPresenter {

		#region Model

		/// <summary>
		/// 選択されたセーブデータのId
		/// </summary>
		public ReactiveProperty<int> selectedSaveDataId = new ReactiveProperty<int>( -1 );

		#endregion

		#region View

		/// <summary>
		/// セーブデータ選択View
		/// </summary>
		private SelectSaveDataView SelectSaveDataView { set; get; }
		
		/// <summary>
		/// UserControllerView
		/// </summary>
		private UserControllerView UserControllerView { set; get; }

		#endregion

		#region Service

		/// <summary>
		/// シーンService
		/// </summary>
		private readonly SceneService sceneService = SceneService.GetInstance();

		/// <summary>
		/// セーブService
		/// </summary>
		private readonly SaveService saveService = SaveService.GetInstance();

		#endregion
		
		/// <summary>
		/// 一人プレイかどうか
		/// </summary>
		private readonly bool isSinglePlayMode;
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="parameter">前画面から引き継ぐパラメータ</param>
		public SelectSaveDataPresenter( SelectSaveDataParameter parameter ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Single Play Mode is {parameter.IsSinglePlayMode}" );

			this.isSinglePlayMode = parameter.IsSinglePlayMode;

			// Viewの設定
			this.InitialViewSetting();

			// ModelのSubscribeを設定
			this.InitialModelSubscribeSetting();

			// セーブデータをViewに必要な情報に加工
			List<SelectSaveDataView.SaveData> saveDataList = this.saveService.GetSaves()
				.Select( s => this.ConvertSaveDataOfPresenterToSaveDataOfView( s ) )
				.OrderBy( s => s.Id )
				.ToList();

			// セーブデータリストの描画
			this.SelectSaveDataView.SetSaveDataList( saveDataList );
			
			// Save1が選ばれている状態にしておく
			this.SelectSaveDataView.SetSelectedSaveData( this.SelectSaveDataView.Saves[ 0 ] );

			this.LogDebug( "End" );
		}
		
		#region 初期設定

		/// <summary>
		/// Viewの設定
		/// </summary>
		private void InitialViewSetting() {
			this.LogDebug( "Start" );

			// Viewを取得
			this.SelectSaveDataView = GameObject.Find( "Canvas" ).GetComponent<SelectSaveDataView>();
			this.UserControllerView = GameObject.Find( "UserController" ).GetComponent<UserControllerView>();
			
			this.LogDebug( "End" );
		}
		
		/// <summary>
		/// ModelのSubscribeを設定
		/// </summary>
		private void InitialModelSubscribeSetting() {
			this.LogDebug( "Start" );
			this.selectedSaveDataId.Subscribe( ( id ) => this.ChangedSelectSaveData( id ) ).AddTo( this.SelectSaveDataView );
			this.UserControllerView.MenuButtons[ "Cancel" ].Subscribe( ( value ) => { this.ChangedCancelButton( value ); } ).AddTo( this.SelectSaveDataView );
			this.LogDebug( "End" );
		}

		#endregion

		#region ModelのSubscribeによるイベント

		/// <summary>
		/// 選択されたセーブデータ変更時イベント
		/// </summary>
		/// <param name="index">選択されたセーブデータのId</param>
		private void ChangedSelectSaveData( int id ) {
			this.LogDebug( "Start" );
			List<SaveDataModel> saves = this.saveService.GetSaves();
			SaveDataModel save = saves.FirstOrDefault( s => s.id == id );

			if( save != null ) {
				if( !save.exsitsAlreadyData ) {
					// 値がある場合でセーブデータがない場合はセーブデータを作成し遷移
					this.saveService.CreateNewSave( id );
					this.LogWarning( "未実装" );
				}
				else {

					int index = saves
						.Select( ( content , i ) => new { Content = content , Index = i } )
						.FirstOrDefault( element => element.Content.id == id ).Index;

					// 値がある場合でセーブデータがある場合はパネルを表示
					this.SelectSaveDataView.ShowPanel( index , this.ConvertSaveDataOfPresenterToSaveDataOfView( save ) );
					this.SelectSaveDataView.SetSelectedButtonInPanel( index );

				}
			}
			else {
				// 値がない場合パネルを非表示にする
				this.SelectSaveDataView.ShowPanel( null , null );
			}
			this.LogDebug( "End" );
		}

		/// <summary>
		/// キャンセルボタン押下時イベント
		/// </summary>
		/// <param name="value">値</param>
		private void ChangedCancelButton( int value ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Value is {value}." );
			if( value == 1 ) {
				this.LogWarning( $"Selected Save Data Index is {this.selectedSaveDataId.Value}." );
				// パネルが非表示ならタイトルに戻る
				if( this.selectedSaveDataId.Value == -1 ) {
					this.LogDebug( "Panel Don't show" );
					TitleParameter parameter = new TitleParameter() {
						InitialTitlePart = TitleParameter.InitialTitlePartEnum.MainMenu
					};
					this.sceneService.LoadScene( "Title" , parameter );
				}
				// パネルが表示されているならパネルを非表示にする
				else {
					this.LogDebug( "Close Panel" );
					this.SelectSaveDataView.SetSelectedSaveData( this.SelectSaveDataView.Saves[ this.selectedSaveDataId.Value ] );
					this.selectedSaveDataId.Value = -1;
				}
			}
			this.LogDebug( "End" );
		}

		#endregion

		#region Viewイベント

		/// <summary>
		/// セーブデータ選択時イベント
		/// </summary>
		/// <param name="id">id</param>
		private void ClickedSaveData( int id ) {
			this.LogDebug( "Start" );
			this.selectedSaveDataId.Value = id;
			this.LogDebug( "End" );
		}

		/// <summary>
		/// 続きからボタン押下時イベント
		/// </summary>
		/// <param name="id">id</param>
		private void ClickedContinueButtonEvent( int id ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Id is {id}" );

			this.LogWarning( "未実装" );

			this.LogDebug( "End" );
		}

		/// <summary>
		/// チャプターセレクトボタン押下時イベント
		/// </summary>
		/// <param name="id">Id</param>
		private void ClickedChapterSelectButtonEvent( int id ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Id is {id}" );
			SaveDataModel save = this.saveService.GetSaves().FirstOrDefault( s => s.id == id );
			ChapterSelectParameter parameter = new ChapterSelectParameter() {
				SaveDataId = save.id ,
				IsSinglePlayMode = this.isSinglePlayMode ,
				ClearedChapters
					= save.clearedChapters
						.Select( c => new ChapterSelectParameter.Chapter() { Id = c.Id } ).ToList()
			};

			this.sceneService.LoadScene( "ChapterSelect" , parameter );

			this.LogDebug( "End" );
		}

		/// <summary>
		/// コピーボタン押下時イベント
		/// </summary>
		/// <param name="id">id</param>
		private void ClickedCopyButtonEvent( int id ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Id is {id}" );

			this.LogWarning( "未実装" );
			this.LogDebug( "End" );
		}

		/// <summary>
		/// 削除ボタン押下時イベント
		/// </summary>
		/// <param name="id">id</param>
		private void ClickedDeleteButtonEvent( int id ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Id is {id}" );

			this.saveService.DeleteSaveData( id );

			// TODO レイアウトの更新

			this.LogDebug( "End" );
		}

		#endregion

		#region 変換

		/// <summary>
		/// ModelをViewで使える形に変換する
		/// </summary>
		/// <param name="model">Model</param>
		/// <returns>Viewで使用する形</returns>
		private SelectSaveDataView.SaveData ConvertSaveDataOfPresenterToSaveDataOfView( SaveDataModel model ) {
			this.LogDebug( "Start" );
			SelectSaveDataView.SaveData data = new SelectSaveDataView.SaveData() {
				Id = model.id ,
				ExistsAlreadyData = model.exsitsAlreadyData ,
				userName = model.userName ,
				latestUpdateDateTime = model.latestUpdateDateTime
			};
			data.OnClickedSaveData.Subscribe( _ => this.ClickedSaveData( model.id ) );
			data.OnClickedContinueButton.Subscribe( _ => this.ClickedContinueButtonEvent( model.id ) );
			data.OnClickedChapterSelectButton.Subscribe( _ => this.ClickedChapterSelectButtonEvent( model.id ) );
			data.OnClickedCopyButton.Subscribe( _ => this.ClickedCopyButtonEvent( model.id ) );
			data.OnClickedDeleteButton.Subscribe( _ => this.ClickedDeleteButtonEvent( model.id ) );

			this.LogDebug( "End" );
			return data;
		}

		#endregion

	}

}
