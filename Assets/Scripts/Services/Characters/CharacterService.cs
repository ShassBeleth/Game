using System;
using System.Collections.Generic;
using System.Linq;
using Models.Charactor;
using Repositories;
using Repositories.Models;
using UniRx;
using Utils.Exxtensions;

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

		/// <summary>
		/// 装備可能箇所に装備できる装備Repository
		/// </summary>
		private EquipmentEquipableInEquipablePlaceRepository equipmentEquipableInEquipablePlaceRepository = EquipmentEquipableInEquipablePlaceRepository.GetInstance();

		/// <summary>
		/// 装備すると装備できなくなる装備可能箇所Repository
		/// </summary>
		private EquippedWhenUnequippingEquipablePlaceRepository equippedWhenUnequippingEquipablePlaceRepository = EquippedWhenUnequippingEquipablePlaceRepository.GetInstance();

		/// <summary>
		/// 装備すると増える装備可能箇所Repository
		/// </summary>
		private EquippedWhenIncreasingEquipablePlaceRepository equippedWhenIncreasingEquipablePlaceRepository = EquippedWhenIncreasingEquipablePlaceRepository.GetInstance();

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
		private IEnumerable<Body> GetHavingBodyRows( int saveId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Save Id is {saveId}." );
			IEnumerable<Body> bodies = this.bodyRepository.Rows
				.Where( row =>
					this.GetHavingBodyRowIds( saveId ).Contains( row.id )
				);
			Logger.Debug( "End" );
			return bodies;
		}

		/// <summary>
		/// 所持している素体ID一覧を取得する
		/// </summary>
		/// <param name="saveId">セーブID</param>
		/// <returns>所持している素体ID一覧</returns>
		private IEnumerable<int> GetHavingBodyRowIds( int saveId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Save Id is {saveId}." );
			IEnumerable<int> ids = this.havingBodyRepository.Rows
					.Where( row => row.saveId == saveId )
					.Select( row => row.bodyId );
			Logger.Debug( "End" );
			return ids;
		}
		
		private IEnumerable<int> GetIncreasingEquipablePlaceIds( int equipmentId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Equipment Id is {equipmentId}." );
			IEnumerable<int> equipablePlaceIds = this.equippedWhenIncreasingEquipablePlaceRepository.Rows
				.Where( row => row.equipmentId == equipmentId )
				.Select( row => row.equipablePlaceId );
			Logger.Debug( "End" );
			return equipablePlaceIds;
		}

		/// <summary>
		/// 装備ID一覧より追加される装備可能箇所一覧を取得する
		/// </summary>
		/// <param name="equipmentIds">装備ID一覧</param>
		/// <returns>装備可能箇所一覧</returns>
		private IEnumerable<EquipablePlace> GetIncreasingEquipablePlaces( IEnumerable<int> equipmentIds ) {
			Logger.Debug( "Start" );

			IEnumerable<int> equipablePlaceIds = equipmentIds
				.SelectMany( equipmentId => this.GetIncreasingEquipablePlaceIds( equipmentId ) )
				.Distinct();
			
			IEnumerable<EquipablePlace> equipablePlaces = this.equipablePlaceRepository.Rows
				.Where( row => equipablePlaceIds.Contains( row.id ) )
				.Select( row => new EquipablePlace() {
					id = row.id ,
					name = row.name
				} );
			
			Logger.Debug( "End" );
			return equipablePlaces;
		}
		
		/// <summary>
		/// 素体に紐づく装備可能箇所ID一覧を取得する
		/// </summary>
		/// <param name="bodyId">素体ID</param>
		/// <returns>装備可能箇所ID一覧</returns>
		private IEnumerable<int> GetEquipablePlaceRowIdsAssociateInBody( int bodyId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Body Id is {bodyId}." );
			IEnumerable<int> ids = this.bodyEquipablePlaceRepository.Rows
				.Where( row => row.bodyId == bodyId )
				.Select( row => row.equipablePlaceId );
			Logger.Debug( "End" );
			return ids;
		}

		/// <summary>
		/// 装備可能箇所一覧を取得する
		/// </summary>
		/// <param name="createdCharacter">作成したキャラクター</param>
		/// <returns>装備可能箇所一覧</returns>
		public List<EquipablePlaceModel> GetEquipablePlaces( BodyModel createdCharacter ) {
			Logger.Debug( "Start" );

			if( 
				createdCharacter == null 
				|| !createdCharacter.Id.HasValue 
				|| !createdCharacter.Id.Value.HasValue 
			) {
				Logger.Debug( "Created Character is Null." );
				return new List<EquipablePlaceModel>();
			}
			
			// 素体IDを取得
			int bodyId = createdCharacter.Id.Value.Value;

			// 素体に紐づく装備可能箇所ID一覧を取得
			IEnumerable<int> equipablePlaceIdsAssociateInBody = this.GetEquipablePlaceRowIdsAssociateInBody( bodyId );

			IEnumerable<EquipablePlace> equipablePlaceAssociateInBody = this.equipablePlaceRepository.Rows
				.Where( row => equipablePlaceIdsAssociateInBody.Contains( row.id ) )
				.Select( row => new EquipablePlace() {
					id = row.id ,
					name = row.name
				} );

			// 装備している装備ID一覧を取得
			IEnumerable<int> equipmentIds = createdCharacter.EquipablePlaces
				.Where( equipablePlace =>
					( equipablePlace.EquipmentModel?.Id?.Value ?? -1 ) != -1
				)
				.Select( equipablePlace => equipablePlace.EquipmentModel.Id.Value.Value );

			// 装備ID一覧より追加される装備可能箇所一覧を取得
			IEnumerable<EquipablePlace> increasingEquipablePlaces = this.GetIncreasingEquipablePlaces( equipmentIds );

			List<EquipablePlaceModel> models = equipablePlaceAssociateInBody
				.Concat( increasingEquipablePlaces )
				.Select( row => new EquipablePlaceModel() {
					Id = new ReactiveProperty<int>( row.id ) ,
					Name = row.name ,
					EquipmentModel = new EquipmentModel()
				} )
				.ToList();

			Logger.Debug( "End" );
			return models;
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
					EquipablePlaces = new List<EquipablePlaceModel>()
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
		private IEnumerable<int> GetHavingEquipmentRowIds( int saveId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Save Id is {saveId}." );
			IEnumerable<int> ids = this.havingEquipmentRepository.Rows
					.Where( row => row.saveId == saveId )
					.Select( row => row.equipmentId );
			Logger.Debug( "End" );
			return ids;
		}

		/// <summary>
		/// 所持している装備一覧を取得する
		/// </summary>
		/// <param name="saveId">セーブID</param>
		/// <returns>所持している装備一覧</returns>
		private IEnumerable<Equipment> GetHavingEquipmentRows( int saveId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Save Id is {saveId}." );
			IEnumerable<Equipment> equipments = this.equipmentRepository.Rows
				.Where( row => this.GetHavingEquipmentRowIds( saveId ).Contains( row.id ) );
			Logger.Debug( "End" );
			return equipments;
		}

		/// <summary>
		/// セーブIDより所持している装備一覧を取得する
		/// </summary>
		/// <param name="saveId">セーブID</param>
		/// <returns>所持している装備一覧</returns>
		private IEnumerable<Equipment> GetHavingEquipmentsuipmentRows( int saveId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Save Id is {saveId}." );

			IEnumerable<Equipment> model = this.equipmentRepository.Rows
				.Where( row => this.havingEquipmentRepository.Rows
					.Where( r => r.saveId == saveId )
					.Select( r => r.equipmentId )
					.Contains( row.id )
				);

			Logger.Debug( "End" );
			return model;
		}

		/// <summary>
		/// 装備可能かどうか
		/// </summary>
		/// <param name="equipmentId">装備ID</param>
		/// <param name="equipablePlaceId">装備可能箇所ID</param>
		/// <returns>装備可能かどうか</returns>
		private bool IsEquipable( int equipmentId , int equipablePlaceId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Equipment Id is {equipmentId}." );
			Logger.Debug( $"Equipable Place Id is {equipablePlaceId}." );
			bool isEquipable = this.equipmentEquipableInEquipablePlaceRepository.Rows
				.FirstOrDefault( 
					row => row.equipmentId == equipmentId && row.equipablePlaceId == equipablePlaceId 
				) != null;
			Logger.Debug( "End" );
			return isEquipable;
		}

		/// <summary>
		/// セーブIDより所持している装備の中から指定の装備可能箇所に装備できる装備一覧を返す
		/// </summary>
		/// <param name="saveId">セーブID</param>
		/// <param name="equipablePlaceId">装備可能箇所ID</param>
		/// <param name="createdCharacterModel">キャラクタモデル</param>
		/// <returns></returns>
		public List<EquipmentModel> GetHavingEquipments( 
			int saveId , 
			int equipablePlaceId , 
			BodyModel createdCharacterModel 
		) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Save Id is {saveId}." );
			Logger.Debug( $"Equipable Place Id is {equipablePlaceId}." );

			// 装備済み装備可能箇所一覧
			IEnumerable<int> equippedEquipablePlaces = createdCharacterModel.EquipablePlaces
				.Where( equipablePlace => ( equipablePlace?.EquipmentModel?.Id?.Value ?? -1 ) != -1 )
				.Select( equipablePlace => equipablePlace.Id.Value );

			List<EquipmentModel> models = this.GetHavingEquipmentRows( saveId )
				// 装備可能箇所に装備できる装備に絞る
				.Where( row => this.IsEquipable( row.id , equipablePlaceId ) )
				.Select( row => new EquipmentModel() {
					Id = new ReactiveProperty<int?>( row.id ) ,
					Name = row.name ,
					Ruby = row.ruby ,
					Flavor = row.flavor ,
					// 装備すると装備できなくなる箇所に装備があるか判定する
					CanEquip = this.equippedWhenUnequippingEquipablePlaceRepository.Rows
						.Any( equipablePlace => 
							!equippedEquipablePlaces.Contains( equipablePlace.equipablePlaceId ) 
						) ,
					DisplayOrder = row.displayOrder
				} )
				.ToList();

			Logger.Debug( "End" );
			return models;
		}

		/// <summary>
		/// 装備Model取得
		/// </summary>
		/// <param name="id">装備ID</param>
		/// <returns>装備Model</returns>
		public EquipmentModel GetEquipment( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Equipment Id is {id}." );
			EquipmentModel model = this.equipmentRepository.Rows
				.Where( row => row.id == id )
				.Select( row => new EquipmentModel() {
					Id = new ReactiveProperty<int?>( row.id ) ,
					Name = row.name ,
					Ruby = row.ruby ,
					CanEquip = true ,
					Flavor = row.flavor ,
					DisplayOrder = row.displayOrder
				} )
				.FirstOrDefault( row => row.Id.Value == id );

			Logger.Debug( "End" );
			return model;
		}

		/// <summary>
		/// 作成したキャラクターの装備可能箇所を更新する
		/// </summary>
		/// <param name="createdCharacter">作成したキャラクター</param>
		/// <param name="equipablePlaces">装備可能箇所一覧</param>
		/// <returns>更新された作成されたキャラクター</returns>
		public BodyModel UpdateEquipablePlacesOfCreatedCharacter( 
			BodyModel createdCharacter , 
			List<EquipablePlaceModel> equipablePlaces 
		) {
			Logger.Debug( "Start" );
			
			if( createdCharacter == null ) {
				Logger.Warning( "Created Character is Null." );
				return null;
			}

			// どの装備可能箇所にどの装備が装備されているかを一覧として取得
			Dictionary<int , EquipmentModel> equipments = new Dictionary<int , EquipmentModel>();
			createdCharacter.EquipablePlaces
				.ForEach( equipablePlace => equipments.Add(
					equipablePlace.Id?.Value ?? -1 ,
					( equipablePlace.EquipmentModel?.Id.HasValue ?? false )
						? equipablePlace.EquipmentModel
						: new EquipmentModel()
				) );

			// 装備可能箇所更新
			createdCharacter.EquipablePlaces = equipablePlaces;

			// 装備を付けなおす
			createdCharacter.EquipablePlaces
				.ForEach( equipablePlace =>
					equipablePlace.EquipmentModel = equipments.GetOrDefault( equipablePlace.Id.Value , new EquipmentModel() )
				);

			Logger.Debug( "End" );
			return createdCharacter;
		}

	}

}
