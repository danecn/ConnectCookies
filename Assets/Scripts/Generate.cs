using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generate : MonoBehaviour {
	public static Generate Instance;
    public float Size;
	public Transform Root;
	public Transform RootConnect;
	private Ray ray;
	private RaycastHit hit;
	public bool isDrag = false;
	public Vector3 currentPos;
	[HideInInspector]
    public GameObject currentGo;
	[HideInInspector]
	public Sprite spriteSize;

	private List<GameObject> selectedList = new List<GameObject>();
    private float moveTime = 0.8f;
	void Start(){
		GenerateBox();
		Instance = this;
	}

	void Update(){
		if(isDrag)
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				if(hit.collider.tag.Equals("Box"))
				{
					BoxHandle b = hit.collider.gameObject.GetComponent<BoxHandle>();
                    if (!b.isSelected)
                    {
						CheckValidConnect(b);
                    }
                    else
                    {
						HandleErrorConnect(b);
                    }        
				}
			}
		}
	}

	void CheckValidConnect(BoxHandle b){
		if (Mathf.Abs(currentPos.x - b.xIndex) <= 1 && Mathf.Abs(currentPos.y - b.yIndex) <= 1)
		{
			if (currentPos.z == b.Type)
			{
				currentPos = new Vector3(b.xIndex, b.yIndex, b.Type);
				selectedList.Add(hit.collider.gameObject);
				DrawNewConnectLine();
				b.isSelected = true;
				currentGo = b.gameObject;
				b.CheckStatus();
			}
		}
	}
	void HandleErrorConnect(BoxHandle b){
		if (!b.gameObject.Equals(currentGo))
		{
			GameObject go = selectedList[selectedList.Count-1];
			go.GetComponent<BoxHandle>().isSelected = false;
			go.GetComponent<BoxHandle>().CheckStatus();
			selectedList.Remove(go);
			RemoveConnectLine();
		}
	}

	void DrawNewConnectLine(){
		if(selectedList.Count > 1){
			GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/ConnectLine", typeof(GameObject)),
			                                        new Vector3(0,0,2),Quaternion.identity);
		go.transform.parent = RootConnect;
			var lr = go.GetComponent<LineRenderer>();
			lr.SetPosition(0,selectedList[selectedList.Count-1].transform.position);
			lr.SetPosition(1,selectedList[selectedList.Count-2].transform.position);
		}
	}

	void RemoveConnectLine(){
		if(RootConnect.childCount > 0)
			Destroy(RootConnect.GetChild(RootConnect.childCount-1).gameObject);
	}

	void GenerateBox(){
		for(int i=0;i<9;i++)
		{
			for(int j=0;j<9;j++)
			{
				GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/Box", typeof(GameObject)),
				                                        Vector3.zero,Quaternion.identity);
				go.transform.parent = Root;
				go.transform.localScale = Vector3.one * Size;
				go.transform.localPosition= new Vector3(j*0.6f*Size,-i*0.6f*Size);
				if(go.GetComponent<BoxHandle>() != null)
				{
					BoxHandle b = go.GetComponent<BoxHandle>();
					b.xIndex = j;
					b.yIndex = i;
				}

                GameObject grid = (GameObject)Instantiate(Resources.Load("Prefabs/Grid", typeof(GameObject)), 
				                                          Vector3.zero, Quaternion.identity);
                grid.transform.parent = Root;
                grid.transform.localScale = Vector3.one / Size;
                grid.transform.localPosition = new Vector3(j * 0.6f * Size, -i * 0.6f * Size);
                if (grid.GetComponent<Grid>() != null)
                {
                    Grid b = go.GetComponent<Grid>();
                    b.xIndex = j;
                    b.yIndex = i;
                }
			}
		}
	}

    public void EndAction()
    {
        if (selectedList.Count > 2)
        {
            foreach (GameObject go in selectedList)
            {
                int x = go.GetComponent<BoxHandle>().xIndex;
                int y = go.GetComponent<BoxHandle>().yIndex;
                Destroy(go);
                MovePieces(x, y);
            }
        }
        else
        {
            foreach (GameObject go in selectedList)
            {
                go.GetComponent<BoxHandle>().isSelected = false;
                go.GetComponent<BoxHandle>().CheckStatus();
            }
        }

        selectedList.Clear();
		RemoveAllConnectLine();
    }

	void RemoveAllConnectLine(){
		for(int i=0;i<RootConnect.childCount;i++){
			Destroy(RootConnect.GetChild(i).gameObject);
		}
	}

    void MovePieces(int x, int y)
    {
        if (y > 0)
        {
            GameObject[] list = GameObject.FindGameObjectsWithTag("Box");
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].GetComponent<BoxHandle>().xIndex == x)
                {
                    if (list[i].GetComponent<BoxHandle>().yIndex < y)
                    {
                        list[i].GetComponent<BoxHandle>().yIndex += 1;
                        Vector3 pos = new Vector3(list[i].transform.localPosition.x,
                            -list[i].GetComponent<BoxHandle>().yIndex * 0.6f * Size);
                        iTween.MoveTo(list[i].gameObject, iTween.Hash("position", pos, "islocal", true, "time", moveTime));
                    }
                }
            }
        }
        GameObject newObj = (GameObject)Instantiate(Resources.Load("Prefabs/Box", typeof(GameObject)), 
		                                            Vector3.zero, Quaternion.identity);
        newObj.transform.parent = Root;
        newObj.transform.localScale = Vector3.one * Size;
        newObj.transform.localPosition = new Vector3(x * 0.6f * Size, 2);
        Vector3 vpos = new Vector3(x * 0.6f * Size, 0);
        if (newObj.GetComponent<BoxHandle>() != null)
        {
            BoxHandle b = newObj.GetComponent<BoxHandle>();
            b.xIndex = x;
            b.yIndex = 0;
        }
        iTween.MoveTo(newObj, iTween.Hash("position", vpos, "islocal", true, "time", moveTime));
    }
}
