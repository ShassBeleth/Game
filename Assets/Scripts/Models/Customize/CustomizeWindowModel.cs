using UniRx;

namespace Models.Customize {

	/// <summary>
	/// カスタマイズのWindowの切り替え状態を保持するModel
	/// </summary>
	public class CustomizeWindowModel {

		/// <summary>
		/// 選択状態
		/// </summary>
		public ReactiveProperty<SelectableNameEnum> SelectableName { set; get; }

		/// <summary>
		/// 遷移元状態
		/// </summary>
		public SelectableNameEnum BeforeSelectableName { set; get; }

		/// <summary>
		/// 詳細窓表示
		/// </summary>
		public ReactiveProperty<bool> IsShownDetail { set; get; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="selectableName">選択状態</param>
		public CustomizeWindowModel( SelectableNameEnum selectableName ) {
			this.LogDebug( "Start" );
			this.SelectableName = new ReactiveProperty<SelectableNameEnum>( selectableName );
			this.BeforeSelectableName = SelectableNameEnum.None;
			this.IsShownDetail = new ReactiveProperty<bool>( false );
			this.LogDebug( "End" );
		}

	}

}
