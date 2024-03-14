using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PixelArtDrawer))]
public class PixelArtDrawerEditor : Editor
{
    bool drawToggle = false;
    void OnSceneGUI()
    {
        PixelArtDrawer drawer = target as PixelArtDrawer;
        if (drawer == null)
        {
            return;
        }

        Handles.BeginGUI();

        GUILayout.BeginArea(new Rect(100, 100, 100, 100));
        if(GUILayout.Button("New Texture"))
        {
            drawer.NewTexture();
        }
        drawToggle = GUILayout.Toggle(drawToggle, "Draw");
        GUILayout.EndArea();

        Handles.EndGUI();

        if (!drawToggle)
            return;

        if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag) && (Event.current.button == 0 || Event.current.button == 1))
        {
            Vector3 mousePosition = Event.current.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            mousePosition = ray.origin;
            drawer.DrawPixel(mousePosition, (Event.current.button == 1));
            Event.current.Use();
        }
        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
    }

}
