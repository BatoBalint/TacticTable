using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

[CustomEditor(typeof(LineRendererSmoother))]
public class LineRendererSmootherEditor : Editor
{

    private LineRendererSmoother Smoother;

    private SerializedProperty Line;
    private SerializedProperty InitialState;
    private SerializedProperty SmoothingLength;
    private SerializedProperty SmoothingSections;

    private GUIContent UpdateInitialStateGUICOntent = new GUIContent("Set Initial State");
    private GUIContent SmoothButtonGUIContnt = new GUIContent("Smooth Path");
    private GUIContent RestoreDefaultGUICOntent = new GUIContent("Restore Default Path");

    private bool ExpandCurves =false;
    private BezierCurves[] Curves;
    private void OnEnable()
    {
        Smoother = (LineRendererSmoother)target;

        if(Smoother.Line == null)
        {
            Smoother.Line=Smoother.GetComponent<LineRenderer>();
        }
        Line  = serializedObject.FindProperty("line");
        InitialState = serializedObject.FindProperty("InitialState");
        SmoothingLength = serializedObject.FindProperty("SmoothingLength");
        SmoothingSections = serializedObject.FindProperty("SmoothingSection");

        //Curves = new BezierCurves[Smoother.Line.positionCount - 1];
        EnsureCurvesMatchLineRendererPositions();


    }
    public override void OnIspectorGUI()
    {
        if(Smoother == null)
        {
            return;
        }
        EnsureCurvesMatchLineRendererPositions();

        EditorGUILayout.PropertyField(Line);
        EditorGUILayout.PropertyField(InitialState);
        EditorGUILayout.PropertyField(SmoothingLength);
        EditorGUILayout.PropertyField(SmoothingSections);

        if (GUILayout.Button(UpdateInitialStateGUICOntent))
        {
            Smoother.InitialState = new Vector3[Smoother.Line.positionCount];
            Smoother.Line.GetPositions(Smoother.InitialState);
        }
        EditorGUILayout.BeginHorizontal();
        {
            GUI.enabled = Smoother.Line.positionCount >= 3;
            if (GUILayout.Button(SmoothButtonGUIContnt))
            {
                SmoothPath();
            }
            bool lineRenderPathAndInitialStateAreSame = Smoother.Line.positionCount == Smoother.InitialState.Length;

            if (lineRenderPathAndInitialStateAreSame)
            {
                Vector3[] postitions = new Vector3[Smoother.Line.positionCount];
                Smoother.Line.GetPositions(postitions);

                lineRenderPathAndInitialStateAreSame = postitions.SequenceEqual(Smoother.InitialState);
            }
            GUI.enabled = !lineRenderPathAndInitialStateAreSame;
            if(GUILayout.Button(RestoreDefaultGUICOntent))
            {
                Smoother.Line.positionCount = Smoother.InitialState.Length;
                Smoother.Line.SetPositions(Smoother.InitialState);

                if(Curves.Length != Smoother.Line.positionCount - 1)
                {
                    Curves = new BezierCurves[Smoother.Line.positionCount-1];
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
    private void SmoothPath()
    {
        Smoother.Line.positionCount = Curves.Length * SmoothingSections.intValue;
        int index = 0;
        for (int i = 0; i < Curves.Length; i++)
        {
            Vector3[] segments = Curves[i].GetSegments(SmoothingSections.intValue);
            for (int j = 0; j < segments.Length; j++)
            {
                Smoother.Line.SetPosition(index, segments[j]);
                index++;
            }

        }

        //Reeset values so inspector doesn't freeze if you use a lots of smoothing sections
        SmoothingSections.intValue = 1;
        SmoothingLength.floatValue = 0;
        serializedObject.ApplyModifiedProperties();

    }
    private void OnSceneGUI()
    {
        if(Smoother.Line.positionCount < 3)
        {
            return;
        }
        EnsureCurvesMatchLineRendererPositions();

        for (int i = 0; i < Curves.Length; i++)
        {
            Vector3 position = Smoother.Line.GetPosition(i);
            Vector3 lastPosition = i == 0 ? Smoother.Line.GetPosition(0) : Smoother.Line.GetPosition(i-1);
            Vector3 nextPositon = Smoother.Line.GetPosition(i+1);

            Vector3 lastDirection = (position - lastPosition).normalized;
            Vector3 nextDirection = (nextPositon - position).normalized;

            Vector3 startTangent = (lastDirection + nextDirection) * SmoothingLength.floatValue;
            Vector3 endTangent = (nextDirection+ lastDirection) * -1 * SmoothingLength.floatValue;

            Handles.color = Color.green;
            Handles.DotHandleCap(EditorGUIUtility.GetControlID(FocusType.Passive), position + startTangent, Quaternion.identity, 0.25f, EventType.Repaint);

            if (i != 0)
            {
                Handles.color = Color.blue;
                Handles.DotHandleCap(EditorGUIUtility.GetControlID(FocusType.Passive), nextPositon + endTangent, Quaternion.identity, 0.25f, EventType.Repaint);

            }
            Curves[i].Points[0] = position;
            Curves[i].Points[1] = position + startTangent; // Start Tangent
            Curves[i].Points[2] = nextPositon + endTangent; // end Tangent
            Curves[i].Points[3] = nextPositon;
        }
        //Apply look-ahead for first curve and retroactively apply end tangent
        {
            Vector3 nextDirection = (Curves[1].EndPosition - Curves[1].StartPosition).normalized;
            Vector3 lastDirection = (Curves[0].EndPosition - Curves[0].StartPosition).normalized;

            Curves[0].Points[2] = Curves[0].Points[3] +
                (nextDirection + lastDirection) * -1 * SmoothingLength.floatValue;

            Handles.color = Color.blue;
            Handles.DotHandleCap(EditorGUIUtility.GetControlID(FocusType.Passive), Curves[0].Points[2], Quaternion.identity, 0.25f, EventType.Repaint);

        }
        DrawSegments();
    }
    private void DrawSegments()
    {

    }
    private void EnsureCurvesMatchLineRendererPositions()
    {
        if(Curves.Length != Smoother.Line.positionCount - 1)
        {
            Curves = new BezierCurves[Smoother.Line.positionCount-1];
            for (int i = 0; i < Curves.Length; i++)
            {
                Curves[i] = new BezierCurves();
            }
        }
    }



}
