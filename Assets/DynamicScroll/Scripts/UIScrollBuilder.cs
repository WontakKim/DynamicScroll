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
				section.transform.localPosition = new Vector3(pos, 0f, 0f);
				pos += section.Bounds.size.x;
			}
			else
			{
				section.transform.localPosition = new Vector3(0f, -pos, 0f);
				pos += section.Bounds.size.y;
			}
		}
	}
}
