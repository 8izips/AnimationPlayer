using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public partial class AnimationPlayer : AnimationPlayerBase
{
    public AnimationState GetState(string stateName)
    {
        return _states[_GetStateIndex(stateName)];
    }

    public AnimationState GetState(int index)
    {
        if (!IsStateIndexValid(index))
            return null;
        return _states[index];
    }

    public int AddState(string stateName, AnimationClip animClip)
    {
        return _AddState(stateName, animClip);
    }

    public void RemoveState(string stateName)
    {
        _RemoveState(_GetStateIndex(stateName));
    }

    public void RemoveState(int index)
    {
        _RemoveState(index);
    }

    public void Play(string stateName)
    {
        Play(_GetStateIndex(stateName));
    }

    public void Play(int index)
    {
        if (!IsStateIndexValid(index))
            return;
        if (!_graph.IsPlaying())
            Play();

        CurStateIndex = index;
        for (int i = 0; i < _states.Length; i++) {
            AnimationState state = _states[i];
            if (state == null)
                continue;

            state.SetEnable(i == index ? true : false);
            state.SetWeight(i == index ? 1.0f : 0.0f);
            state.ResetFade();
        }
    }

    public void CrossFade(string stateName, float fadeTime)
    {
        CrossFade(_GetStateIndex(stateName), fadeTime);
    }

    public void CrossFade(int index, float fadeTime)
    {
        if (!IsStateIndexValid(index))
            return;
        if (!_graph.IsPlaying())
            Play();

        CurStateIndex = index;
        _states[index].SetEnable(true);

        for (int i = 0; i < _states.Length; i++) {
            AnimationState state = _states[i];
            if (state == null)
                continue;
            if (state.enable == false)
                continue;

            float targetWeight = i == index ? 1.0f : 0.0f;
            state.SetFade(targetWeight, fadeTime);
        }
    }

    public void Blend(string stateName, float targetWeight, float fadeTime)
    {
        Blend(_GetStateIndex(stateName), targetWeight, fadeTime);
    }

    public void Blend(int index, float targetWeight, float fadeTime)
    {
        if (!IsStateIndexValid(index))
            return;

        AnimationState state = _states[index];

        if (!state.enable)
            state.SetEnable(true);

        state.SetFade(targetWeight, fadeTime);
    }

    public void Stop(string stateName)
    {
        Stop(_GetStateIndex(stateName));
    }

    public void Stop(int index)
    {
        if (!IsStateIndexValid(index))
            return;

        AnimationState state = _states[index];
        state.SetEnable(false);
        state.SetWeight(0.0f);
        state.ResetFade();
    }

    public void SetTime(float time)
    {
        for (int i = 0; i < _states.Length; i++)
            _states[i]?.SetStateTime(time);
    }

    public void Rewind()
    {
        for (int i = 0; i < _states.Length; i++)
            _states[i]?.SetStateTime(0.0f);
    }

    public void Rewind(string stateName)
    {
        Rewind(_GetStateIndex(stateName));
    }

    public void Rewind(int stateIndex)
    {
        if (IsStateIndexValid(stateIndex))
            return;

        _states[stateIndex].SetStateTime(0.0f);
    }
}
