using System.Linq;
using UnityEngine;

public class MapHelper : MonoBehaviour
{
	[SerializeField] private SpriteRenderer mapTilePrefabs;
	[SerializeField] private Color primaryColor;
	[SerializeField] private Color secondColor;
	[SerializeField] private Vector2 mapSize;

	[ContextMenu(nameof(Create))]
	private void Create()
	{
		var list = transform.GetComponentsInChildren<Transform>().ToList();
		list.RemoveAt(0);
		foreach (var child in list)
			DestroyImmediate(child.gameObject);
		
		bool isPrimal = true;
		for (int i = 1; i < mapSize.x; i++)
		{
			for (int j = 1; j < mapSize.y; j++)
			{
				var sprite = Instantiate(mapTilePrefabs, transform);
				sprite.transform.position = new Vector3(i, j, 0);
				sprite.color = isPrimal ? primaryColor : secondColor;
				isPrimal = !isPrimal;
			}
		}
	}
}