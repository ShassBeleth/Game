using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Views.ChapterSelect {

	/// <summary>
	/// チャプター選択View
	/// </summary>
	public class ChapterSelectView : MonoBehaviour {

		/// <summary>
		/// Event System
		/// </summary>
		public EventSystem eventSystem;

		/// <summary>
		/// チャプター
		/// </summary>
		public class Chapter {

			/// <summary>
			/// Id
			/// </summary>
			public int Id { set; get; }

			/// <summary>
			/// チャプター名
			/// </summary>
			public string Name { set; get; }

			/// <summary>
			/// 決定ボタン押下時イベントハンドラ
			/// </summary>
			public Action OnClickDecisionButtonEventHandler { set; get; }

			/// <summary>
			/// 時系列順
			/// </summary>
			public int TimelineOrder { set; get; }

			/// <summary>
			/// チャプター順
			/// </summary>
			public int NumberOrder { set; get; }

			/// <summary>
			/// タイムラインに表示するかどうか
			/// </summary>
			public bool IsShownTimeline { set; get; }

			/// <summary>
			/// スクロールに表示するかどうか
			/// </summary>
			public bool IsShownScrollView { set; get; }

		}

		/// <summary>
		/// 選択中のチャプターIdを取得
		/// </summary>
		/// <returns></returns>
		public int GetSelectedChapterId() {
			Logger.Debug( "Start" );
			GameObject current = this.eventSystem.currentSelectedGameObject;
			ScrollChapterNodeView view = current.GetComponent<ScrollChapterNodeView>();
			int id = view.Id;
			Logger.Debug( $"Selected Chapter Id is {id}.");
			Logger.Debug( "End" );
			return id;
		}
		
		#region Timeline

		/// <summary>
		/// Timeline Content
		/// </summary>
		public GameObject TimelineContent;

		/// <summary>
		/// TimelineのNode
		/// </summary>
		public GameObject TimelineNodePrefab;
		
		/// <summary>
		/// タイムラインを設定する
		/// </summary>
		/// <param name="chapterList">Chapter List</param>
		public void SetTimelineContent( List<int> chapterList ) {
			Logger.Debug( "Start" );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 指定したIDの選択肢を強制的に選択する
		/// </summary>
		/// <param name="id">Chapter Id</param>
		public void SetSelectedTimelineNode( int id ) {
			Logger.Debug( "Start" );

			Logger.Debug( "End" );
		}
		
		#endregion

		#region Scroll View

		/// <summary>
		/// Scroll View Content
		/// </summary>
		public GameObject ScrollViewContent;

		/// <summary>
		/// スクロールのNode
		/// </summary>
		public GameObject ScrollViewNodePrefab;

		/// <summary>
		/// Scroll Viewを設定する
		/// </summary>
		/// <param name="chapterList">Chapter List</param>
		public void SetScrollViewContent( List<Chapter> chapterList ) {
			Logger.Debug( "Start" );

			List<GameObject> chapterGameObjects = new List<GameObject>();

			chapterList
				// チャプター順に並び替え
				.OrderBy( (chapter) => chapter.NumberOrder ).ToList()
				// 一覧から子要素生成
				.ForEach( ( chapter ) => {

					GameObject node = GameObject.Instantiate( this.ScrollViewNodePrefab );
					node.transform.SetParent( this.ScrollViewContent.transform , false );
					chapterGameObjects.Add( node );
				
					ScrollChapterNodeView view = node.GetComponent<ScrollChapterNodeView>();
					view.Id = chapter.Id;
					view.OnClickDecisionButtonEventHandler = chapter.OnClickDecisionButtonEventHandler;
					view.SetText( chapter.Name );

					Button button = node.GetComponent<Button>();
					button.interactable = chapter.IsShownScrollView;

				} );
			
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 指定したIDの選択肢を強制的に選択する
		/// </summary>
		/// <param name="id">Chapter Id</param>
		public void SetSelectedScrollNode( int id ) {
			Logger.Debug( "Start" );
			GameObject selectedObject = null;
			foreach( Transform childTransform in this.ScrollViewContent.transform ) {
				ScrollChapterNodeView view = childTransform.GetComponent<ScrollChapterNodeView>();
				if( view.Id == id ) {
					selectedObject = view.gameObject;
					break;
				}
			}
			if( selectedObject == null ) {
				Logger.Warning( "Selected Object is Null." );
				return;
			}
			this.eventSystem.SetSelectedGameObject( selectedObject );
			Logger.Debug( "End" );
		}
		
		#endregion

		#region 詳細テキスト

		/// <summary>
		/// 詳細テキスト
		/// </summary>
		public Text DetailText;

		/// <summary>
		/// 詳細テキストの設定
		/// </summary>
		/// <param name="text">テキスト</param>
		public void SetDetailText( string text ) {
			Logger.Debug( "Start" );
			this.DetailText.text = text;
			Logger.Debug( "End" );
		}

		#endregion

	}

}