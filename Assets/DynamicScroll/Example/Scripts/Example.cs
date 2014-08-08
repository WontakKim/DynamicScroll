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

	public UIScrollView scrollView;

	void Start()
	{
		CreateChampions();
		CreateItems();

		scrollView.ResetPosition();
	}

	private void CreateChampions()
	{
		GameObject go = new GameObject();
		go.name = "Champions";

		UIScrollSection section = go.AddComponent<UIScrollSection>();
		section.cellWidth = 420f;
		section.cellHeight = 60f;
		section.arrangement = UIScrollSection.Arrangement.Vertical;
		section.onInitContent = InitalizeChampion;

		go.AddComponent<UIScrollBlinker>();

		foreach (string champion in champions)
		{
			UIScrollContent content = new UIScrollContent();
			content.id = champion;
			content.prefab = prefabChampion;

			section.AddScrollContent(content);
		}

		section.CalculateBounds();

		scrollBuilder.AddScrollSection(section);
	}

	private void CreateItems()
	{
		GameObject go = new GameObject();
		go.name = "Items";
		
		UIScrollSection section = go.AddComponent<UIScrollSection>();
		section.arrangement = UIScrollSection.Arrangement.Vertical;
		section.cellWidth = 420f;
		section.cellHeight = 60f;
		section.arrangement = UIScrollSection.Arrangement.Vertical;
		section.onInitContent = InitalizeItem;

		go.AddComponent<UIScrollBlinker>();

		foreach (string item in items)
		{
			UIScrollContent content = new UIScrollContent();
			content.id = item;
			content.prefab = prefabItem;
			
			section.AddScrollContent(content);
		}

		section.CalculateBounds();

		scrollBuilder.AddScrollSection(section);
	}

	public void InitalizeChampion(string id, GameObject go)
	{
		Champion champion = go.GetComponent<Champion>();
		if (champion == null)
			return;

		champion.labelName.text = id;
		champion.spriteIcon.spriteName = id;
	}

	public void InitalizeItem(string id, GameObject go)
	{
		Item item = go.GetComponent<Item>();
		if (item == null)
			return;

		item.labelName.text = id;
		item.spriteIcon.spriteName = id;
	}
}
