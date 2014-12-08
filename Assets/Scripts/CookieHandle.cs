using UnityEngine;
using System.Collections;

public class CookieHandle : MonoBehaviour {
	public int xIndex;
	public int yIndex;
	[HideInInspector]
	public int Type;
	public Sprite[] sprite;
    public Sprite[] special;
	public bool isSelected = false;
	

	void Start(){
		Type = Random.Range(0,5);
		gameObject.GetComponent<SpriteRenderer>().sprite = sprite[Type];
	}

	void OnMouseDown(){
		GAction.Instance.isDrag = true;
		GAction.Instance.currentPos = new Vector3(xIndex,yIndex,Type);
	}



	void OnMouseUp(){
		GAction.Instance.isDrag = false;
		GAction.Instance.EndAction();
	}

    public void CheckStatus() {
        if (isSelected)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = special[Type];
//            transform.localScale *= 1.1f;
        }
            
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite[Type];
//            transform.localScale /= 1.1f;
        }
            
    }
}
