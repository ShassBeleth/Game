using System.Collections.Generic;
using System.Linq;
using Models.Chapter;
using SceneManagers;
using SceneManagers.Parameters;
using UniRx;
using UnityEngine;
using Views.ChapterSelect;
using Views.UserController;

namespace Presenters.ChapterSelect {

	/// <summary>
	/// チャプター選択Presenter
	/// </summary>
	public class ChapterSelectPresenter {

		#region Model

		/// <summary>
		/// チャプターリスト
		/// </summary>
		private List<ChapterModel> chapters = new List<ChapterModel>();

		/// <summary>
		/// 選択中のチャプターID
		/// </summary>
		private ReactiveProperty<int> selectedChapterId = new ReactiveProperty<int>();
		
		#endregion

		#region View

		/// <summary>
		/// チャプター選択View
		/// </summary>
		private ChapterSelectView chapterSelectView;

		/// <summary>
		/// User Controller View
		/// </summary>
		private UserControllerView userControllerView;

		#endregion

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
			Logger.Debug( $"Id is {parameter.SaveDataId}" );
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
			
			// Viewの初期設定
			this.InitialViewSetting();

			// 全チャプターの情報をリストで保持
			this.chapters = this.GetChapterData( parameter.SaveDataId );
			this.chapterSelectView.SetScrollViewContent( this.ConvertChapterModel( this.chapters ) );
			// タイムラインにはクリアした項目のみ渡す
			this.chapterSelectView.SetTimelineContent( 
				this.ConvertChapterModel( 
					this.chapters.Where( chapter => chapter.IsCleared ).ToList() 
				) 
			);
			
			// ModelのSubscribeを設定
			this.InitialModelSubscribeSetting();
			
			Logger.Debug( "End" );

		}

		#region 変換

		/// <summary>
		/// ModelのリストをViewに渡せる形に変換
		/// </summary>
		/// <param name="models">Modelのリスト</param>
		/// <returns>Viewに渡せる形に変換したリスト</returns>
		private List<ChapterSelectView.Chapter> ConvertChapterModel( List<ChapterModel> models ) {
			Logger.Debug( "Start" );

			List<ChapterSelectView.Chapter> list = new List<ChapterSelectView.Chapter>();
			foreach( int i in Enumerable.Range( 0 , models.Count ) ) {
				int index = i;
				list.Add( new ChapterSelectView.Chapter() {
					Id = models[ index ].Id ,
					Name = models[ index ].Name ,
					NumberOrder = models[ index ].NumberOrder ,
					TimelineOrder = models[ index ].TimelineOrder ,
					OnClickDecisionButtonEventHandler = () => this.ClickedDecisionButtonEvent( models[ index ].Id ) ,
					IsShownScrollView = models[ index ].IsCleared ,
					IsShownTimeline = models[ index ].IsCleared ,
					NodeCoodinate = models[ index ].NodeCoodinate ,
					CoodinateOnLine = models[ index ].CoodinateOnLine
				} );
			}
			Logger.Debug( "End" );
			return list;
		}

		#endregion

		#region 初期設定

		/// <summary>
		/// Viewの設定
		/// </summary>
		private void InitialViewSetting() {
			Logger.Debug( "Start" );
			
			// Viewを取得
			this.chapterSelectView = GameObject.Find( "Canvas" ).GetComponent<ChapterSelectView>();
			this.userControllerView = GameObject.Find( "UserController" ).GetComponent<UserControllerView>();

			Logger.Debug( "End" );
		}

		/// <summary>
		/// ModelのSubscribeを設定
		/// </summary>
		private void InitialModelSubscribeSetting() {
			Logger.Debug( "Start" );
			this.selectedChapterId.Subscribe( ( id ) => { this.ChangedSelectedChapterId( id ); } );
			this.userControllerView.MenuButtons[ "CursorUp" ].Subscribe( value => this.ChangedCursorUp( value ) );
			this.userControllerView.MenuButtons[ "CursorDown" ].Subscribe( value => this.ChangedCursorDown( value ) );
			this.userControllerView.MenuButtons[ "CursorLeft" ].Subscribe( value => this.ChangedCursorLeft( value ) );
			this.userControllerView.MenuButtons[ "CursorRight" ].Subscribe( value => this.ChangedCursorRight( value ) );
			this.userControllerView.MenuButtons[ "Cancel" ].Subscribe( value => this.ChangedCancel( value ) );
			Logger.Debug( "End" );
		}

		#endregion

		#region ModelのSubscribeによるイベント

