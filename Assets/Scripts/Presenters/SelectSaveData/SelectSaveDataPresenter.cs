using System;
using System.Collections.Generic;
using Models;
using Saves.Models;
using Saves.Serializers;
using SceneManagers;
using SceneManagers.Parameters;
using UniRx;
using UnityEngine;
using Views.SelectSaveData;
using Views.UserController;

namespace Presenters.SelectSaveData {

	/// <summary>
	/// セーブデータ選択Presenter
	/// </summary>
	public class SelectSaveDataPresenter {

		#region Model

		/// <summary>
		/// セーブデータ選択Model
		/// </summary>
		private SelectSaveDataModel selectSaveDataModel;
		
		#endregion

		#region View

		/// <summary>
		/// セーブデータ選択View
		/// </summary>
		private SelectSaveDataView selectSaveDataView;
		
		/// <summary>
		/// UserControllerView
		/// </summary>
		private UserControllerView UserControllerView { set; get; }

		#endregion

		/// <summary>
		/// 一人プレイかどうか
		/// </summary>
		private bool isSinglePlayMode;
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="parameter">前画面から引き継ぐパラメータ</param>
		public SelectSaveDataPresenter( SelectSaveDataParameter parameter ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Single Play Mode is {parameter.IsSinglePlayMode}" );

			this.isSinglePlayMode = parameter.IsSinglePlayMode;

			#region View
			
			// Viewを取得
			this.selectSaveDataView = GameObject.Find( "Canvas" ).GetComponent<SelectSaveDataView>();
			this.UserControllerView = GameObject.Find( "UserController" ).GetComponent<UserControllerView>();

			#endregion

			#region Model

			// Model変更時イベント設定
			this.selectSaveDataModel = new SelectSaveDataModel( null , new List<SaveDataModel>() {
				this.ConvertSaveDataModel( 0 , SinglePlaySaveDataSerializer.LoadSinglePlaySaveData( 0 ) ) ,
				this.ConvertSaveDataModel( 1 , SinglePlaySaveDataSerializer.LoadSinglePlaySaveData( 1 ) ) ,
				this.ConvertSaveDataModel( 2 , SinglePlaySaveDataSerializer.LoadSinglePlaySaveData( 2 ) ) ,
				this.ConvertSaveDataModel( 3 , SinglePlaySaveDataSerializer.LoadSinglePlaySaveData( 3 ) )
			} );
			this.selectSaveDataModel.selectedSaveDataIndex.Subscribe( (index) => this.ChangedSelectSaveData(index) );
			this.UserControllerView.MenuButtons[ "Cancel" ].Subscribe( ( value ) => { this.ChangedCancelButton( value ); } );

			#endregion

			// セーブデータをViewに必要な情報に加工
			List<SelectSaveDataView.SaveData> saveDataList = new List<SelectSaveDataView.SaveData>() {
				this.ConvertSaveDataOfView( 0 ) ,
				this.ConvertSaveDataOfView( 1 ) ,
				this.ConvertSaveDataOfView( 2 ) ,
				this.ConvertSaveDataOfView( 3 )
			};

			// セーブデータリストの描画
			this.selectSaveDataView.SetSaveDataList( saveDataList );
			
			// Save1が選ばれている状態にしておく
			this.selectSaveDataView.SetSelectedSaveData( this.selectSaveDataView.saves[ 0 ] );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 読み込んだセーブデータをModelに変換する
		/// </summary>
		/// <param name="id">ID</param>
		/// <param name="singlePlaySaveDataModel">セーブデータ</param>
		/// <returns></returns>
		private SaveDataModel ConvertSaveDataModel( int id , SinglePlaySaveDataModel singlePlaySaveDataModel ) {
			Logger.Debug( "Start" );
			SaveDataModel model = new SaveDataModel() {
				exsitsAlreadyData = singlePlaySaveDataModel != null ,
				id = singlePlaySaveDataModel?.id ?? id ,
				userName = singlePlaySaveDataModel?.userName ,
				latestUpdateDateTime = singlePlaySaveDataModel?.latestUpdateDateTime ?? new DateTime() ,
				clearedChapters = new List<ChapterModel>()
			};
			if( singlePlaySaveDataModel?.clearedChapters != null ) {
				foreach( ChapterSaveDataModel chapter in singlePlaySaveDataModel.clearedChapters ) {
					model.clearedChapters.Add( new ChapterModel() {
						id = chapter.id ,
						Name = chapter.Name
					} );
				}
			}
			Logger.Debug( "End" );
			return model;
		}

		/// <summary>
		/// 読み込んだセーブデータをViewに渡す形に変換する
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="singlePlaySaveDataModel">セーブデータModel</param>
		/// <returns>Viewに表示するセーブデータ</returns>
		private SelectSaveDataView.SaveData ConvertSaveDataOfView( int index ) {
			Logger.Debug( "Start" );

			SelectSaveDataView.SaveData saveData = new SelectSaveDataView.SaveData() {
				ExistsAlreadyData = this.selectSaveDataModel.saveData[index].exsitsAlreadyData ,
				Id = this.selectSaveDataModel.saveData[ index ].id ,
				userName = this.selectSaveDataModel.saveData[ index ].userName  ,
				latestUpdateDateTime = this.selectSaveDataModel.saveData[ index ].latestUpdateDateTime ,
				OnClickedSaveData = () => this.ClickedSaveData( index ) ,
				OnClickContinueButtonEventHandler = () => this.ClickedContinueButtonEvent( index ) ,
				OnClickChapterSelectButtonEventHandler = () => this.ClickedChapterSelectButtonEvent( index ) ,
				OnClickCopyButtonEventHandler = () => this.ClickedCopyButtonEvent( index ) ,
				OnClickDeleteButtonEventHandler = () => this.ClickedDeleteButtonEvent( index )
			};

			Logger.Debug( "End" );
			return saveData;
		}
		
		/// <summary>
		/// セーブデータを作成する
		/// </summary>
		/// <param name="index">index</param>
		/// <returns>作成したセーブデータModel</returns>
		private SinglePlaySaveDataModel CreateSinglePlaySaveData( int index ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Index is {index}" );

			SinglePlaySaveDataModel model = new SinglePlaySaveDataModel() {
				id = index ,
				userName = Guid.NewGuid().ToString() ,
				latestUpdateDateTime = new DateTime()
			};
			// TODO 仮
			{
				model.clearedChapters = new ChapterSaveDataModel[ 3 ];
				model.clearedChapters[ 0 ] = new ChapterSaveDataModel() {
					id = 4 ,
					Name = "a"
				};
				model.clearedChapters[ 1 ] = new ChapterSaveDataModel() {
					id = 7 ,
					Name = "b"
				};
				model.clearedChapters[ 2 ] = new ChapterSaveDataModel() {
					id = 13 ,
					Name = "c"
				};
			}

			this.selectSaveDataModel.saveData[ index ] = this.ConvertSaveDataModel( index , model );

			// セーブデータに書き込み
			SinglePlaySaveDataSerializer.WriteSinglePlaySaveData( index , model );

			Logger.Debug( "End" );
			return model;
		}

		/// <summary>
		/// チャプター選択画面へ遷移
		/// </summary>
		/// <param name="saveDataModel">セーブデータモデル</param>
		private void TransitionToChapterSelect( SaveDataModel saveDataModel ) {
			Logger.Debug( "Start" );

			ChapterSelectParameter parameter = new ChapterSelectParameter() {
				Id = saveDataModel.id ,
				IsSinglePlayMode = this.isSinglePlayMode
			};
			parameter.ClearedChapters = new List<ChapterSelectParameter.Chapter>();
			Logger.Debug( $"Cleared Chapters Length is {( saveDataModel.clearedChapters?.Count ?? 0 )}" );
			if( saveDataModel.clearedChapters != null ) {
				foreach( ChapterModel chapter in saveDataModel.clearedChapters ) {
					parameter.ClearedChapters.Add(
						new ChapterSelectParameter.Chapter {
							Id = chapter.id
						}
					);
				}
			}

			SceneManager.GetInstance().LoadScene( "ChapterSelect" , parameter );
			Logger.Debug( "End" );
		}

		#region Viewイベント

		/// <summary>
		/// セーブデータ選択時イベント
		/// </summary>
		/// <param name="index">index</param>
		private void ClickedSaveData( int index ) {
			Logger.Debug( "Start" );
			this.selectSaveDataModel.selectedSaveDataIndex.Value = index;
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 続きからボタン押下時イベント
		/// </summary>
		/// <param name="index">index</param>
		private void ClickedContinueButtonEvent( int index ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Index is {index}" );

			Logger.Warning( "未実装" );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// チャプターセレクトボタン押下時イベント
		/// </summary>
		/// <param name="index">Index</param>
		private void ClickedChapterSelectButtonEvent( int index ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Index is {index}" );

			// チャプターセレクトに遷移
			this.TransitionToChapterSelect( this.selectSaveDataModel.saveData[ index ] );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// コピーボタン押下時イベント
		/// </summary>
		/// <param name="index">index</param>
		private void ClickedCopyButtonEvent( int index ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Index is {index}" );

			Logger.Warning( "未実装" );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 削除ボタン押下時イベント
		/// </summary>
		/// <param name="index">index</param>
		private void ClickedDeleteButtonEvent( int index ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Index is {index}" );

			SinglePlaySaveDataSerializer.DeleteSinglePlaySaveData( index );

			// TODO レイアウトの更新

			Logger.Debug( "End" );
		}

		#endregion

		#region Modelイベント

		/// <summary>
		/// 選択されたセーブデータ変更時イベント
		/// </summary>
		/// <param name="index">選択されたセーブデータのIndex</param>
		private void ChangedSelectSaveData( int? index ) {
			Logger.Debug( "Start" );

			if( index.HasValue ) {
				if( !this.selectSaveDataModel.saveData[ index.Value ].exsitsAlreadyData ) {

					// 値がある場合でセーブデータがない場合はセーブデータを作成し遷移
					this.selectSaveDataModel.saveData[ index.Value ] = this.ConvertSaveDataModel( index.Value , this.CreateSinglePlaySaveData( index.Value ) );
					Logger.Warning( "未実装" );

				}
				else {

					// 値がある場合でセーブデータがある場合はパネルを表示
					this.selectSaveDataView.ShowPanel( index , this.ConvertSaveDataOfView( index.Value ) );
					this.selectSaveDataView.SetSelectedButtonInPanel( index.Value );

				}
			}
			else {
				// 値がない場合パネルを非表示にする
				this.selectSaveDataView.ShowPanel( null , null );
			}
			Logger.Debug( "End" );
		}

		/// <summary>
		/// キャンセルボタン押下時イベント
		/// </summary>
		/// <param name="value">値</param>
		private void ChangedCancelButton( int value ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Value is {value}." );
			if( value == 1 ) {
				Logger.Warning( $"Selected Save Data Index is {( this.selectSaveDataModel.selectedSaveDataIndex.HasValue ? this.selectSaveDataModel.selectedSaveDataIndex.Value.ToString() : "Null" )}." );
				// パネルが非表示ならタイトルに戻る
				if( !this.selectSaveDataModel.selectedSaveDataIndex.Value.HasValue ) {
					Logger.Debug( "Panel Don't show" );
					TitleParameter parameter = new TitleParameter() {
						InitialTitlePart = TitleParameter.InitialTitlePartEnum.MainMenu
					};
					SceneManager.GetInstance().LoadScene( "Title" , parameter );
				}
				// パネルが表示されているならパネルを非表示にする
				else {
					Logger.Debug( "Close Panel" );
					this.selectSaveDataView.SetSelectedSaveData( this.selectSaveDataView.saves[ this.selectSaveDataModel.selectedSaveDataIndex.Value.Value ] );
					this.selectSaveDataModel.selectedSaveDataIndex.Value = null;
				}
			}
			Logger.Debug( "End" );
		}

		#endregion
		
	}

}
