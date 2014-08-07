using UnityEngine;
using System.Collections;

public class UIScrollSection : UIGrid
{
	public BetterList<UIScrollContent> listScrollContents = new BetterList<UIScrollContent>();

	public delegate void OnUpdateScrollSection();
	public OnUpdateScrollSection onUpdateScrollSection;
	
	public void AddScrollContent(UIScrollContent content)
	{
		listScrollContents.Add(content);

		if (onUpdateScrollSection != null)
			onUpdateScrollSection();
	}

	public Bounds GetBounds()
	{
		int x = 0;
		int y = 0;

		if (arrangement == Arrangement.Horizontal)
		{
			x = (maxPerLine == 0) ? listScrollContents.size : maxPerLine;
			y = (maxPerLine == 0) ? 1 : Mathf.FloorToInt(listScrollContents.size / 2f);
		}
		else
		{
			x = (maxPerLine == 0) ? 1 : Mathf.FloorToInt(listScrollContents.size / 2f);
			y = (maxPerLine == 0) ? listScrollContents.size : maxPerLine;
		}

		return new Bounds(Vector3.zero, new Vector3(cellWidth * x / 2f, cellHeight * y / 2f, 0f));
	}
}
