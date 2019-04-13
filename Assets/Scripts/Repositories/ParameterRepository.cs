﻿using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// パラメータリポジトリ
	/// </summary>
	public class ParameterRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static ParameterRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static ParameterRepository GetInstance() {
			if( Instance == null ) {
				Instance = new ParameterRepository();
			}
			return Instance;
		}

		#endregion

		/// <summary>
		/// パラメータ一覧
		/// </summary>
		public List<Parameter> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "parameters.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private ParameterRepository() {
			this.LogDebug( "Start" );

			Parameters parameters = this.Load<Parameters>( this.FilePath );
			this.Rows = parameters.rows;

			this.LogDebug( "End" );

		}

	}
}