		/// <summary>
		/// 選択中のチャプターId変更時イベント
		/// </summary>
		/// <param name="id">チャプターId</param>
		private void ChangedSelectedChapterId( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Chapter Id is {id}." );
			this.chapterSelectView.SetSelectedScrollNode( id );
			this.chapterSelectView.SetSelectedTimelineNode( id );
			this.chapterSelectView.SetDetailText( 
				this.chapters?.FirstOrDefault( (chapter) => chapter.Id == id )?.Detail ?? ""
			);
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 十字上変更時イベント
		/// </summary>
		/// <param name="value">値</param>
		private void ChangedCursorUp( int value ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Value is {value}." );

			// イベント実行条件
			// ・初回1フレーム目
			// ・Event Systemに合わせて 0.5秒(30F)押し続けた場合に10フレーム毎に1回
			if( !( value == 1 || ( 30 < value && value % 10 == 0 ) ) ) {
				return;
			}

			int selectedChapterId = this.chapterSelectView.GetSelectedChapterId();
			Logger.Debug( $"Selected Chapter Id is {selectedChapterId}." );

			// チャプター順にソート
			IOrderedEnumerable<ChapterModel> sortedChapters
				= this.chapters.OrderByDescending( chapter => chapter.NumberOrder );

			// 選択中のチャプターの次のチャプターを取得
			ChapterModel nextObject = sortedChapters
				// 選択中のチャプターまでスキップ
				.SkipWhile( chapter => chapter.Id != selectedChapterId )
				// 選択中のチャプターをスキップ
				.Skip( 1 )
				// 先頭を取得
				.FirstOrDefault();

			// 選択中のチャプターが一覧の末尾だった場合は先頭を選択
			if( nextObject == null ) {
				Logger.Debug( "Next Object is Null." );
				nextObject = sortedChapters.FirstOrDefault();
			}
			else {
				Logger.Debug( $"Next Object Id is {nextObject.Id}." );
			}

			this.selectedChapterId.Value = nextObject.Id;
			
			Logger.Debug( "End" );
		}
		/// <summary>
		/// 十字下変更時イベント
		/// </summary>
		/// <param name="value">値</param>
		private void ChangedCursorDown( int value ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Value is {value}." );

			// イベント実行条件
			// ・初回1フレーム目
			// ・Event Systemに合わせて 0.5秒(30F)押し続けた場合に10フレーム毎に1回
			if( !( value == 1 || ( 30 < value && value % 10 == 0 ) ) ) {
				return;
			}
			
			int selectedChapterId = this.chapterSelectView.GetSelectedChapterId();
			Logger.Debug( $"Selected Chapter Id is {selectedChapterId}." );

			// チャプター順にソート
			IOrderedEnumerable<ChapterModel> sortedChapters
				= this.chapters.OrderBy( chapter => chapter.NumberOrder );
			
			// 選択中のチャプターの次のチャプターを取得
			ChapterModel nextObject = sortedChapters
				// 選択中のチャプターまでスキップ
				.SkipWhile( chapter => chapter.Id != selectedChapterId )
				// 選択中のチャプターをスキップ
				.Skip( 1 )
				// 先頭を取得
				.FirstOrDefault();

			// 選択中のチャプターが一覧の末尾だった場合は先頭を選択
			if( nextObject == null ) {
				Logger.Debug( "Next Object is Null." );
				nextObject = sortedChapters.FirstOrDefault();
			}
			else {
				Logger.Debug( $"Next Object Id is {nextObject.Id}." );
			}

			this.selectedChapterId.Value = nextObject.Id;

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 十字左変更時イベント
		/// </summary>
		/// <param name="value">値</param>
		/// TODO 未クリアの項目を選択中に左右ボタン押下で先頭か末尾が選ばれてしまう
		/// 一番近いものを選択するようにする
		private void ChangedCursorLeft( int value ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Value is {value}." );

			// イベント実行条件
			// ・初回1フレーム目
			// ・Event Systemに合わせて 0.5秒(30F)押し続けた場合に10フレーム毎に1回
			if( !( value == 1 || ( 30 < value && value % 10 == 0 ) ) ) {
				return;
			}

			int selectedChapterId = this.chapterSelectView.GetSelectedChapterId();
			Logger.Debug( $"Selected Chapter Id is {selectedChapterId}." );

			IOrderedEnumerable<ChapterModel> sortedAndClearedOnlyChapters = this.chapters
				// クリア済みに絞り込む
				.Where( chapter => chapter.IsCleared )
				// 時系列順にソート
				.OrderByDescending( chapter => chapter.TimelineOrder );

			// 選択中のチャプターの次のチャプターを取得
			ChapterModel nextObject = sortedAndClearedOnlyChapters
				// 選択中のチャプターまでスキップ
				.SkipWhile( chapter => chapter.Id != selectedChapterId )
				// 選択中のチャプターをスキップ
				.Skip( 1 )
				// 先頭を取得
				.FirstOrDefault();

			// 選択中のチャプターが一覧の末尾だった場合は先頭を選択
			if( nextObject == null ) {
				Logger.Debug( "Next Object is Null." );
				nextObject = sortedAndClearedOnlyChapters.FirstOrDefault();
			}
			else {
				Logger.Debug( $"Next Object Id is {nextObject.Id}." );
			}

			this.selectedChapterId.Value = nextObject.Id;
			
			Logger.Debug( "End" );
		}
		
		/// <summary>
		/// 十字右変更時イベント
		/// </summary>
		/// <param name="value">値</param>
		/// TODO 未クリアの項目を選択中に左右ボタン押下で先頭か末尾が選ばれてしまう
		/// 一番近いものを選択するようにする
		private void ChangedCursorRight( int value ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Value is {value}." );

			// イベント実行条件
			// ・初回1フレーム目
			// ・Event Systemに合わせて 0.5秒(30F)押し続けた場合に10フレーム毎に1回
			if( !( value == 1 || ( 30 < value && value % 10 == 0 ) ) ) {
				return;
			}
			
			int selectedChapterId = this.chapterSelectView.GetSelectedChapterId();
			Logger.Debug( $"Selected Chapter Id is {selectedChapterId}." );

			IOrderedEnumerable<ChapterModel> sortedAndClearedOnlyChapters = this.chapters
				// クリア済みに絞り込む
				.Where( chapter => chapter.IsCleared )
				// 時系列順にソート
				.OrderBy( chapter => chapter.TimelineOrder );

			// 選択中のチャプターの次のチャプターを取得
			ChapterModel nextObject = sortedAndClearedOnlyChapters
				// 選択中のチャプターまでスキップ
				.SkipWhile( chapter => chapter.Id != selectedChapterId )
				// 選択中のチャプターをスキップ
				.Skip( 1 )
				// 先頭を取得
				.FirstOrDefault();

			// 選択中のチャプターが一覧の末尾だった場合は先頭を選択
			if( nextObject == null ) {
				Logger.Debug( "Next Object is Null." );
				nextObject = sortedAndClearedOnlyChapters.FirstOrDefault();
			}
			else {
				Logger.Debug( $"Next Object Id is {nextObject.Id}." );
			}

			this.selectedChapterId.Value = nextObject.Id;
			
			Logger.Debug( "End" );
		}

		/// <summary>
		/// キャンセルボタン押下時イベント
		/// </summary>
		/// <param name="value">値</param>
		private void ChangedCancel( int value ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Value is {value}." );

			if( value != 1 ) {
				return;
			}

			SceneManager.GetInstance().LoadScene( "SelectSaveData" , new SelectSaveDataParameter() {
				IsSinglePlayMode = this.isSinglePlayMode
			} );

			Logger.Debug( "End" );
		}

		#endregion

		#region Viewイベント

		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		/// <param name="id">チャプターId</param>
		private void ClickedDecisionButtonEvent( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Chapter Id is {id}." );
			CustomizeParameter parameter = new CustomizeParameter() {
				
			};
			SceneManager.GetInstance().LoadScene( "Customize" , parameter );
			Logger.Debug( "End" );
		}

		#endregion

		/// <summary>
		/// チャプター内のすべての情報を取得する
		/// </summary>
		/// <param name="saveDataId">セーブデータID</param>
		/// <returns>チャプター一覧</returns>
		/// TODO サーバ等からデータを取得する　returnで返ってくる値もサーバから取ってきた形のデータモデル
		private List<ChapterModel> GetChapterData( int saveDataId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Save Data Id is {saveDataId}." );

			List<ChapterModel> list = new List<ChapterModel>();
			for(int i = 0 ; i < 100 ; i++ ) {
				list.Add( new ChapterModel() {
					Id = i ,
					Name = "Chapter1-" + i ,
					Detail = "おなかすいた" + i ,
					NumberOrder = i ,
					TimelineOrder = i,
					IsCleared = i % 2 == 0 ,
					NodeCoodinate = -600 + i * 130 ,
					CoodinateOnLine = -630 + ( i % 5 ) * 30
				} );
			}
			
			Logger.Debug( "End" );
			return list;
		}
		
	}

}