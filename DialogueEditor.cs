/* Links to tutorials

https://youtu.be/RInUu1_8aGw A simple custom inspector, probably easier to start with than the reorderable list

https://youtu.be/yeBggZz4OYM - Unity Tutorial - Create a Custom Inspector with a Reorderable List: Part 1
Is a continuation of some other tutorial, but is how I started setting up the reorderable list.
Limited documentation, but a source of his seems to be the most detailed I can find: https://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/

https://youtu.be/_nRzoTzeyxU Once the editor is set up, I will be implementing a way of looping through the list similar to this video.

https://youtu.be/c_3DXBrH-Is Possible alternative if we can't get this working. Also has more information about how windows and other fields are drawn in the inspector.

*/

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Text;
using System;

[CustomEditor(typeof(DialogueManager))]
public class DialogueEditor : Editor
{
    DialogueManager DManager;
    ReorderableList NodeList;

    float lineHeight;
    float lineHeightSpace;

    public void OnEnable()
    {
        if(target == null)
            return;

        lineHeight = EditorGUIUtility.singleLineHeight;
        lineHeightSpace = lineHeight + 10;

        DManager = (DialogueManager)target;
        NodeList = new ReorderableList(serializedObject, serializedObject.FindProperty("Conversation"), true, true, true, true);

        NodeList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(drawElementCallback);
        NodeList.onChangedCallback = new ReorderableList.ChangedCallbackDelegate(onChangedCallback);
        NodeList.elementHeightCallback = new ReorderableList.ElementHeightCallbackDelegate(elementHeightCallback);
        NodeList.onAddCallback = new ReorderableList.AddCallbackDelegate(onAddCallback);
        NodeList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(onRemoveCallback);
        NodeList.onMouseUpCallback = new ReorderableList.SelectCallbackDelegate(onMouseUpCallback);
        NodeList.onCanRemoveCallback = new ReorderableList.CanRemoveCallbackDelegate(onCanRemoveCallback);
    }
    public void onMouseUpCallback(ReorderableList l)
    {

    }

    public override void OnInspectorGUI()
    {
        // Override from Unity Editor class
        //EditorGUI.BeginChangeCheck();
        //GUI.changed = true;

        serializedObject.Update();
        DrawDefaultInspector();
        NodeList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        /*
         * if (EditorGUI.EndChangeCheck())
        {
            // Code to execute if GUI.changed
            // was set to true inside the block of code above.
        }
        */
    }

    private void drawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
        //serializedObject.Update();
        SerializedProperty element = NodeList.serializedProperty.GetArrayElementAtIndex(index);
        //SerializedProperty dia = serializedObject.FindProperty("Dialogue").st;
        //Display Name of Speaker, and the order they speak in.
        EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width, lineHeight), element.FindPropertyRelative("name").stringValue);

        //What is the speaker saying? How Many Paragraphs?
        int c = element.FindPropertyRelative("Dialogue").arraySize; //number of items in the Dialogue Array

        for (int i = 0; i < c; i++)
        {
            //Display text areas for each paragraph
            //EditorGUI.TextField(new Rect(rect.x, rect.y + lineHeightSpace + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("Dialogue").GetArrayElementAtIndex(i).ToString());
            EditorGUI.TextField(new Rect(rect.x, rect.y + lineHeightSpace + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("Dialogue").GetArrayElementAtIndex(i).stringValue);
            //element.FindPropertyRelative("Dialogue").GetArrayElementAtIndex(i).stringValue = i.ToString();
            //Debug.Log(element.FindPropertyRelative("Dialogue").GetArrayElementAtIndex(i).stringValue);
        }
        //serializedObject.ApplyModifiedProperties();
    }
    private float elementHeightCallback(int index)
    {
        float height = 0;
        SerializedProperty element = NodeList.serializedProperty.GetArrayElementAtIndex(index);
        int c = element.FindPropertyRelative("Dialogue").arraySize; //number of items in the Dialogue Array
        for (int i = 0; i < c; i++)
        {
            height = (2 * lineHeightSpace) + (i * lineHeightSpace);
        }
        return height;
    }
    private void onAddCallback(ReorderableList l)
    {
        DManager.AddDialogue();
    }
    private void onRemoveCallback(ReorderableList l)
    {
        SerializedProperty element = NodeList.serializedProperty.GetArrayElementAtIndex(l.index);
        if (element != null)
        {
            if (EditorUtility.DisplayDialog("Warning",
                                            String.Format("Are you sure you want to delete '{0}'?", element.displayName), "Yes", "No"))
                DManager.RemoveDialogue(l.index);
        }
    }
    private void onChangedCallback(ReorderableList list)
    {
        //Int32 a = 0;
    }

    private Boolean onCanRemoveCallback(ReorderableList list)
    {
        return DManager.CanRemoveCallback();
    }
}
