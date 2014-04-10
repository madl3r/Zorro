using UnityEngine;
using System.Collections;

public class spawnHeart : MonoBehaviour {

	public GameObject[] childList;
	public Material[] colors;

	private float spawnTime;
	private float liveTime;

	private bool spawned;

	// Use this for initialization
	void Start () {
		int colNum = Random.Range(0, colors.Length);
		float yHeight = Random.Range(1.0f, 2.0f);

		for(int i = 0; i < childList.Length; i++)
		{
			childList[i].SetActive(false);
			childList[i].renderer.sortingLayerID = 1; 
			childList[i].renderer.material.color = colors[colNum].color;
		}

		transform.Translate(new Vector3(0, yHeight, 0));
		liveTime = 3;
		spawned = false;


	}
	
	// Update is called once per frame
	void Update () {

		//Kill the heart after some time;
		// TODO, THIS IS ASSUMING THAT THE HEART LIGHTS UP AS SOON AS IT IS SPAWNED!
		if (Time.time - spawnTime > liveTime)
		{
			Destroy(gameObject);
		}
	}

	void lightUp()
	{
		StartCoroutine("spawnThis");
		spawnTime = Time.time;
		spawned = true;
	}
	
	IEnumerator spawnThis()
	{
		yield return new WaitForSeconds(0.005f);
		for(int i = 0; i < childList.Length; i++)
		{
			childList[i].SetActive(true);
			childList[i].renderer.sortingLayerName = "Background";
			if (i%2 != 0)
				yield return new WaitForSeconds(0.005f);
		}
	}
}
