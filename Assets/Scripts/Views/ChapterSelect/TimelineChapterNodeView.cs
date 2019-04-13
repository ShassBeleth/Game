using UnityEngine;
using UnityEngine.UI;

namespace Views.ChapterSelect {

	/// <summary>
	/// タイムライン上のチャプターのView
	/// </summary>
	public class TimelineChapterNodeView : MonoBehaviour{

		/// <summary>
		/// Id
		/// </summary>
		public int Id { set; get; }

		/// <summary>
		/// 選択状態かどうか
		/// </summary>
		public bool IsSelected { set; get; }

		// 仮
		public void Update() {
			Image image = this.GetComponent<Image>();
			if( this.IsSelected ) {
				image.color = Color.red;
			}
			else {
				image.color = Color.blue;
			}
		}

		/// <summary>
		/// Nodeの座標を設定
		/// </summary>
		/// <param name="coodinate">座標</param>
		public void SetNodeCoodinate( int coodinate ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Coodinate is {coodinate}." );
			RectTransform rect = this.GetComponent<RectTransform>();
			RectTransform parentRect = this.transform.parent.GetComponent<RectTransform>();
			rect.localPosition = new Vector3( (float)coodinate , 0f , parentRect.localPosition.z );
			this.LogDebug( "End" );
		}

		/// <summary>
		/// ライン上の座標を設定
		/// </summary>
		/// <param name="coodinate">座標</param>
		public void SetCoodinateOnLine( int coodinate ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Coodinate is {coodinate}." );
			this.LogDebug( "End" );
		}

	}

}
