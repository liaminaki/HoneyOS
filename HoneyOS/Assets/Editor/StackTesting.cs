using System;
using UnityEngine;
using UnityEditor;

public class StackTesting : EditorWindow
{
    public FiniteStack<byte[]> undoStack;
    public FiniteStack<byte[]> redoStack;

    public UndoRedo currentSI;

    private int intVal;
    private string stringVal;

    public StackTesting(){
        titleContent = new GUIContent ("Undo Testing");

        currentSI = new UndoRedo(0, "Hello");

        undoStack = new FiniteStack<byte[]>();
        redoStack = new FiniteStack<byte[]>();

        position = new Rect(400f, 400f, 1000f, 500f);
    }

    void OnGUI(){
        if (currentSI != null){
            intVal = currentSI.myNum;
            stringVal = currentSI.myString;

            intVal = EditorGUILayout.IntField(currentSI.myNum);
            stringVal = EditorGUILayout.TextField(currentSI.myString);

            if (intVal != currentSI.myNum || stringVal.Equals(currentSI.myString)){
                undoStack.Push(currentSI.GetBytes());

                currentSI.myNum = intVal;
                currentSI.myString = stringVal;
            }

            if (GUILayout.Button ("Undo", GUILayout.ExpandWidth(false))){
                if(undoStack.Count > 0){
                    redoStack.Push(currentSI.GetBytes());

                    byte[] undoArray = undoStack.Pop();

                    if(undoArray != null){
                        UndoRedo prev = UndoRedo.Load(undoArray);

                        if(prev != null){
                            currentSI = prev;
                            Repaint();
                        }
                    }
                }
            }

            if (GUILayout.Button("Redo", GUILayout.ExpandWidth(false))){
                if(redoStack.Count > 0){
                    undoStack.Push(currentSI.GetBytes());

                    byte[] redoArray = redoStack.Pop();

                    if(redoArray != null){
                        UndoRedo prev = UndoRedo.Load(redoArray);

                        if(prev != null){
                            currentSI = prev;
                            Repaint();
                        }
                    }
                }
            }

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Z){
                if (Event.current.modifiers != EventModifiers.Shift){
                    Event.current.Use();
                    Debug.Log("Undo");
                    Undo();
                }
                else if (Event.current.modifiers != EventModifiers.Shift){
                    Debug.Log("Redo");
                    Redo();
                    Event.current.Use();
                }
            }
        }
    }

    private void Undo (){
        if (undoStack.Count > 0){
            redoStack.Push(currentSI.GetBytes());

            byte [] undoArray = undoStack.Pop();

            if(undoArray != null){
                UndoRedo prev = UndoRedo.Load(undoArray);
                if(prev != null){
                    currentSI = prev;
                    Repaint();
                }
            }
        }
    }

    private void Redo (){
        if (redoStack.Count > 0){
            undoStack.Push(currentSI.GetBytes());

            byte [] redoArray = redoStack.Pop();

            if(redoArray != null){
                UndoRedo prev = UndoRedo.Load(redoArray);
                if(prev != null){
                    currentSI = prev;
                    Repaint();
                }
            }
        }
    }
}
