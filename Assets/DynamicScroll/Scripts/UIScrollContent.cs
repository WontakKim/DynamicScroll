using UnityEngine;
using System.Collections;

public class UIScrollContent
{
	public string id;
	public GameObject prefab;
	
	public delegate void OnInitContent(ref UIScrollContent content, GameObject go);
	public OnInitContent onInitContent;
}
