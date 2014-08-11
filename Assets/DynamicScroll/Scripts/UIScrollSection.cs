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
	private Dictionary<int, GameObject> dicRealContents = new Dictionary<int, GameObject>();
	
	void Start()
	{
		if (string.IsNullOrEmpty(sectionId) == true)
			sectionId = gameObject.name;

		scrollView = NGUITools.FindInParents<UIScrollView>(gameObject);
		if (scrollView != null)
		{
			if (scrollView.horizontalScrollBar != null)
				EventDelegate.Add (scrollView.horizontalScrollBar.onChange, Refresh);

			if (scrollView.verticalScrollBar != null)
				EventDelegate.Add (scrollView.verticalScrollBar.onChange, Refresh);
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

		if (arrangement == Arrangement.Horizontal)
		{
			for (int i=0; i<listVirtualContents.size; i++)
			{
				float min = corners[0].x - contentWidth;
				float max = corners[2].x + contentWidth;

				float localPos = i * contentWidth + contentWidth / 2f;
				float distance = localPos - center.x + scrollView.panel.clipOffset.x - transform.localPosition.x;

				if (distance > min && distance < max)
				{
					if (dicRealContents.ContainsKey(i) == true)
					{
						UIScrollContent content = listVirtualContents[i];
						if (content.onVisibleContent == null)
							continue;

						GameObject go = dicRealContents[i];
						content.onVisibleContent(content.id, go);
					}
					else
					{
						MakeContent(i);
					}

				}
			}
		}
		else
		{
			for (int i=0; i<listVirtualContents.size; i++)
			{
				if (dicRealContents.ContainsKey(i) == true)
					continue;
				
				float min = corners[0].y - contentHeight;
				float max = corners[2].y + contentHeight;
				
				float localPos = -(i * contentHeight + contentHeight / 2f);
				float distance = localPos - center.y + scrollView.panel.clipOffset.y - transform.localPosition.y;
				
				if (distance > min && distance < max)
				{
					if (dicRealContents.ContainsKey(i) == true)
					{
						UIScrollContent content = listVirtualContents[i];
						if (content.onVisibleContent == null)
							continue;
						
						GameObject go = dicRealContents[i];
						content.onVisibleContent(content.id, go);
					}
					else
					{
						MakeContent(i);
					}
					
				}
			}
		}
	}

	private void MakeContent(int index)
	{
		if (listVirtualContents.size <= index)
			return;

		if (dicRealContents.ContainsKey(index) == true)
			return;

		UIScrollContent content = listVirtualContents[index];
		GameObject go = NGUITools.AddChild(gameObject, content.prefab);
		go.name = content.id;

		if (arrangement == Arrangement.Horizontal)
		{
			float localPos = index * contentWidth + contentWidth / 2f;
			go.transform.localPosition = new Vector3(localPos, 0f, 0f);
		}
		else
		{
			float localPos = -(index * contentHeight + contentHeight / 2f);
			go.transform.localPosition = new Vector3(0f, localPos, 0f);
		}

		dicRealContents.Add(index, go);

		if (content.onInitContent != null)
			content.onInitContent(content.id, go);
	}

	public void AddScrollContent(UIScrollContent content)
	{
		listVirtualContents.Add(content);

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
			y = (maxLine == 0) ? 1 : Mathf.FloorToInt(listVirtualContents.size / 2f);
		}
		else
		{
			x = (maxLine == 0) ? 1 : Mathf.FloorToInt(listVirtualContents.size / 2f);
			y = (maxLine == 0) ? listVirtualContents.size : maxLine;
		}
		
		bounds = new Bounds(Vector3.zero, new Vector3(contentWidth * x, contentHeight * y, 0f));

		UIWidget widget = gameObject.GetComponent<UIWidget>();
		if (widget == null)
		{
			widget = gameObject.AddComponent<UIWidget>();
			
			if (arrangement == UIScrollSection.Arrangement.Horizontal)
				widget.pivot = UIWidget.Pivot.Left;
			else
				widget.pivot = UIWidget.Pivot.Top;
		}

		widget.width = (int)bounds.size.x;
		widget.height = (int)bounds.size.y;
	}
}
