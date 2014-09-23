using UnityEngine;
using System.Collections;

public class SimpleScroll : MonoBehaviour
{
	public readonly string[] Champions = 
	{
		"Ahri", "Akali", "Evelynn", "Fiora", "Fizz", "Irelia", "Jax", "Kassadin", "Katarina", "Khazix", "Leblanc", 
		"LeeSin", "Malzahar", "MasterYi", "Nidalee", "Nocturne", "Pantheon", "Poppy", "Rengar", "Riven", "Shaco", 
		"Talon", "Teemo", "Tristana", "Tryndamere", "Twitch", "Vayne", "Vi", "Xerath", "XinZhao", "Yasuo", "Zed"
	};

	public UIScrollBuilder scrollBuilder;
	public UIScrollSection scrollSection;
	
	public GameObject prefabChampion;

	public UIAtlas atlas;

	public UIScrollView scrollView;

	void Start()
	{
		CreateChampions();

		scrollView.ResetPosition();
	}

	private void CreateChampions()
	{
		// Champions.
		{
			foreach (string champion in Champions)
			{
				UIScrollContent content = new UIScrollContent();
				content.id = champion;
				content.prefab = prefabChampion;
				content.onInitContent = InitalizeChampion;

				scrollSection.AddScrollContent(content);
			}
			
			scrollSection.CalculateBounds();
		}
	}

	public void InitalizeChampion(ref UIScrollContent content, GameObject go)
	{
		Champion champion = go.GetComponent<Champion>();
		if (champion == null)
			return;

		champion.spriteIcon.atlas = atlas;

		champion.labelName.text = content.id;
		champion.spriteIcon.spriteName = content.id;
	}
}
