using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class DraggerScript : MonoBehaviour, IDragHandler, IDropHandler
{

    private Vector3 _startPos;
    public GameObject[] droppableAreas;
    public GameObject resetArea;
    private int selectedArea = -1;
    public int textIndex;

    void Awake()
    {
        _startPos = transform.position;
    }

    //this works like other unity message methods like update, start or any other
    public void OnDrag(PointerEventData eventData)
    {
        //since Input.mousePosition return cursor position on screen in pixels
        //assigning it directly to UI objects transform.position, works perfectly
        transform.position = Input.mousePosition;
        int overArea = -1;
        for(int i=0; i<droppableAreas.Length; i++){
            if(RectOverlapsRect(GetWorldRect(droppableAreas[i].GetComponent<RectTransform>()), GetWorldRect(GetComponent<RectTransform>()))){
                    if(overArea == -1){
                        // droppableAreas[i].GetComponent<UnityEngine.UI.Image>().color = new Color32(255,0,0,10);
                        overArea = i;
                    }
                    
            }else{
                    // droppableAreas[i].GetComponent<UnityEngine.UI.Image>().color = new Color32(255,255,0,100);
            }
        }

        if(RectOverlapsRect(GetWorldRect(resetArea.GetComponent<RectTransform>()), GetWorldRect(GetComponent<RectTransform>())) && overArea != -1){
            overArea = -1;
        }

        selectedArea = overArea;
        Debug.Log(selectedArea);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(selectedArea != -1){
            transform.position = droppableAreas[selectedArea].transform.position;
            transform.position = new Vector2(transform.position.x, transform.position.y-30);
        }else{
            transform.position = resetArea.transform.position;
        }
    }



    bool RectOverlapsRect (Rect rA, Rect rB) {
        return (rA.x < rB.x+rB.width && rA.x+rA.width > rB.x && rA.y < rB.y+rB.height && rA.y+rA.height > rB.y);
    }
    Rect GetWorldRect( RectTransform rectTransform)
     {
         Vector3[] corners = new Vector3[4];
         rectTransform.GetWorldCorners(corners);
         // Get the bottom left corner.
         Vector3 position = corners[0];
         
         Vector2 size = new Vector2(
             rectTransform.lossyScale.x * rectTransform.rect.size.x,
             rectTransform.lossyScale.y * rectTransform.rect.size.y);
 
         return new Rect(position, size);
     }
}

