using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour
{
	public readonly string[] champions =
	{
		"Aatrox", "Ahri", "Akali", "Alistar", "Amumu", 
		"Anivia", "Annie", "Ashe", "Blitzcrank", "Brand", 
		"Braum", "Caitlyn", "Cassiopeia", "Chogath", "Corki", 
		"Darius", "Diana", "Draven", "DrMundo", "Elise", "Evelynn",
	};

	public readonly string[] items = 
	{
		"AbyssalScepter",
		"AegisOfTheLegion",
		"AetherWisp",
		"AmplifyingTome",
		"AncientCoin",
		"ArchangelsStaff",
		"ArdentCenser",
	};

	public UIScrollBuilder scrollBuilder;

	public GameObject prefabChampion;
	public GameObject prefabItem;

	void Start()
	{
		CreateChampions();
		CreateItems();
	}

	private void CreateChampions()
	{
		GameObject go = new GameObject();
		go.name = "Champions";

		UIScrollSection section = go.AddComponent<UIScrollSection>();
		scrollBuilder.AddScrollSection(section);

		foreach (string champion in champions)
		{
			UIScrollContent content = new UIScrollContent();
			content.id = champion;
			content.prefab = prefabChampion;

			section.AddScrollContent(content);
		}

		Debug.LogWarning("Bounds : " + section.GetBounds());
	}

	private void CreateItems()
	{
		GameObject go = new GameObject();
		go.name = "Items";
		
		UIScrollSection section = go.AddComponent<UIScrollSection>();
		scrollBuilder.AddScrollSection(section);
		
		foreach (string item in items)
		{
			UIScrollContent content = new UIScrollContent();
			content.id = item;
			content.prefab = prefabItem;
			
			section.AddScrollContent(content);
		}
		
		Debug.LogWarning("Bounds : " + section.GetBounds());
	}
}
