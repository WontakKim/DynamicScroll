using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour
{
	public readonly string[] AssassinChampions = 
	{
		"Ahri", "Akali", "Evelynn", "Fiora", "Fizz", "Irelia", "Jax", "Kassadin", "Katarina", "Khazix", "Leblanc", 
		"LeeSin", "Malzahar", "MasterYi", "Nidalee", "Nocturne", "Pantheon", "Poppy", "Rengar", "Riven", "Shaco", 
		"Talon", "Teemo", "Tristana", "Tryndamere", "Twitch", "Vayne", "Vi", "Xerath", "XinZhao", "Yasuo", "Zed"
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

	public GameObject prefabTitle;
	public GameObject prefabChampion;
	public GameObject prefabItem;

	public UIAtlas atlasAssasin;

	public UIScrollView scrollView;

	void Start()
	{
		CreateChampions();
		CreateItems();

		scrollView.ResetPosition();
	}

	private void CreateChampions()
	{
		// Title.
		{
			GameObject go = new GameObject();
			go.name = "Title";

			UIScrollSection section = go.AddComponent<UIScrollSection>();
			section.cellWidth = 420f;
			section.cellHeight = 60f;
			section.arrangement = UIScrollSection.Arrangement.Vertical;
			section.onInitContent = InitalizeTitle;
			
			go.AddComponent<UIScrollBlinker>();
			
			UIScrollContent content = new UIScrollContent();
			content.id = "Assassin Champions";
			content.prefab = prefabTitle;

			section.AddScrollContent(content);

			section.CalculateBounds();
			
			scrollBuilder.AddScrollSection(section);
		}


		// Assassin champions.
		{
			GameObject go = new GameObject();
			go.name = "Champions";

			UIScrollSection section = go.AddComponent<UIScrollSection>();
			section.cellWidth = 420f;
			section.cellHeight = 60f;
			section.arrangement = UIScrollSection.Arrangement.Vertical;
			section.onInitContent = InitalizeChampion;
			
			go.AddComponent<UIScrollBlinker>();
			
			foreach (string champion in AssassinChampions)
			{
				UIScrollContent content = new UIScrollContent();
				content.id = "Assassin" + "_" + champion;
				content.prefab = prefabChampion;
				
				section.AddScrollContent(content);
			}
			
			section.CalculateBounds();
			
			scrollBuilder.AddScrollSection(section);
		}
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

	public void InitalizeTitle(string id, GameObject go)
	{
		Title title = go.GetComponent<Title>();
		if (title == null)
			return;

		title.labelName.text = id;
	}

	public void InitalizeChampion(string id, GameObject go)
	{
		Champion champion = go.GetComponent<Champion>();
		if (champion == null)
			return;

		string[] parse = id.Split('_');

		string type = parse[0];
		string name = parse[1];

		if (type.Equals("Assassin") == true)
			champion.spriteIcon.atlas = atlasAssasin;

		champion.labelName.text = name;
		champion.spriteIcon.spriteName = name;
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
