using UnityEngine;
using System.Collections;

public class SectionsScroll : MonoBehaviour
{
	public readonly string[] Champions = 
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

	public UIAtlas atlasChampion;
	public UIAtlas atlasItem;

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
			section.contentWidth = 420f;
			section.contentHeight = 60f;
			section.arrangement = UIScrollSection.Arrangement.Vertical;

			go.AddComponent<UIScrollBlinker>();
			
			UIScrollContent content = new UIScrollContent();
			content.id = "Champions";
			content.prefab = prefabTitle;
			content.onInitContent = InitalizeTitle;

			section.AddScrollContent(content);

			section.CalculateBounds();
			
			scrollBuilder.AddScrollSection(section);
		}

		// Champions.
		{
			GameObject go = new GameObject();
			go.name = "Champions";

			UIScrollSection section = go.AddComponent<UIScrollSection>();
			section.contentWidth = 420f;
			section.contentHeight = 60f;
			section.arrangement = UIScrollSection.Arrangement.Vertical;
			
			go.AddComponent<UIScrollBlinker>();
			
			foreach (string champion in Champions)
			{
				UIScrollContent content = new UIScrollContent();
				content.id = champion;
				content.prefab = prefabChampion;
				content.onInitContent = InitalizeChampion;

				section.AddScrollContent(content);
			}
			
			section.CalculateBounds();
			
			scrollBuilder.AddScrollSection(section);
		}
	}

	private void CreateItems()
	{
		// Title.
		{
			GameObject go = new GameObject();
			go.name = "Title";
			
			UIScrollSection section = go.AddComponent<UIScrollSection>();
			section.contentWidth = 420f;
			section.contentHeight = 60f;
			section.arrangement = UIScrollSection.Arrangement.Vertical;
			
			go.AddComponent<UIScrollBlinker>();
			
			UIScrollContent content = new UIScrollContent();
			content.id = "Items";
			content.prefab = prefabTitle;
			content.onInitContent = InitalizeTitle;
			
			section.AddScrollContent(content);
			
			section.CalculateBounds();
			
			scrollBuilder.AddScrollSection(section);
		}

		// Items.
		{
			GameObject go = new GameObject();
			go.name = "Items";
			
			UIScrollSection section = go.AddComponent<UIScrollSection>();
			section.arrangement = UIScrollSection.Arrangement.Vertical;
			section.contentWidth = 420f;
			section.contentHeight = 60f;
			section.arrangement = UIScrollSection.Arrangement.Vertical;
			
			go.AddComponent<UIScrollBlinker>();
			
			foreach (string item in items)
			{
				UIScrollContent content = new UIScrollContent();
				content.id = item;
				content.prefab = prefabItem;
				content.onInitContent = InitalizeItem;
				
				section.AddScrollContent(content);
			}
			
			section.CalculateBounds();
			
			scrollBuilder.AddScrollSection(section);
		}
	}

	public void InitalizeTitle(ref UIScrollContent content, GameObject go)
	{
		Title title = go.GetComponent<Title>();
		if (title == null)
			return;

		title.labelName.text = content.id;
	}

	public void InitalizeChampion(ref UIScrollContent content, GameObject go)
	{
		Champion champion = go.GetComponent<Champion>();
		if (champion == null)
			return;

		champion.labelName.text = content.id;

		champion.spriteIcon.atlas = atlasChampion;
		champion.spriteIcon.spriteName = content.id;
	}

	public void InitalizeItem(ref UIScrollContent content, GameObject go)
	{
		Item item = go.GetComponent<Item>();
		if (item == null)
			return;

		item.labelName.text = content.id;
		
		item.spriteIcon.atlas = atlasItem;
		item.spriteIcon.spriteName = content.id;
	}
}
