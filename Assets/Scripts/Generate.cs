using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generate : MonoBehaviour {
	public static Generate Instance;
    public float Size;
	public Transform Root;
	public Transform RootConnect;

    private float moveTime = 0.8f;
	private float scaleSize = 0.6f;

	void Start(){
		GenerateGameBoard();
		Instance = this;
	}
	
	void GenerateGameBoard(){
		for(int i=0;i<9;i++)
		{
			for(int j=0;j<9;j++)
			{
				GameObject go = (GameObject)Instantiate(Resources.Load(ResourceStr.CookiePrefabs, typeof(GameObject)),
				                                        Vector3.zero,Quaternion.identity);
				go.transform.parent = Root;
				go.transform.localScale = Vector3.one * Size;
				go.transform.localPosition= new Vector3(j*scaleSize*Size,-i*scaleSize*Size);
				if(go.GetComponent<CookieHandle>() != null)
				{
					CookieHandle b = go.GetComponent<CookieHandle>();
					b.xIndex = j;
					b.yIndex = i;
				}

                GameObject grid = (GameObject)Instantiate(Resources.Load(ResourceStr.GridPrefabs, typeof(GameObject)), 
				                                          Vector3.zero, Quaternion.identity);
                grid.transform.parent = Root;
                grid.transform.localScale = Vector3.one / Size;
				grid.transform.localPosition = new Vector3(j * scaleSize * Size, -i * scaleSize * Size);
                if (grid.GetComponent<Grid>() != null)
                {
                    Grid b = go.GetComponent<Grid>();
                    b.xIndex = j;
                    b.yIndex = i;
                }
			}
		}
	}
	
    public void MovePieces(int x, int y)
    {
        if (y > 0)
        {
            GameObject[] list = GameObject.FindGameObjectsWithTag(ResourceStr.CookieTag);
            for (int i = 0; i < list.Length; i++)
            {
				if (list[i].GetComponent<CookieHandle>().xIndex == x)
                {
					if (list[i].GetComponent<CookieHandle>().yIndex < y)
                    {
						list[i].GetComponent<CookieHandle>().yIndex += 1;
                        Vector3 pos = new Vector3(list[i].transform.localPosition.x,
						                          -list[i].GetComponent<CookieHandle>().yIndex * scaleSize * Size);
                        iTween.MoveTo(list[i].gameObject, iTween.Hash("position", pos, "islocal", true, "time", moveTime));
                    }
                }
            }
        }
        GameObject newObj = (GameObject)Instantiate(Resources.Load(ResourceStr.CookiePrefabs, typeof(GameObject)), 
		                                            Vector3.zero, Quaternion.identity);
        newObj.transform.parent = Root;
        newObj.transform.localScale = Vector3.one * Size;
		newObj.transform.localPosition = new Vector3(x * scaleSize * Size, 2);
		Vector3 vpos = new Vector3(x * scaleSize * Size, 0);
		if (newObj.GetComponent<CookieHandle>() != null)
        {
			CookieHandle b = newObj.GetComponent<CookieHandle>();
            b.xIndex = x;
            b.yIndex = 0;
        }
        iTween.MoveTo(newObj, iTween.Hash("position", vpos, "islocal", true, "time", moveTime));
    }
}
