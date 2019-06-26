using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(AnimationPlayer))]
public class AnimationPlayerEditor : Editor
{
    AnimationPlayer _instance;
    Animator animator;
    SerializedProperty stateProperty;
    ReorderableList states;
    void OnEnable()
    {
        _instance = (AnimationPlayer)target;
        _instance.Init();

        animator = _instance.Animator;
        stateProperty = serializedObject.FindProperty("_states");
        states = new ReorderableList(serializedObject, stateProperty);
        states.drawHeaderCallback = (rect) =>
        {
            EditorGUI.LabelField(rect, "Animation State");
        };
        states.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            var element = stateProperty.GetArrayElementAtIndex(index);
            float rectWidth = rect.width * 0.5f;
            rect.width = rectWidth - 2;
            rect.height -= 4;
            rect.y += 1;

            _instance.States[index].name = EditorGUI.TextField(rect, _instance.States[index].name);

            rect.x += rectWidth;
            var curClip = (AnimationClip)EditorGUI.ObjectField(rect, _instance.States[index].clip, typeof(AnimationClip), true);
            if (curClip != _instance.States[index].clip)
                EditorUtility.SetDirty(_instance);

            _instance.States[index].clip = curClip;
        };
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
            StateOnEditor();
        }
        else {
            StateOnPlay();
        }
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }

    void StateOnEditor()
    {
        EditorGUILayout.LabelField("Editor Mode", EditorStyles.boldLabel);

        states.DoLayoutList();
        StateDetailOnEditor();
    }

    void StateDetailOnEditor()
    {
        if (states.index == -1 || states.index >= _instance.States.Length)
            return;

        AnimationPlayer.AnimationState state = _instance.States[states.index];
        if (state == null)
            return;

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.LabelField(state.name, EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        bool isDirty = false;
        var speed = EditorGUILayout.DelayedFloatField("Speed", state.speed);
        if (speed != state.speed) {
            state.speed = speed;
            isDirty = true;
        }

        var applyFootIK = EditorGUILayout.Toggle("Apply Foot IK", state.applyFootIK);
        if (applyFootIK != state.applyFootIK) {
            state.applyFootIK = applyFootIK;
            isDirty = true;
        }

        var applyPlayableIK = EditorGUILayout.Toggle("Apply Playable IK", state.applyPlayableIK);
        if (applyPlayableIK != state.applyPlayableIK) {
            state.applyPlayableIK = applyPlayableIK;
            isDirty = true;
        }

        if (isDirty)
            EditorUtility.SetDirty(_instance);

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }

    bool playDetailFoldOpened = false;
    void StateOnPlay()
    {
        EditorGUILayout.LabelField("Play Mode", EditorStyles.boldLabel);

        // Controller
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Play"))
            _instance.Play();
        if (GUILayout.Button("Stop"))
            _instance.Stop();

        EditorGUILayout.EndHorizontal();

        // State Information
        Color weightZeroColor = new Color(0.18f, 0.3f, 0.15f, 0.7f);
        Color weightOneColor = new Color(0.68f, 0.825f, 0.65f, 0.3f);
        EditorGUILayout.BeginVertical("Box");
        EditorGUI.indentLevel++;

        playDetailFoldOpened = EditorGUILayout.Foldout(playDetailFoldOpened, playDetailFoldOpened ? "Detail" : "Simple");
        for (int i = 0; i < _instance.States.Length; i++) {
            AnimationPlayer.AnimationState state = _instance.States[i];
            if (state == null)
                continue;

            if (i == _instance.CurStateIndex)
                EditorGUILayout.LabelField(state.name, EditorStyles.boldLabel);
            else
                EditorGUILayout.LabelField(state.name);

            EditorGUILayout.ObjectField(state.clip, typeof(AnimationClip), true);
            Rect controlRect = GUILayoutUtility.GetLastRect();
            Rect timeRect = controlRect;
            timeRect.x += 16;
            timeRect.y += 1;
            timeRect.width -= 35;
            timeRect.height -= 2;
            timeRect.width *= (state.normalizedTime - (int)state.normalizedTime);
            Color weightColor = Color.Lerp(weightZeroColor, weightOneColor, state.weight);
            EditorGUI.DrawRect(timeRect, weightColor);

            if (playDetailFoldOpened) {
                EditorGUILayout.DelayedFloatField("Time", state.time);
                EditorGUILayout.DelayedFloatField("NormalizedTime", state.normalizedTime);
                EditorGUILayout.DelayedFloatField("Weight", state.weight);
                EditorGUILayout.DelayedFloatField("FadeSpeed", state.fadeSpeed);
            }
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}