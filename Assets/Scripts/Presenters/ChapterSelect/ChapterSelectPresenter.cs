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
		private readonly ReactiveProperty<int> selectedChapterId = new ReactiveProperty<int>();
		
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
		private readonly SceneService sceneService = SceneService.GetInstance();

		/// <summary>
		/// チャプターService
		/// </summary>
		private readonly ChapterService chapterService = ChapterService.GetInstance();

		#endregion

		/// <summary>
		/// 一人プレイかどうか
		/// </summary>
		private readonly bool isSinglePlayMode;

		/// <summary>
		/// セーブデータID
		/// </summary>
		private readonly int SaveDataId;

		/// <summary>
		/// クリア済みチャプターID一覧
		/// </summary>
		private readonly List<int> clearedChapterIds;
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="parameter">前画面から受けとるパラメータ</param>
		public ChapterSelectPresenter( ChapterSelectParameter parameter ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Id is {parameter.SaveDataId}" );
			this.LogDebug( $"Single Play Mode is {parameter.IsSinglePlayMode}" );
			// デバッグ用
			{
				string cleardChapterIds = "";
				parameter.ClearedChapters.ForEach(
					( chapter ) => {
						cleardChapterIds += chapter.Id.ToString() + " ";
					}
				);
				this.LogWarning( $"Cleared Chapter Id is {cleardChapterIds}" );
			}

			this.isSinglePlayMode = parameter.IsSinglePlayMode;
			this.SaveDataId = parameter.SaveDataId;
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
			
			this.LogDebug( "End" );

		}

		#region 変換

		/// <summary>
		/// ModelのリストをViewに渡せる形に変換
		/// </summary>
		/// <param name="models">Modelのリスト</param>
		/// <returns>Viewに渡せる形に変換したリスト</returns>
		private List<ChapterSelectView.Chapter> ConvertChapterModel( List<ChapterModel> models ) {
			this.LogDebug( "Start" );

			List<ChapterSelectView.Chapter> list = new List<ChapterSelectView.Chapter>();
			foreach( int i in Enumerable.Range( 0 , models.Count ) ) {
				int index = i;
				ChapterSelectView.Chapter c = new ChapterSelectView.Chapter() {
					Id = models[ index ].Id ,
					Name = models[ index ].Name ,
					NumberOrder = models[ index ].NumberOrder ,
					TimelineOrder = 0 ,
					IsShownScrollView = models[ index ].IsCleared ,
					IsShownTimeline = models[ index ].IsCleared ,
					NodeCoodinate = 0 ,
					CoodinateOnLine = 0
				};
				c.OnClickedDecisionButtonSubject.Subscribe( _ => this.ClickedDecisionButtonEvent( models[ index ].Id ) );
				list.Add( c );
			}
			this.LogDebug( "End" );
			return list;
		}

		#endregion

		#region 初期設定

		/// <summary>
		/// Viewの設定
		/// </summary>
		private void InitialViewSetting() {
			this.LogDebug( "Start" );
			
			// Viewを取得
			this.chapterSelectView = GameObject.Find( "Canvas" ).GetComponent<ChapterSelectView>();
			this.userControllerView = GameObject.Find( "UserController" ).GetComponent<UserControllerView>();

			this.LogDebug( "End" );
		}

		/// <summary>
		/// ModelのSubscribeを設定
		/// </summary>
		private void InitialModelSubscribeSetting() {
			this.LogDebug( "Start" );
			this.selectedChapterId.Subscribe( ( id ) => { this.ChangedSelectedChapterId( id ); } );
			this.userControllerView.MenuButtons[ "CursorUp" ].Subscribe( value => this.ChangedCursorUp( value ) );
			this.userControllerView.MenuButtons[ "CursorDown" ].Subscribe( value => this.ChangedCursorDown( value ) );
			this.userControllerView.MenuButtons[ "CursorLeft" ].Subscribe( value => this.ChangedCursorLeft( value ) );
			this.userControllerView.MenuButtons[ "CursorRight" ].Subscribe( value => this.ChangedCursorRight( value ) );
			this.userControllerView.MenuButtons[ "Cancel" ].Subscribe( value => this.ChangedCancel( value ) );
			this.LogDebug( "End" );
		}

		#endregion

		#region ModelのSubscribeによるイベント

		/// <summary>
		/// 選択中のチャプターId変更時イベント
		/// </summary>
		/// <param name="id">チャプターId</param>
		private void ChangedSelectedChapterId( int id ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Chapter Id is {id}." );
			this.chapterSelectView.SetSelectedScrollNode( id );
			this.chapterSelectView.SetSelectedTimelineNode( id );
			this.chapterSelectView.SetDetailText( 
				this.chapterService.GetChapters( this.clearedChapterIds )
					.FirstOrDefault( (chapter) => chapter.Id == id ).Detail
			);
			this.LogDebug( "End" );
		}

		/// <summary>
		/// 十字上変更時イベント
		/// </summary>
		/// <param name="value">値</param>
		private void ChangedCursorUp( int value ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Value is {value}." );

			// イベント実行条件
			// ・初回1フレーム目
			// ・Event Systemに合わせて 0.5秒(30F)押し続けた場合に10フレーム毎に1回
			if( !( value == 1 || ( 30 < value && value % 10 == 0 ) ) ) {
				return;
			}

			int selectedChapterId = this.chapterSelectView.GetSelectedChapterId();
			this.LogDebug( $"Selected Chapter Id is {selectedChapterId}." );

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
				this.LogDebug( "Next Object is Null." );
				nextObject = sortedChapters.FirstOrDefault();
			}
			else {
				this.LogDebug( $"Next Object Id is {nextObject.Id}." );
			}

			this.selectedChapterId.Value = nextObject.Id;
			
			this.LogDebug( "End" );
		}
		/// <summary>
		/// 十字下変更時イベント
		/// </summary>
		/// <param name="value">値</param>
		private void ChangedCursorDown( int value ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Value is {value}." );

			// イベント実行条件
			// ・初回1フレーム目
			// ・Event Systemに合わせて 0.5秒(30F)押し続けた場合に10フレーム毎に1回
			if( !( value == 1 || ( 30 < value && value % 10 == 0 ) ) ) {
				return;
			}
			
			int selectedChapterId = this.chapterSelectView.GetSelectedChapterId();
			this.LogDebug( $"Selected Chapter Id is {selectedChapterId}." );

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
				this.LogDebug( "Next Object is Null." );
				nextObject = sortedChapters.FirstOrDefault();
			}
			else {
				this.LogDebug( $"Next Object Id is {nextObject.Id}." );
			}

			this.selectedChapterId.Value = nextObject.Id;

			this.LogDebug( "End" );
		}

		/// <summary>
		/// 十字左変更時イベント
		/// </summary>
		/// <param name="value">値</param>
		/// TODO 未クリアの項目を選択中に左右ボタン押下で先頭か末尾が選ばれてしまう
		/// 一番近いものを選択するようにする
		private void ChangedCursorLeft( int value ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Value is {value}." );

			// イベント実行条件
			// ・初回1フレーム目
			// ・Event Systemに合わせて 0.5秒(30F)押し続けた場合に10フレーム毎に1回
			if( !( value == 1 || ( 30 < value && value % 10 == 0 ) ) ) {
				return;
			}

			int selectedChapterId = this.chapterSelectView.GetSelectedChapterId();
			this.LogDebug( $"Selected Chapter Id is {selectedChapterId}." );

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
				this.LogDebug( "Next Object is Null." );
				nextObject = sortedAndClearedOnlyChapters.FirstOrDefault();
			}
			else {
				this.LogDebug( $"Next Object Id is {nextObject.Id}." );
			}

			this.selectedChapterId.Value = nextObject.Id;
			
			this.LogDebug( "End" );
		}
		
		/// <summary>
		/// 十字右変更時イベント
		/// </summary>
		/// <param name="value">値</param>
		/// TODO 未クリアの項目を選択中に左右ボタン押下で先頭か末尾が選ばれてしまう
		/// 一番近いものを選択するようにする
		private void ChangedCursorRight( int value ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Value is {value}." );

			// イベント実行条件
			// ・初回1フレーム目
			// ・Event Systemに合わせて 0.5秒(30F)押し続けた場合に10フレーム毎に1回
			if( !( value == 1 || ( 30 < value && value % 10 == 0 ) ) ) {
				return;
			}
			
			int selectedChapterId = this.chapterSelectView.GetSelectedChapterId();
			this.LogDebug( $"Selected Chapter Id is {selectedChapterId}." );

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
				this.LogDebug( "Next Object is Null." );
				nextObject = sortedAndClearedOnlyChapters.FirstOrDefault();
			}
			else {
				this.LogDebug( $"Next Object Id is {nextObject.Id}." );
			}

			this.selectedChapterId.Value = nextObject.Id;
			
			this.LogDebug( "End" );
		}

		/// <summary>
		/// キャンセルボタン押下時イベント
		/// </summary>
		/// <param name="value">値</param>
		private void ChangedCancel( int value ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Value is {value}." );

			if( value != 1 ) {
				return;
			}

			this.sceneService.LoadScene( "SelectSaveData" , new SelectSaveDataParameter() {
				IsSinglePlayMode = this.isSinglePlayMode
			} );

			this.LogDebug( "End" );
		}

		#endregion

		#region Viewイベント

		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		/// <param name="id">チャプターId</param>
		private void ClickedDecisionButtonEvent( int id ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Chapter Id is {id}." );
			CustomizeParameter parameter = new CustomizeParameter() {
				SaveId = this.SaveDataId
			};
			this.sceneService.LoadScene( "Customize" , parameter );
			this.LogDebug( "End" );
		}

		#endregion
		
	}

}