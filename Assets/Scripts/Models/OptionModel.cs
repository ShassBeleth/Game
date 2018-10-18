using UniRx;

namespace Models {

	/// <summary>
	/// オプションModel
	/// </summary>
	public class OptionModel {

		/// <summary>
		/// 垂直方向のカメラ移動を反転させるかどうか
		/// </summary>
		public ReactiveProperty<bool> IsReverseVerticalCamera;

		/// <summary>
		/// 水平方向のカメラ移動を反転させるかどうか
		/// </summary>
		public ReactiveProperty<bool> IsReverseHorizontalCamera;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="isReverseVerticalCamera">垂直方向のカメラ移動を反転させるかどうか</param>
		/// <param name="isReverseHorizontalCamera">水平方向のカメラ移動を反転させるかどうか</param>
		public OptionModel(
			bool isReverseVerticalCamera ,
			bool isReverseHorizontalCamera
		) {
			Logger.Debug( "Start" );

			this.IsReverseVerticalCamera = new ReactiveProperty<bool>( isReverseVerticalCamera );
			this.IsReverseHorizontalCamera = new ReactiveProperty<bool>( isReverseHorizontalCamera );

			Logger.Debug( "End" );
		}

	}

}
