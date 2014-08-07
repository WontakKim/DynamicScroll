using UnityEngine;
using System.Collections;

public class UIScrollBuilder : MonoBehaviour
{
	public UIScrollView scrollView;

	private BetterList<UIScrollSection> listScrollSections = new BetterList<UIScrollSection>();

	public void AddScrollSection(UIScrollSection section)
	{
		section.transform.parent = transform;
		section.transform.localPosition = Vector3.zero;
		section.transform.localScale = Vector3.one;

		section.onUpdateScrollSection = CalculateBounds;

		listScrollSections.Add(section);
	}

	public void CalculateBounds()
	{
		float pos = 0f;

		foreach (UIScrollSection section in listScrollSections)
		{
			Bounds b = section.GetBounds();

			if (scrollView.movement == UIScrollView.Movement.Horizontal)
			{
				section.transform.localPosition = new Vector3(pos, 0f, 0f);
				pos += b.extents.x * 2;
			}
			else
			{
				section.transform.localPosition = new Vector3(0f, pos, 0f);
				pos += b.extents.y * 2;
			}
		}
	}
}
