using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GAction : MonoBehaviour {
	public static GAction Instance;
	private Ray ray;
	private RaycastHit hit;
	public bool isDrag = false;

	public Transform Root;
	public Transform RootConnect;
	
	private List<GameObject> selectedList = new List<GameObject>();

	public Vector3 currentPos;
	[HideInInspector]
	public GameObject currentGo;
	[HideInInspector]
	public Sprite spriteSize;

	void Awake(){
		Instance = this;
	}

	void Update(){
		if(isDrag)
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				if(hit.collider.tag.Equals(ResourceStr.CookieTag))
				{
					CookieHandle b = hit.collider.gameObject.GetComponent<CookieHandle>();
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
	
	void CheckValidConnect(CookieHandle b){
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
	void HandleErrorConnect(CookieHandle b){
		if (!b.gameObject.Equals(currentGo))
		{
			GameObject go = selectedList[selectedList.Count-1];
			go.GetComponent<CookieHandle>().isSelected = false;
			go.GetComponent<CookieHandle>().CheckStatus();
			selectedList.Remove(go);
			RemoveConnectLine();
		}
	}
	
	void DrawNewConnectLine(){
		if(selectedList.Count > 1){
			GameObject go = (GameObject)Instantiate(Resources.Load(ResourceStr.ConnectPrefabs
			                                                       , typeof(GameObject)),
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

	public void EndAction()
	{
		if (selectedList.Count > 2)
		{
			foreach (GameObject go in selectedList)
			{
				int x = go.GetComponent<CookieHandle>().xIndex;
				int y = go.GetComponent<CookieHandle>().yIndex;
				Destroy(go);
				Generate.Instance.MovePieces(x, y);
			}
		}
		else
		{
			foreach (GameObject go in selectedList)
			{
				go.GetComponent<CookieHandle>().isSelected = false;
				go.GetComponent<CookieHandle>().CheckStatus();
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
}
