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

			/// <summary>
			/// ライン上の座標
			/// </summary>
			public int CoodinateOnLine { set; get; }

			/// <summary>
			/// ノードの座標
			/// </summary>
			public int NodeCoodinate { set; get; }

		}

		/// <summary>
		/// 選択中のチャプターIdを取得
		/// </summary>
		/// <returns></returns>
		public int GetSelectedChapterId() {
			this.LogDebug( "Start" );
			GameObject current = this.eventSystem.currentSelectedGameObject;
			ScrollChapterNodeView view = current.GetComponent<ScrollChapterNodeView>();
			int id = view.Id;
			this.LogDebug( $"Selected Chapter Id is {id}.");
			this.LogDebug( "End" );
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
		public void SetTimelineContent( List<Chapter> chapterList ) {
			this.LogDebug( "Start" );
			chapterList.ForEach( ( chapter ) => {

				GameObject node = GameObject.Instantiate( this.TimelineNodePrefab );
				node.transform.SetParent( this.TimelineContent.transform , false );

				TimelineChapterNodeView view = node.GetComponent<TimelineChapterNodeView>();
				view.Id = chapter.Id;
				view.IsSelected = false;
				view.SetNodeCoodinate( chapter.NodeCoodinate );
				view.SetCoodinateOnLine( chapter.CoodinateOnLine );
				
			} );

			this.LogDebug( "End" );
		}

		/// <summary>
		/// 指定したIDの選択肢を強制的に選択する
		/// </summary>
		/// <param name="id">Chapter Id</param>
		public void SetSelectedTimelineNode( int id ) {
			this.LogDebug( "Start" );
			foreach( Transform childTransform in this.TimelineContent.transform ) {
				TimelineChapterNodeView view = childTransform.GetComponent<TimelineChapterNodeView>();
				if( view == null ) {
					this.LogDebug( "Continue" );
					continue;
				}
				view.IsSelected = view.Id == id;
			}
			this.LogDebug( "End" );
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
			this.LogDebug( "Start" );

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
			
			this.LogDebug( "End" );
		}

		/// <summary>
		/// 指定したIDの選択肢を強制的に選択する
		/// </summary>
		/// <param name="id">Chapter Id</param>
		public void SetSelectedScrollNode( int id ) {
			this.LogDebug( "Start" );
			GameObject selectedObject = null;
			foreach( Transform childTransform in this.ScrollViewContent.transform ) {
				ScrollChapterNodeView view = childTransform.GetComponent<ScrollChapterNodeView>();
				if( view.Id == id ) {
					selectedObject = view.gameObject;
					break;
				}
			}
			if( selectedObject == null ) {
				this.LogWarning( "Selected Object is Null." );
				return;
			}
			this.eventSystem.SetSelectedGameObject( selectedObject );
			this.LogDebug( "End" );
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
			this.LogDebug( "Start" );
			this.DetailText.text = text;
			this.LogDebug( "End" );
		}

		#endregion

	}

}