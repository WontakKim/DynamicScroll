using UnityEngine;
using System.Collections;

public class UIScrollBlinker : MonoBehaviour
{
	public UIPanel scrollPanel;

	private bool firstTime;

	void Start()
	{
		if (scrollPanel != null)
			scrollPanel.onClipMove = this.OnMove;
		
		firstTime = true;
		Blink();
	}
	
	void OnEnable()
	{
		if (firstTime == false)
		{
			if (scrollPanel == null)
				return;

			scrollPanel.onClipMove = this.OnMove;
			Blink();
		}
	}

	protected void OnMove(UIPanel panel)
	{
		Blink();
	}

	public void Blink()
	{
		if (scrollPanel == null)
			return;

		Vector3[] corners = scrollPanel.worldCorners;
		
		for (int i=0; i<4; i++)
		{
			Vector3 v = corners[i];
			v = transform.InverseTransformPoint(v);
			corners[i] = v;
		}
		
		Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);

		for (int i=0; i<transform.childCount; i++)
		{
			Transform t = transform.GetChild(i);
			Bounds b = NGUIMath.CalculateRelativeWidgetBounds(t, true);

			float minX = corners[0].x - b.extents.x;
			float minY = corners[0].y - b.extents.y;
			float maxX = corners[2].x + b.extents.x;
			float maxY = corners[2].y + b.extents.y;

			float distanceX = t.localPosition.x - center.x + scrollPanel.clipOffset.x - transform.localPosition.x;
			float distanceY = t.localPosition.y - center.y + scrollPanel.clipOffset.y - transform.localPosition.y;

			bool active = (distanceX > minX && distanceX < maxX) && (distanceY > minY && distanceY < maxY);
			NGUITools.SetActive(t.gameObject, active, false);
		}
	}
}
