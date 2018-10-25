using UnityEngine;

namespace Views.Customize {

	/// <summary>
	/// キャラクターを回転させるView
	/// </summary>
	public class ShowcaseView : MonoBehaviour {

		/// <summary>
		/// 角度増加量
		/// </summary>
		public float IncreaseAngle { set; get; }

		/// <summary>
		/// 角度のデフォルト増加量
		/// </summary>
		private float DefaultIncreaseAngle { set; get; } = 0.5f;

		/// <summary>
		/// 入力されたかどうか
		/// </summary>
		public bool IsInput { set; get; } = false;

		void Update() {

			// 入力がなければ回転し続ける
			if( !this.IsInput ) {
				this.IncreaseAngle = this.DefaultIncreaseAngle;
			}

			this.transform.Rotate( new Vector3( 0f , this.IncreaseAngle , 0f ) );

		}

	}

}