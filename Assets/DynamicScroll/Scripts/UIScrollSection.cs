using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIScrollSection : MonoBehaviour
{
	public enum Arrangement
	{
		Horizontal,
		Vertical,
	}

	public string sectionId;

	public UIScrollView scrollView;

	public Arrangement arrangement = Arrangement.Horizontal;

	public int maxLine = 0;

	public float contentWidth = 200f;
	public float contentHeight = 200f;

	public delegate void OnUpdateScrollSection();
	public OnUpdateScrollSection onUpdateScrollSection;
	
	private Bounds bounds;
	public Bounds Bounds
	{
		get {	return bounds;	}
	}

	private BetterList<UIScrollContent> listVirtualContents = new BetterList<UIScrollContent>();
	private Dictionary<string, GameObject> dicRealContents = new Dictionary<string, GameObject>();
	
	void Start()
	{
		if (string.IsNullOrEmpty(sectionId) == true)
			sectionId = gameObject.name;

		if (scrollView == null)
			scrollView = NGUITools.FindInParents<UIScrollView>(gameObject);

		if (scrollView != null)
		{
			if (scrollView.horizontalScrollBar != null)
				EventDelegate.Add(scrollView.horizontalScrollBar.onChange, Refresh);

			if (scrollView.verticalScrollBar != null)
				EventDelegate.Add(scrollView.verticalScrollBar.onChange, Refresh);
		}

		Refresh();
	}

	void Update()
	{
		if (scrollView.panel == null)
			return;

		SpringPanel sp = scrollView.gameObject.GetComponent<SpringPanel>();
		if ((sp != null && sp.enabled == true) || scrollView.currentMomentum.magnitude > 0f)
			Refresh();
	}

	public void Refresh()
	{
		if (scrollView.panel == null)
			return;

		Vector3[] corners = scrollView.panel.worldCorners;
		
		for (int i=0; i<4; i++)
		{
			Vector3 v = corners[i];
			v = transform.InverseTransformPoint(v);
			corners[i] = v;
		}

		Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);

		for (int i=0; i<listVirtualContents.size; i++)
		{
			float minX = corners[0].x - contentWidth;
			float minY = corners[0].y - contentHeight;
			float maxX = corners[2].x + contentWidth;
			float maxY = corners[2].y + contentHeight;

			Vector3 pos = GetContentPosition(i);

			float distanceX = pos.x - center.x + scrollView.panel.clipOffset.x - transform.localPosition.x + contentWidth / 2f;
			float distanceY = pos.y - center.y + scrollView.panel.clipOffset.y - transform.localPosition.y - contentHeight / 2f;

			UIScrollContent content = listVirtualContents[i];
			if (dicRealContents.ContainsKey(content.id) == true)
			{
				GameObject go = dicRealContents[content.id];
				Vector3 dst = GetContentPosition(i);
				go.transform.localPosition = dst;
			}

			if ((distanceX > minX && distanceX < maxX) && (distanceY > minY && distanceY < maxY))
				MakeContent(i);
		}
	}

	private void MakeContent(int index)
	{
		if (listVirtualContents.size <= index)
			return;

		UIScrollContent content = listVirtualContents[index];
		if (dicRealContents.ContainsKey(content.id) == true)
			return;

		GameObject go = NGUITools.AddChild(gameObject, content.prefab);
		go.name = content.id;

		go.transform.localPosition = GetContentPosition(index);

		dicRealContents.Add(content.id, go);

		if (content.onInitContent != null)
			content.onInitContent(ref content, go);
	}

	public void AddScrollContent(UIScrollContent content)
	{
		listVirtualContents.Add(content);

		if (onUpdateScrollSection != null)
			onUpdateScrollSection();
	}

	public void AddScrollContent(UIScrollContent content, int index)
	{
		listVirtualContents.Insert(index, content);

		if (onUpdateScrollSection != null)
			onUpdateScrollSection();
	}

	public void CalculateBounds()
	{
		int x = 0;
		int y = 0;
		
		if (arrangement == Arrangement.Horizontal)
		{
			x = (maxLine == 0) ? listVirtualContents.size : maxLine;
			if (listVirtualContents.size > 0)
				y = (maxLine == 0) ? 1 : Mathf.CeilToInt(listVirtualContents.size / maxLine) + 1;
		}
		else
		{
			if (listVirtualContents.size > 0)
				x = (maxLine == 0) ? 1 : Mathf.CeilToInt(listVirtualContents.size / maxLine) + 1;
			y = (maxLine == 0) ? listVirtualContents.size : maxLine;
		}
		
		bounds = new Bounds(Vector3.zero, new Vector3(contentWidth * x, contentHeight * y, 0f));

		UIWidget widget = gameObject.GetComponent<UIWidget>();
		if (widget == null)
		{
			widget = gameObject.AddComponent<UIWidget>();
			widget.pivot = UIWidget.Pivot.TopLeft;
		}

		widget.width = (int)bounds.size.x;
		widget.height = (int)bounds.size.y;
	}

	public Vector3 GetContentPosition(int index)
	{
		if (listVirtualContents.size <= index)
			return Vector3.zero;

		float x;
		float y;

		if (arrangement == Arrangement.Horizontal)
		{
			x = ((maxLine == 0) ? index : index % maxLine) * contentWidth + contentWidth / 2f;
			y = ((maxLine == 0) ? 0f : index / maxLine) * contentHeight + contentHeight / 2f;
		}
		else
		{
			x = ((maxLine == 0) ? 0f : index / maxLine) * contentWidth + contentWidth /2f;
			y = ((maxLine == 0) ? index : index % maxLine) * contentHeight + contentHeight / 2f;
		}

		return new Vector3(x, -y, 0f);
	}
}
