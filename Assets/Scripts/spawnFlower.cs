using UnityEngine;
using System.Collections;

public class spawnFlower : MonoBehaviour {


	public GameObject[] childList;
	public Material[] colors;
	public GameObject flCenter;
	public GameObject[] petals;

	private int petalNum;
	private int flNum;

	// Use this for initialization
	void Start () {

		flCenter = transform.FindChild("f1").gameObject;

		petalNum = Random.Range(0, colors.Length);
		flNum = Random.Range(0, colors.Length);

		Color theCol = colors[flNum].color;
		
		flCenter.renderer.material.color = theCol;
		
		for (int i = 0; i < petals.Length; i++)
		{
			petals[i].renderer.material.color = colors[petalNum].color;
		}

		for(int i = 0; i < childList.Length; i++)
		{
//			childList[i].SetActive(false);
			childList[i].renderer.enabled = false;
			childList[i].renderer.sortingLayerID = 1; 
		}

	

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void lightUp()
	{
		StartCoroutine("spawnThis");
	}

	IEnumerator spawnThis()
	{
		for(int i = 0; i < childList.Length; i++)
		{
			//childList[i].SetActive(true);
			childList[i].renderer.enabled = true;
			childList[i].renderer.sortingLayerID = 1; 
			yield return new WaitForSeconds(0.005f);
		}
	}

}
