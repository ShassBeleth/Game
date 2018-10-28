using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIExtentions {

	[RequireComponent( typeof( ScrollRect ) )]
	[AddComponentMenu( "UI/Extensions/UIScrollToSelection" )]
	public class UIScrollToSelection : MonoBehaviour {

		//*** ATTRIBUTES ***//
		[Header( "[ Settings ]" )]
		[SerializeField]
		private ScrollType scrollDirection;
		[SerializeField]
		private float scrollSpeed = 10f;

		[Header( "[ Input ]" )]
		[SerializeField]
		private bool cancelScrollOnInput = false;
		[SerializeField]
		private List<KeyCode> cancelScrollKeycodes = new List<KeyCode>();

		//*** PROPERTIES ***//
		// REFERENCES
		protected RectTransform LayoutListGroup => this.TargetScrollRect?.content;

		// SETTINGS
		protected ScrollType ScrollDirection => this.scrollDirection;
		protected float ScrollSpeed => this.scrollSpeed;

		// INPUT
		protected bool CancelScrollOnInput => this.cancelScrollOnInput;

		protected List<KeyCode> CancelScrollKeycodes => this.cancelScrollKeycodes;

		// CACHED REFERENCES
		protected RectTransform ScrollWindow { get; set; }
		protected ScrollRect TargetScrollRect { get; set; }

		// SCROLLING
		protected EventSystem CurrentEventSystem => EventSystem.current;
		protected GameObject LastCheckedGameObject { get; set; }
		protected GameObject CurrentSelectedGameObject => EventSystem.current.currentSelectedGameObject;
		protected RectTransform CurrentTargetRectTransform { get; set; }
		protected bool IsManualScrollingAvailable { get; set; }

		//*** METHODS - PUBLIC ***//


		//*** METHODS - PROTECTED ***//
		protected virtual void Awake() {
			this.TargetScrollRect = GetComponent<ScrollRect>();
			this.ScrollWindow = this.TargetScrollRect.GetComponent<RectTransform>();
		}

		protected virtual void Start() {

		}

		protected virtual void Update() {
			UpdateReferences();
			CheckIfScrollingShouldBeLocked();
			ScrollRectToLevelSelection();
		}

		//*** METHODS - PRIVATE ***//
		private void UpdateReferences() {
			// update current selected rect transform
			if( this.CurrentSelectedGameObject != this.LastCheckedGameObject ) {
				this.CurrentTargetRectTransform = this.CurrentSelectedGameObject?.GetComponent<RectTransform>();

				// unlock automatic scrolling
				if( this.CurrentSelectedGameObject != null &&
					this.CurrentSelectedGameObject.transform.parent == this.LayoutListGroup.transform ) {
					this.IsManualScrollingAvailable = false;
				}
			}

			this.LastCheckedGameObject = this.CurrentSelectedGameObject;
		}

		private void CheckIfScrollingShouldBeLocked() {
			if( this.CancelScrollOnInput == false || this.IsManualScrollingAvailable == true ) {
				return;
			}

			for( int i = 0 ; i < this.CancelScrollKeycodes.Count ; i++ ) {
				if( Input.GetKeyDown( this.CancelScrollKeycodes[ i ] ) == true ) {
					this.IsManualScrollingAvailable = true;

					break;
				}
			}
		}

		private void ScrollRectToLevelSelection() {
			// check main references
			bool referencesAreIncorrect = ( this.TargetScrollRect == null || this.LayoutListGroup == null || this.ScrollWindow == null );

			if( referencesAreIncorrect == true || this.IsManualScrollingAvailable == true ) {
				return;
			}

			RectTransform selection = this.CurrentTargetRectTransform;

			// check if scrolling is possible
			if( selection == null || selection.transform.parent != this.LayoutListGroup.transform ) {
				return;
			}

			// depending on selected scroll direction move the scroll rect to selection
			switch( this.ScrollDirection ) {
				case ScrollType.VERTICAL:
					UpdateVerticalScrollPosition( selection );
					break;
				case ScrollType.HORIZONTAL:
					UpdateHorizontalScrollPosition( selection );
					break;
				case ScrollType.BOTH:
					UpdateVerticalScrollPosition( selection );
					UpdateHorizontalScrollPosition( selection );
					break;
			}
		}

		private void UpdateVerticalScrollPosition( RectTransform selection ) {
			// move the current scroll rect to correct position
			float selectionPosition = -selection.anchoredPosition.y - ( selection.rect.height * ( 1 - selection.pivot.y ) );

			float elementHeight = selection.rect.height;
			float maskHeight = this.ScrollWindow.rect.height;
			float listAnchorPosition = this.LayoutListGroup.anchoredPosition.y;

			// get the element offset value depending on the cursor move direction
			float offlimitsValue = GetScrollOffset( selectionPosition , listAnchorPosition , elementHeight , maskHeight );

			// move the target scroll rect
			this.TargetScrollRect.verticalNormalizedPosition +=
				( offlimitsValue / this.LayoutListGroup.rect.height ) * Time.unscaledDeltaTime * this.scrollSpeed;
		}

		private void UpdateHorizontalScrollPosition( RectTransform selection ) {
			// move the current scroll rect to correct position
			float selectionPosition = -selection.anchoredPosition.x - ( selection.rect.width * ( 1 - selection.pivot.x ) );

			float elementWidth = selection.rect.width;
			float maskWidth = this.ScrollWindow.rect.width;
			float listAnchorPosition = -this.LayoutListGroup.anchoredPosition.x;

			// get the element offset value depending on the cursor move direction
			float offlimitsValue = -GetScrollOffset( selectionPosition , listAnchorPosition , elementWidth , maskWidth );

			// move the target scroll rect
			this.TargetScrollRect.horizontalNormalizedPosition +=
				( offlimitsValue / this.LayoutListGroup.rect.width ) * Time.unscaledDeltaTime * this.scrollSpeed;
		}

		private float GetScrollOffset( float position , float listAnchorPosition , float targetLength , float maskLength ) {
			if( position < listAnchorPosition + ( targetLength / 2 ) ) {
				return ( listAnchorPosition + maskLength ) - ( position - targetLength );
			}
			else if( position + targetLength > listAnchorPosition + maskLength ) {
				return ( listAnchorPosition + maskLength ) - ( position + targetLength );
			}

			return 0;
		}

		//*** ENUMS ***//
		public enum ScrollType {
			VERTICAL,
			HORIZONTAL,
			BOTH
		}
	}
}
