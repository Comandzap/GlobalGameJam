using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

    //global variables for settings
    bool showList = false;
    static int listEntrySelected;
    static int listEntry = 2;
    static int defaultEntryNumber = 0;
    GUIStyle generalListStyle 	= new GUIStyle();

    //dropdown menu content
    GUIContent[] listColours; 

    

    int dropdownListHash = "DropdownList".GetHashCode();

    //		List(Rect(0,0,100,100), 		false, 				0, 				GUIContent("Select Colour"), 	listColours				"button",				"box",			generalListStyle)
    void List(Rect position, bool expandList, int listEntry, GUIContent defaultListEntry, GUIContent[] listToUse , GUIStyle buttonStyle, GUIStyle boxStyle, GUIStyle listStyle)
    {

        int controlID = GUIUtility.GetControlID(dropdownListHash, FocusType.Passive);
        bool done = false;

        if (Event.current.GetTypeForControl(controlID) == EventType.mouseDown)
        {
            if (position.Contains(Event.current.mousePosition))
            {
                GUIUtility.hotControl = controlID;
                showList = !showList;
            }
        }

        if (Event.current.GetTypeForControl(controlID) == EventType.mouseDown && !position.Contains(Event.current.mousePosition))
	{
            GUIUtility.hotControl = controlID;
        }

        GUI.Label(position, defaultListEntry, buttonStyle);

        if (expandList)
        {
            //list rectangle
            Rect listRect = new Rect(position.x, position.y + 20, position.width, listStyle.CalcHeight(listToUse[0], 1.0f) * listToUse.Length);
            GUI.Box(listRect, "", boxStyle);

            listEntrySelected = GUI.SelectionGrid(listRect, listEntrySelected, listToUse, 1, listStyle);
            listEntry = listEntrySelected;

            if (listEntrySelected != defaultEntryNumber && !position.Contains(Event.current.mousePosition))
		{
                GUIUtility.hotControl = controlID;
                showList = !showList;
                defaultEntryNumber = listEntrySelected;
            }
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(200, 200, 100, 20), listColours[listEntrySelected].text);
        List(new Rect(50, 100, 100, 20), showList, listEntry, new GUIContent(listColours[listEntrySelected].text), listColours, "button", "box", generalListStyle);
    }

    // Use this for initialization
    void Start () {
        listColours = new GUIContent[5];
        listColours[0] = new GUIContent("Blue");
        listColours[1] = new GUIContent("White");
        listColours[2] = new GUIContent("Red");
        listColours[3] = new GUIContent("Green");
        listColours[4] = new GUIContent("Purple");

        generalListStyle.padding.left = generalListStyle.padding.right = generalListStyle.padding.top = generalListStyle.padding.bottom = 4;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
