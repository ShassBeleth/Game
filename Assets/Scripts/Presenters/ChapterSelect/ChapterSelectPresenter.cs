using System.Collections.Generic;
using System.Linq;
using Saves.Models;
using SceneManagers;
using SceneManagers.Parameters;
using UnityEngine;
using Views.ChapterSelect;

namespace Presenters.ChapterSelect {

	/// <summary>
	/// チャプター選択Presenter
	/// </summary>
	public class ChapterSelectPresenter {

		/// <summary>
		/// チャプター選択View
		/// </summary>
		private ChapterSelectView chapterSelectView;

		/// <summary>
		/// 一人プレイかどうか
		/// </summary>
		private bool isSinglePlayMode;
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="parameter">前画面から受けとるパラメータ</param>
		public ChapterSelectPresenter( ChapterSelectParameter parameter ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Id is {parameter.Id}" );
			Logger.Debug( $"Single Play Mode is {parameter.IsSinglePlayMode}" );
			// デバッグ用
			{
				string cleardChapterIds = "";
				parameter.ClearedChapters.ForEach(
					( chapter ) => {
						cleardChapterIds += chapter.Id.ToString() + " ";
					}
				);
				Logger.Debug( $"Cleared Chapter Id is {cleardChapterIds}" );
			}

			this.isSinglePlayMode = parameter.IsSinglePlayMode;

			// hierarchyからViewを取得
			this.chapterSelectView = GameObject.Find( "Canvas" ).GetComponent<ChapterSelectView>();

			// ChapterSelectViewのEventHandler設定
			this.chapterSelectView.OnClickBackButtonEventHandler = this.OnClickBackButtonEvent;
			
			// TODO サーバから全チャプターの情報を取得
			List<ChapterSaveDataModel> chapterSaveDataModel = this.GetChapterData();
			List<ChapterSelectView.ChapterData> chapterDataList = new List<ChapterSelectView.ChapterData>();

			// 全チャプターの情報をView用に変換する
			foreach( ChapterSaveDataModel saveData in chapterSaveDataModel ) {
				ChapterSelectView.ChapterData chapterData = new ChapterSelectView.ChapterData() {
					Id = saveData.id ,
					Name = saveData.Name ,
					IsShown = parameter.ClearedChapters.Any( chapter => chapter.Id == saveData.id )
				};
				int id = chapterData.Id;
				chapterData.OnClickDecisionButtonEventHandler = () => this.ClickedDecisionButtonEvent( id );
				chapterDataList.Add( chapterData );
			}
			
			// チャプターの情報とクリアしたチャプターの一覧から描画
			this.chapterSelectView.ShowChapterDataList( chapterDataList  );

			Logger.Debug( "End" );

		}

		/// <summary>
		/// チャプター内のすべての情報を取得する
		/// </summary>
		/// <returns>チャプター一覧</returns>
		/// TODO サーバ等からデータを取得する　returnで返ってくる値もサーバから取ってきた形のデータモデル
		private List<ChapterSaveDataModel> GetChapterData() {
			Logger.Debug( "Start" );
			List<ChapterSaveDataModel> list = new List<ChapterSaveDataModel>();
			for(int i = 0 ; i < 100 ; i++ ) {
				list.Add(
					new ChapterSaveDataModel() {
						id = i ,
						Name = "a"
					}
				);
			}
			Logger.Debug( "End" );
			return list;
		}

		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		/// <param name="id"></param>
		private void ClickedDecisionButtonEvent( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Id is {id}" );
			SceneManager.GetInstance().LoadScene(
				"Customize" ,
				null
			);
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		public void OnClickBackButtonEvent() {
			Logger.Debug( "Start" );
			SceneManager.GetInstance().LoadScene( 
				"SelectSaveData" , 
				new SelectSaveDataParameter() {
					IsSinglePlayMode = this.isSinglePlayMode
				}
			);
			Logger.Debug( "End" );
		}

	}

}