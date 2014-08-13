using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIScrollBuilder : MonoBehaviour
{
	public UIScrollView scrollView;

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
		float pos = 0f;
		
		foreach (UIScrollSection section in listScrollSections)
		{
			if (scrollView.movement == UIScrollView.Movement.Horizontal)
			{
				section.transform.localPosition = new Vector3(pos - section.contentWidth / 2f, section.contentHeight / 2f, 0f);
				pos += section.Bounds.size.x;
			}
			else
			{
				section.transform.localPosition = new Vector3(-(section.contentWidth / 2f), section.contentHeight / 2f - pos, 0f);
				pos += section.Bounds.size.y;
			}
		}
	}
}
