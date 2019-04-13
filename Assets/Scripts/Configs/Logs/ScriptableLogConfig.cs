using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configs.Logs {

	/// <summary>
	/// ログ設定情報
	/// </summary>
	[Serializable]
	public class ScriptableLogConfig : ScriptableObject {

		/// <summary>
		/// チェック項目
		/// </summary>
		[SerializableAttribute]
		public class Check {

			/// <summary>
			/// 有効かどうか
			/// </summary>
			[SerializeField]
			private bool isValid = false;

			/// <summary>
			/// 有効かどうか
			/// </summary>
			public bool IsValid {
				set => this.isValid = value;
				get => this.isValid;
			}

			/// <summary>
			/// コメント
			/// </summary>
			[SerializeField]
			private string comment = "";

			/// <summary>
			/// コメント
			/// </summary>
			public string Comment {
				set => this.comment = value;
				get => this.comment;
			}

			/// <summary>
			/// クラス名正規表現
			/// </summary>
			[SerializeField]
			private string regexClass = "";

			/// <summary>
			/// クラス名正規表現
			/// </summary>
			public string RegexClass {
				set => this.regexClass = value;
				get => this.regexClass;
			}

			/// <summary>
			/// メソッド名正規表現
			/// </summary>
			[SerializeField]
			private string regexMethod = "";

			/// <summary>
			/// メソッド名正規表現
			/// </summary>
			public string RegexMethod {
				set => this.regexMethod = value;
				get => this.regexMethod;
			}

			/// <summary>
			/// メッセージ正規表現
			/// </summary>
			[SerializeField]
			private string regexMessage = "";

			/// <summary>
			/// メッセージ正規表現
			/// </summary>
			public string RegexMessage {
				set => this.regexMessage = value;
				get => this.regexMessage;
			}

			/// <summary>
			/// アクション種別
			/// </summary>
			public enum ActionType {
				Hide,
				Color,
				Bold
			}

			/// <summary>
			/// アクション種別
			/// </summary>
			[SerializeField]
			private ActionType type;

			/// <summary>
			/// アクション種別
			/// </summary>
			public ActionType Type {
				get => this.type;
				set => this.type = value;
			}

			/// <summary>
			/// 色
			/// </summary>
			[SerializeField]
			private Color color;

			/// <summary>
			/// 色
			/// </summary>
			public Color Color {
				get => this.color;
				set => this.color = value;
			}

		}

		/// <summary>
		/// チェックリスト
		/// </summary>
		[SerializeField]
		private List<Check> checkList = new List<Check>();

		/// <summary>
		/// チェックリスト
		/// </summary>
		public List<Check> CheckList {
			set => this.checkList = value;
			get => this.checkList;
		}

	}

}