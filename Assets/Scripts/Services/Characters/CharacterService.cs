using System.Collections.Generic;
using System.Linq;
using Models.Charactor;
using Repositories;
using Repositories.Models;
using UniRx;

namespace Services.Characters {

	/// <summary>
	/// キャラクターService
	/// </summary>
	public class CharacterService {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static CharacterService Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static CharacterService GetInstance() {
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new CharacterService();
			}
			Logger.Debug( "End" );
			return Instance;
		}

		#endregion

		#region Repository
		
		/// <summary>
		/// 所持している素体Repository
		/// </summary>
		private HavingBodyRepository havingBodyRepository = HavingBodyRepository.GetInstance();

		/// <summary>
		/// 素体：装備可能箇所Repository
		/// </summary>
		private BodyEquipablePlaceRepository bodyEquipablePlaceRepository = BodyEquipablePlaceRepository.GetInstance();

		/// <summary>
		/// 装備可能箇所Repository
		/// </summary>
		private EquipablePlaceRepository equipablePlaceRepository = EquipablePlaceRepository.GetInstance();

		/// <summary>
		/// 素体Repository
		/// </summary>
		private BodyRepository bodyRepository = BodyRepository.GetInstance();

		/// <summary>
		/// 装備Repository
		/// </summary>
		private EquipmentRepository equipmentRepository = EquipmentRepository.GetInstance();

		/// <summary>
		/// 所持している装備Repository
		/// </summary>
		private HavingEquipmentRepository havingEquipmentRepository = HavingEquipmentRepository.GetInstance();

		#endregion

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private CharacterService() {
			Logger.Debug( "Start" );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 所持している素体一覧を取得する
		/// </summary>
		/// <param name="saveId">セーブID</param>
		/// <returns>所持している素体一覧</returns>
		private List<Body> GetHavingBodyRows( int saveId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Save Id is {saveId}." );
			List<Body> bodies = this.bodyRepository.Rows
				.Where( row => 
					this.GetHavingBodyRowIds( saveId ).Contains(row.id)
				)
				.ToList();
			Logger.Debug( "End" );
			return bodies;
		}

		/// <summary>
		/// 所持している素体ID一覧を取得する
		/// </summary>
		/// <param name="saveId">セーブID</param>
		/// <returns>所持している素体ID一覧</returns>
		private List<int> GetHavingBodyRowIds( int saveId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Save Id is {saveId}." );
			List<int> ids = this.havingBodyRepository.Rows
					.Where( row => row.saveId == saveId )
					.Select( row => row.bodyId )
				.ToList();
			Logger.Debug( "End" );
			return ids;
		}

		/// <summary>
		/// 素体に紐づく装備可能箇所一覧を取得する
		/// </summary>
		/// <param name="bodyId">素体ID</param>
		/// <returns>装備可能箇所一覧</returns>
		private List<EquipablePlace> GetEquipablePlaceRowsAssociateInBody( int bodyId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Body Id is {bodyId}." );
			List<EquipablePlace> equipablePlaces = this.equipablePlaceRepository.Rows
				.Where( row => 
					this.GetEquipablePlaceRowIdsAssociateInBody( bodyId ).Contains(row.id)
				)
				.ToList();
			Logger.Debug( "End" );
			return equipablePlaces;
		}

		/// <summary>
		/// 素体に紐づく装備可能箇所ID一覧を取得する
		/// </summary>
		/// <param name="bodyId">素体ID</param>
		/// <returns>装備可能箇所ID一覧</returns>
		private List<int> GetEquipablePlaceRowIdsAssociateInBody( int bodyId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Body Id is {bodyId}." );
			List<int> ids = this.bodyEquipablePlaceRepository.Rows
				.Where( row => row.bodyId == bodyId )
				.Select( row => row.equipablePlaceId )
				.ToList();
			Logger.Debug( "End" );
			return ids;
		}
		
		/// <summary>
		/// セーブIDより所持している素体一覧を取得する
		/// </summary>
		/// <param name="saveId">セーブID</param>
		/// <returns>所持している素体一覧</returns>
		public List<BodyModel> GetHavingBodies( int saveId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Save Id is {saveId}." );

			List<BodyModel> models = this.GetHavingBodyRows( saveId )
				// BodyModel作成
				.Select( body => new BodyModel() {
					Id = new ReactiveProperty<int?>(body.id) ,
					Name = body.name ,
					Ruby = body.ruby ,
					Flavor = body.flavor ,
					EquipablePlaces = this.GetEquipablePlaceRowsAssociateInBody( body.id )
						// EquipablePlaceModel作成
						.Select( equipablePlace => new EquipablePlaceModel() {
							Id = new ReactiveProperty<int>( equipablePlace.id ) ,
							Name = equipablePlace.name ,
							EquipmentModel = new EquipmentModel()
						} )
						.ToList()
				} )
				.ToList();
			Logger.Debug( "End" );
			return models;
		}
		
		/// <summary>
		/// 所持している装備ID一覧を取得する
		/// </summary>
		/// <param name="saveId">セーブID</param>
		/// <returns>所持している装備ID一覧</returns>
		private List<int> GetHavingEquipmentRowIds( int saveId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Save Id is {saveId}." );
			List<int> ids = this.havingEquipmentRepository.Rows
					.Where( row => row.saveId == saveId )
					.Select( row => row.equipmentId )
				.ToList();
			Logger.Debug( "End" );
			return ids;
		}

		/// <summary>
		/// 所持している装備一覧を取得する
		/// </summary>
		/// <param name="saveId">セーブID</param>
		/// <returns>所持している装備一覧</returns>
		private List<Equipment> GetHavingEquipmentRows( int saveId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Save Id is {saveId}." );
			List<Equipment> equipments = this.equipmentRepository.Rows
				.Where( row => this.GetHavingEquipmentRowIds( saveId ).Contains( row.id ) )
				.ToList();
			Logger.Debug( "End" );
			return equipments;
		}

		/// <summary>
		/// セーブIDより所持している装備一覧を取得する
		/// </summary>
		/// <param name="saveId">セーブID</param>
		/// <returns>所持している装備一覧</returns>
		public List<EquipmentModel> GetHavingEquipments( int saveId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Save Id is {saveId}." );

			List<EquipmentModel> models = this.GetHavingEquipmentRows( saveId )
				// EquipmentModel作成
				.Select( equipment => new EquipmentModel() {
					Id = new ReactiveProperty<int?>( equipment.id ) ,
					Name = equipment.name ,
					Ruby = equipment.ruby ,
					Flavor = equipment.flavor ,
					DisplayOrder = equipment.displayOrder
				} )
				.OrderByDescending( equipment => equipment.DisplayOrder )
				.ToList();
			Logger.Debug( "End" );
			return models;
		}

	}

}
