using System.Collections.Generic;
using System.Linq;
using Models.Chapter;
using Services.Chapters;
using Services.Scenes;
using Services.Scenes.Parameters;
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

		#region Service

		/// <summary>
		/// シーンService
		/// </summary>
		private SceneService sceneService = SceneService.GetInstance();

		/// <summary>
		/// チャプターService
		/// </summary>
		private ChapterService chapterService = ChapterService.GetInstance();

		#endregion

		/// <summary>
		/// 一人プレイかどうか
		/// </summary>
		private bool isSinglePlayMode;

		/// <summary>
		/// クリア済みチャプターID一覧
		/// </summary>
		private List<int> clearedChapterIds;
		
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
				Logger.Warning( $"Cleared Chapter Id is {cleardChapterIds}" );
			}

			this.isSinglePlayMode = parameter.IsSinglePlayMode;
			this.clearedChapterIds = parameter.ClearedChapters.Select( chapter => chapter.Id ).ToList();
			
			// Viewの初期設定
			this.InitialViewSetting();

			// スクロールにチャプター一覧を渡す
			this.chapterSelectView.SetScrollViewContent(
				this.ConvertChapterModel(
					this.chapterService.GetChapters( this.clearedChapterIds )
				) 
			);
			// タイムラインにはクリアした項目のみ渡す
			this.chapterSelectView.SetTimelineContent( 
				this.ConvertChapterModel( 
					this.chapterService.GetChapters( 
						this.clearedChapterIds
					)
					.Where( chapter => chapter.IsCleared ).ToList() 
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
					TimelineOrder = 0 ,
					OnClickDecisionButtonEventHandler = () => this.ClickedDecisionButtonEvent( models[ index ].Id ) ,
					IsShownScrollView = models[ index ].IsCleared ,
					IsShownTimeline = models[ index ].IsCleared ,
					NodeCoodinate = 0 ,
					CoodinateOnLine = 0
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
				this.chapterService.GetChapters( this.clearedChapterIds )
					.FirstOrDefault( (chapter) => chapter.Id == id ).Detail
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
				= this.chapterService
					.GetChapters( this.clearedChapterIds )
					.OrderByDescending( chapter => chapter.NumberOrder );

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
				= this.chapterService.GetChapters( this.clearedChapterIds )
					.OrderBy( chapter => chapter.NumberOrder );
			
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

			IOrderedEnumerable<ChapterModel> sortedAndClearedOnlyChapters
				= this.chapterService.GetChapters( this.clearedChapterIds )
					// クリア済みに絞り込む
					.Where( chapter => chapter.IsCleared )
					// 時系列順にソート
					.OrderByDescending( chapter => 0 );

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

			IOrderedEnumerable<ChapterModel> sortedAndClearedOnlyChapters
				= this.chapterService.GetChapters( this.clearedChapterIds )
					// クリア済みに絞り込む
					.Where( chapter => chapter.IsCleared )
					// 時系列順にソート
					.OrderBy( chapter => 0 );

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

			this.sceneService.LoadScene( "SelectSaveData" , new SelectSaveDataParameter() {
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
			this.sceneService.LoadScene( "Customize" , parameter );
			Logger.Debug( "End" );
		}

		#endregion
		
	}

}