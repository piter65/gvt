/*USAGE:
Simply place the script on the ScrollRect that contains the selectable children we'll be scroling to
and drag'n'drop the RectTransform of the options "container" that we'll be scrolling.*/

using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Extensions/UI ScrollTo Selection XY")]
	[RequireComponent(typeof(ScrollRect))]
	public class UIScrollToSelectionXY : MonoBehaviour
	{
		public float scrollSpeed = 10f;

		public Vector2 scroll_pos;

		[SerializeField]
		private RectTransform layout_list_group = null;

		private RectTransform _scroll_viewport;
		private ScrollRect _scroll_Rect;

		private RectTransform _selection = null;

		private bool _scroll_to_selection = true;
		private Vector2 _target_scroll_pos = new Vector2(0.0f, 1.0f);

		private bool _mouse_down = false;
		private Vector3 _mouse_pos_prev = Vector3.zero;

		private void Start()
		{
			_scroll_Rect = GetComponent<ScrollRect>();
			_scroll_viewport = _scroll_Rect.viewport.GetComponent<RectTransform>();
		}

		private void Update()
		{
			if (_mouse_down)
			{
				if (_mouse_pos_prev != Input.mousePosition)
					_scroll_to_selection = false;

				if (Input.GetMouseButtonUp(0))
					_mouse_down = false;
				else
					return;
			}
			else
			if (Input.GetMouseButtonDown(0))
			{
				_mouse_down = true;
				_scroll_to_selection = true;
				_mouse_pos_prev = Input.mousePosition;
				return;
			}

			// FIX: if you dont do that here events can have null value
			var events = EventSystem.current;

			if (events.currentSelectedGameObject == null)
				return;

			// get calculation references
			RectTransform selection_new = events.currentSelectedGameObject.GetComponent<RectTransform>();

			if (   selection_new == null
				|| !IsDescendantOf(selection_new.transform, _scroll_Rect.transform))
			{
				return;
			}

			if (selection_new != _selection)
			{
				_selection = selection_new;

				if (   !_mouse_down
					&& _scroll_to_selection)
				{
					Rect rect_selection = GetScreenCoordinates(_selection);
					Rect rect_viewport  = GetScreenCoordinates(_scroll_viewport);
					Rect rect_layout    = GetScreenCoordinates(layout_list_group);

					// If the containers have zero size, we're not ready to scroll to the selection.
					if (   rect_viewport.size.x <= 0.0f
						|| rect_viewport.size.y <= 0.0f
						|| rect_layout.size.x   <= 0.0f
						|| rect_layout.size.y   <= 0.0f)
					{
						_selection = null;
						return;
					}

					// Debug.Log("rect_selection: "+rect_selection);
					// Debug.Log("rect_viewport: "+rect_viewport);
					// Debug.Log("rect_layout: "+rect_layout);

					Vector2 pos_local_selection = rect_selection.center - rect_layout.min;
					Vector2 pos_local_viewport  = rect_viewport.center - rect_layout.min;

					Vector2 offset_selection = rect_selection.center - (rect_layout.min + rect_selection.size * 0.5f);

					float nav_width_layout  = rect_layout.width  - rect_selection.width;
					float nav_height_layout = rect_layout.height - rect_selection.height;

					Vector2 offset_normalized = Vector2.zero;
					if (nav_width_layout > 0.0f)
					{
						offset_normalized.x = offset_selection.x / nav_width_layout;
						float normal_bleed = rect_viewport.width * 0.5f / nav_width_layout;
						offset_normalized.x = Mathf.Lerp(-normal_bleed, 1.0f + normal_bleed, offset_normalized.x);
					}
					if (nav_height_layout > 0.0f)
					{
						offset_normalized.y = offset_selection.y / nav_height_layout;
						float normal_bleed = rect_viewport.height * 0.5f / nav_height_layout;
						offset_normalized.y = Mathf.Lerp(-normal_bleed, 1.0f + normal_bleed, offset_normalized.y);
					}

					_target_scroll_pos = offset_normalized;

					// Debug.Log("_target_scroll_pos: "+_target_scroll_pos);

					// _scroll_Rect.horizontalNormalizedPosition = offset_normalized.x;
					// _scroll_Rect.verticalNormalizedPosition   = offset_normalized.y;
				}
			}

			if (   !_mouse_down
				&& _scroll_to_selection)
			{
				Vector2 scroll_pos = new Vector2
				(
					_scroll_Rect.horizontalNormalizedPosition,
					_scroll_Rect.verticalNormalizedPosition
				);

				scroll_pos = Vector2.MoveTowards
				(
					scroll_pos,
					_target_scroll_pos,
					scrollSpeed * Time.deltaTime
				);

				// Debug.Log("scroll_pos: "+scroll_pos);

				_scroll_Rect.horizontalNormalizedPosition = scroll_pos.x;
				_scroll_Rect.verticalNormalizedPosition   = scroll_pos.y;
			}
		}

		private bool IsDescendantOf(Transform descendant, Transform ancestor)
		{
			Transform parent = descendant.parent;
			while (parent != null)
			{
				if (parent == ancestor)
					return true;

				descendant = parent;
				parent = descendant.parent;
			}

			return false;
		}

		private Rect GetScreenCoordinates(RectTransform rect_transform)
		{
			var world_corners = new Vector3[4];
			rect_transform.GetWorldCorners(world_corners);
			var result = new Rect
			(
				world_corners[0].x,
				world_corners[0].y,
				world_corners[2].x - world_corners[0].x,
				world_corners[2].y - world_corners[0].y
			);
			return result;
		}
	}
}