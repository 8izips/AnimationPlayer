using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(AnimationPlayerSimple))]
public class AnimationPlayerSimpleEditor : Editor
{
    static class Styles
    {
    }

    AnimationPlayerSimple _instance;
    Animator animator;
    SerializedProperty animClip;
    void OnEnable()
    {
        _instance = (AnimationPlayerSimple)target;
        _instance.Init();

        animator = _instance.Animator;
        animClip = serializedObject.FindProperty("animClip");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Animator Property
        animator = (Animator)EditorGUILayout.ObjectField("Animator", animator, typeof(Animator), true);
        EditorGUILayout.BeginVertical("Box");
        _instance.Avatar = (Avatar)EditorGUILayout.ObjectField("Avatar", animator.avatar, typeof(Avatar), true);
        _instance.ApplyRootMotion = EditorGUILayout.Toggle("Apply Root Motion", _instance.ApplyRootMotion);
        _instance.UpdateMode = (AnimatorUpdateMode)EditorGUILayout.EnumPopup("Update Mode", _instance.UpdateMode);
        _instance.CullingMode = (AnimatorCullingMode)EditorGUILayout.EnumPopup("Culling Mode", _instance.CullingMode);
        EditorGUILayout.EndVertical();

        // State Property
        EditorGUILayout.BeginVertical("Box");

        bool isDirty = false;
        var playOnAwake = EditorGUILayout.Toggle("Play On Awake", _instance.PlayOnAwake);
        if (playOnAwake != _instance.PlayOnAwake) {
            _instance.PlayOnAwake = playOnAwake;
            isDirty = true;
        }
        var deactivateOnEnd = EditorGUILayout.Toggle("Deactivate On End", _instance.DeactivateOnEnd);
        if (deactivateOnEnd != _instance.DeactivateOnEnd) {
            _instance.DeactivateOnEnd = deactivateOnEnd;
            isDirty = true;
        }
        if (isDirty)
            EditorUtility.SetDirty(_instance);

        if (!Application.isPlaying) {
            ClipOnEditor();
        }
        else {
            ClipOnPlay();
        }
        EditorGUILayout.EndVertical();

        // Controller
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Play")) {
            _instance.Play();
        }
        if (GUILayout.Button("Stop")) {
            _instance.Stop();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    void ClipOnEditor()
    {
        EditorGUILayout.LabelField("Editor Mode", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(animClip);

        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
        }
    }

    void ClipOnPlay()
    {
        EditorGUILayout.LabelField("Play Mode", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(animClip);
    }
}