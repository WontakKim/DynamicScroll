using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIScrollBuilder : MonoBehaviour
{
	public enum Pivot
	{
		Left,
		Center,
		Right,
	}

	public UIPanel scrollPanel;
	public Pivot pivot = Pivot.Center;

	public float padding = 0f;
	public int paddingUnitCount = 0;
	
	public List<UIScrollSection> listScrollSections = new List<UIScrollSection>();

	void Start()
	{
		Reposition();
	}
	
	public void AddScrollSection(UIScrollSection section)
	{
		section.transform.parent = transform;
		section.transform.localPosition = Vector3.zero;
		section.transform.localScale = Vector3.one;

		listScrollSections.Add(section);

		Reposition();
	}

	public void Reposition()
	{
		UIScrollView scrollView = scrollPanel.GetComponent<UIScrollView>();
		Vector4 clipRegion = scrollPanel.baseClipRegion;
		Vector2 pivotOffset = NGUIMath.GetPivotOffset(scrollView.contentPivot);
		Vector2 clipOffset = new Vector3(clipRegion.x + clipRegion.z * (pivotOffset.x - 0.5f),
		                                 clipRegion.y + clipRegion.w * (pivotOffset.y - 0.5f));

		float cachedPos = 0f;
		foreach (UIScrollSection section in listScrollSections)
		{
			Vector2 offset = new Vector3(clipOffset.x - section.Bounds.size.x * pivotOffset.x, 
			                             clipOffset.y + section.Bounds.size.y * (1f - pivotOffset.y));

 			if (scrollView.movement == UIScrollView.Movement.Horizontal)
			{
				section.transform.localPosition = new Vector3(cachedPos + offset.x, offset.y, 0f);
				cachedPos += section.Bounds.size.x;
			}
			else
			{
				section.transform.localPosition = new Vector3(offset.x, offset.y - cachedPos, 0f);
				cachedPos += section.Bounds.size.y;
			}
		}
	}

	public Bounds Bounds
	{
		get
		{
			UIScrollView scrollView = scrollPanel.GetComponent<UIScrollView>();

			float maxX = 0f;
			float maxY = 0f;

			for (int i=0; i<listScrollSections.Count; i++)
			{
				UIScrollSection section = listScrollSections[i];

				if (scrollView.movement == UIScrollView.Movement.Horizontal)
				{
					maxX += (paddingUnitCount > 0 && i % paddingUnitCount == 0) ? padding : 0f;
					maxX += section.Bounds.extents.x;
					if (section.Bounds.extents.y > maxY)
						maxY = section.Bounds.extents.y;
				}
				else
				{
					if (section.Bounds.extents.x > maxX)
						maxX = section.Bounds.extents.x;
					maxY += (paddingUnitCount > 0 && i % paddingUnitCount == 0) ? padding : 0f;
					maxY += section.Bounds.extents.y;
				}
			}

			return new Bounds(Vector3.zero, new Vector3(maxX, maxY));
		}
	}
}
