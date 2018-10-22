using UniRx;

namespace Models.Title {

	/// <summary>
	/// 遷移先指定Model
	/// </summary>
	public class NextSceneModel {

		/// <summary>
		/// 遷移先シーン名
		/// </summary>
		public ReactiveProperty<NextSceneNameEnum> nextSceneName;

		/// <summary>
		/// 一人プレイかどうか
		/// </summary>
		public bool IsSingleMode = false;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public NextSceneModel() {
			Logger.Debug( "Start" );
			this.nextSceneName = new ReactiveProperty<NextSceneNameEnum>();
			Logger.Debug( "End" );
		}

	}
}
