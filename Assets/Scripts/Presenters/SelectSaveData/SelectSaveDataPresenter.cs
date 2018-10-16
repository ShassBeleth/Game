using System;
using System.Collections.Generic;
using Saves.Models;
using Saves.Serializers;
using SceneManagers;
using SceneManagers.Parameters;
using UnityEngine;
using Views.SelectSaveData;

namespace Presenters.SelectSaveData {

	/// <summary>
	/// セーブデータ選択Presenter
	/// </summary>
	public class SelectSaveDataPresenter {

		/// <summary>
		/// セーブデータ選択View
		/// </summary>
		private SelectSaveDataView selectSaveDataView;

		/// <summary>
		/// 一人プレイかどうか
		/// </summary>
		private bool isSinglePlayMode;

		/// <summary>
		/// セーブデータモデル一覧
		/// </summary>
		private List<SinglePlaySaveDataModel> saveDataModels;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="parameter">前画面から引き継ぐパラメータ</param>
		public SelectSaveDataPresenter( SelectSaveDataParameter parameter ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Single Play Mode is {parameter.IsSinglePlayMode}" );

			this.isSinglePlayMode = parameter.IsSinglePlayMode;
			
			// hierarchyからViewを取得
			this.selectSaveDataView = GameObject.Find( "Canvas" ).GetComponent<SelectSaveDataView>();

			// セーブデータ選択ViewのEventHandler設定
			this.selectSaveDataView.OnClickBackButtonEventHandler = this.ClickedBackButtonEvent;

			// セーブデータ取得
			this.saveDataModels = new List<SinglePlaySaveDataModel>() {
				SinglePlaySaveDataSerializer.LoadSinglePlaySaveData( 0 ) ,
				SinglePlaySaveDataSerializer.LoadSinglePlaySaveData( 1 ) ,
				SinglePlaySaveDataSerializer.LoadSinglePlaySaveData( 2 ) ,
				SinglePlaySaveDataSerializer.LoadSinglePlaySaveData( 3 )
			};

			// Viewに必要な情報に加工
			List<SelectSaveDataView.SaveData> saveDataList = new List<SelectSaveDataView.SaveData>() {
				this.ConvertSaveData( 0 , this.saveDataModels[0] ) ,
				this.ConvertSaveData( 1 , this.saveDataModels[1] ) ,
				this.ConvertSaveData( 2 , this.saveDataModels[2] ) ,
				this.ConvertSaveData( 3 , this.saveDataModels[3] )
			};

			// セーブデータリストの描画
			this.selectSaveDataView.ShowSaveDataList( saveDataList );
			
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 読み込んだセーブデータをViewに渡す形に変換する
		/// </summary>
		/// <param name="id">ID</param>
		/// <param name="singlePlaySaveDataModel">読み込んだセーブデータ</param>
		/// <returns>Viewに表示するセーブデータ</returns>
		private SelectSaveDataView.SaveData ConvertSaveData( int id , SinglePlaySaveDataModel singlePlaySaveDataModel ) {
			Logger.Debug( "Start" );

			Logger.Debug( $"Exists Already Data is {( singlePlaySaveDataModel != null )}" );
			if( singlePlaySaveDataModel != null ) {
				Logger.Debug( $"Id is {id}" );
				Logger.Debug( $"User Name is {singlePlaySaveDataModel.userName}" );
				Logger.Debug( $"Latest Update Date Time is {singlePlaySaveDataModel.latestUpdateDateTime}" );
			}

			SelectSaveDataView.SaveData saveData = new SelectSaveDataView.SaveData() {
				ExistsAlreadyData = ( singlePlaySaveDataModel != null ) ,
				Id = id ,
				userName = singlePlaySaveDataModel?.userName ,
				latestUpdateDateTime = singlePlaySaveDataModel?.latestUpdateDateTime ?? new DateTime() ,
				OnClickNewButtonEventHandler = () => this.ClickedNewButtonEvent( id ) ,
				OnClickDecisionButtonEventHandler = () => this.ClickedDecisionButtonEvent( id ) ,
				OnClickCopyButtonEventHandler = () => this.ClickedCopyButtonEvent( id ) ,
				OnClickDeleteButtonEventHandler = () => this.ClickedDeleteButtonEvent( id )
			};

			Logger.Debug( "End" );
			return saveData;
		}

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		private void ClickedBackButtonEvent() {
			Logger.Debug( "Start" );
			SceneManager.GetInstance().LoadScene( 
				"Title" , 
				new TitleParameter() {
					InitialTitlePart = TitleParameter.InitialTitlePartEnum.MainMenu
				}
			);
			Logger.Debug( "End" );
		}
		
		/// <summary>
		/// セーブデータを作成する
		/// </summary>
		/// <param name="id">セーブデータID</param>
		/// <returns>作成したセーブデータModel</returns>
		private SinglePlaySaveDataModel CreateSinglePlaySaveData( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Id is {id}" );

			SinglePlaySaveDataModel model = new SinglePlaySaveDataModel() {
				id = id ,
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

			this.saveDataModels[ id ] = model;

			// セーブデータに書き込み
			SinglePlaySaveDataSerializer.WriteSinglePlaySaveData( id , model );

			Logger.Debug( "End" );
			return model;
		}

		/// <summary>
		/// チャプター選択画面へ遷移
		/// </summary>
		/// <param name="saveDataModel">セーブデータモデル</param>
		private void TransitionToChapterSelect( SinglePlaySaveDataModel saveDataModel ) {
			Logger.Debug( "Start" );

			ChapterSelectParameter parameter = new ChapterSelectParameter() {
				Id = saveDataModel.id ,
				IsSinglePlayMode = this.isSinglePlayMode
			};
			parameter.ClearedChapters = new List<ChapterSelectParameter.Chapter>();
			Logger.Debug( $"Cleared Chapters Length is {( saveDataModel.clearedChapters?.Length ?? 0 )}" );
			if( saveDataModel.clearedChapters != null ) {
				foreach( ChapterSaveDataModel chapter in saveDataModel.clearedChapters ) {
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

		/// <summary>
		/// 新規作成ボタン押下時イベント
		/// </summary>
		/// <param name="id">セーブデータID</param>
		private void ClickedNewButtonEvent( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Id is {id}" );

			// 新規にセーブデータを作成する
			this.saveDataModels[ id ] = this.CreateSinglePlaySaveData( id );

			// チャプターセレクトに遷移
			this.TransitionToChapterSelect( this.saveDataModels[ id ] );

			Logger.Debug( "End" );
		}
		
		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		/// <param name="id">セーブデータID</param>
		private void ClickedDecisionButtonEvent( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Id is {id}" );

			// チャプターセレクトに遷移
			this.TransitionToChapterSelect( this.saveDataModels[ id ] );
			
			Logger.Debug( "End" );
		}

		/// <summary>
		/// コピーボタン押下時イベント
		/// </summary>
		/// <param name="id">セーブデータID</param>
		private void ClickedCopyButtonEvent( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Id is {id}" );

			Logger.Warning( "未実装" );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 削除ボタン押下時イベント
		/// </summary>
		/// <param name="id">セーブデータID</param>
		private void ClickedDeleteButtonEvent( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Id is {id}" );

			SinglePlaySaveDataSerializer.DeleteSinglePlaySaveData( id );

			// TODO レイアウトの更新

			Logger.Debug( "End" );
		}

	}

}
