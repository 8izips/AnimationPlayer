using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public partial class AnimationPlayer : AnimationPlayerBase
{
    [System.Serializable]
    public class AnimationState
    {
        public string name;
        public AnimationClip clip;
        public float time = 0.0f;
        public float normalizedTime = 0.0f;
        public float duration;
        
        public AnimationClipPlayable clipPlayable { get; private set; }
        public void Init(PlayableGraph graph, bool enable, float weight)
        {
            if (graph.IsValid()) {
                clipPlayable = AnimationClipPlayable.Create(graph, clip);
                if (!clip.isLooping)
                    clipPlayable.SetDuration(clip.length);
                clipPlayable.SetApplyFootIK(applyFootIK);
            }
            duration = clip.length;

            this.enable = enable;
            this.enableDirty = false;
            this.weight = weight;
            this.weightDirty = false;
            this.fading = false;
            this.fadeSpeed = 0.0f;
            this.isLooping = clip.isLooping;
        }

        public bool enable { get; private set; } = false;
        public bool enableDirty { get; set; } = false;
        public void SetEnable(bool enable, bool enableDirty = true)
        {
            this.enable = enable;
            this.enableDirty = enableDirty;
        }
        
        public float weight { get; private set; } = 0.0f;
        public bool weightDirty { get; set; } = false;
        public void SetWeight(float weight, bool weightDirty = true)
        {
            this.weight = weight;
            this.weightDirty = weightDirty;
        }

        public bool fading { get; private set; } = false;
        public float fadeSpeed { get; private set; } = 0.0f;
        public float targetWeight { get; private set; } = 0.0f;
        public void ResetFade()
        {
            this.fading = false;
            this.fadeSpeed = 0.0f;
        }
        public void SetFade(float targetWeight, float fadeTime)
        {
            float diff = Mathf.Abs(this.weight - targetWeight);
            this.fading = diff > 0f;
            if (fadeTime == 0.0f) {
                this.fading = false;
                this.weight = targetWeight;
            }
            else {
                this.fadeSpeed = diff / fadeTime;
            }
            
            this.targetWeight = targetWeight;
        }

        [SerializeField]
        float _speed = 1.0f;
        public float speed {
            get { return _speed; }
            set {
                _speed = value;
                speedDirty = true;
            }
        }

        public bool speedDirty = false;
        public bool applyFootIK = false;
        public bool isLooping { get; private set; } = false;

        public System.Action endCallback { get; private set; }
        public void SetEndCallback(System.Action callback)
        {
            endCallback = callback;
        }
    }

    [SerializeField]
    AnimationState[] _states = new AnimationState[1];
    public AnimationState[] States { get { return _states; } set { _states = value; } }
    public int StateCount { get; private set; }
    public int CurStateIndex { get; private set; }
    public AnimationState CurState { get { return _states[CurStateIndex]; } }

    bool IsStateIndexValid(int index)
    {
        return (index >= 0 && index < _states.Length);
    }

    int _GetStateIndex(string stateName)
    {
        for (int i = 0; i < _states.Length; i++) {
            if (_states[i].name == stateName)
                return i;
        }

        return -1;
    }

    void _InitStates()
    {
        if (_states == null)
            return;

        _playable.SetInputCount(_states.Length);
        for (int i = 0; i < _states.Length; i++) {
            AnimationState state = _states[i];
            if (state == null)
                continue;

            state.Init(_graph, i == 0 ? true : false, i == 0 ? 1.0f : 0.0f);

            _playable.ConnectInput(i, state.clipPlayable);
            _playable.SetInputWeight(i, state.weight);
        }
    }

    AnimationState _GetState(int index)
    {
        if (!IsStateIndexValid(index))
            return null;

        return _states[index];
    }

    int _AddState(string stateName, AnimationClip animClip, int index = -1)
    {
        AnimationState newState = new AnimationState();
        newState.name = stateName;
        newState.clip = animClip;

        return _AddState(newState, index);
    }

    int _AddState(AnimationState newState, int index = -1)
    {
        if (!_graph.IsValid())
            return -1;

        newState.Init(_graph, false, 0.0f);
        newState.clipPlayable.Pause();

        _playable.DisconnectInputs();
        _playable.SetInputCount(_states.Length + 1);

        AnimationState[] newStates = new AnimationState[_states.Length + 1];
        // append to last
        if (index == -1) {
            index = _states.Length;
        }

        newStates[index] = newState;
        for (int i = 0; i < newStates.Length; i++) {
            if (i < index)
                newStates[i] = _states[i];
            else if (i > index)
                newStates[i + 1] = _states[i];

            _playable.ConnectInput(i, newStates[i].clipPlayable);
            _playable.SetInputWeight(i, newStates[i].weight);
        }

        _states = newStates;

        return index;
    }

    void _RemoveState(int index)
    {
        if (!IsStateIndexValid(index))
            return;

        _playable.DisconnectInput(index);

        AnimationState removedState = _states[index];        
        _states[index] = null;

        _graph.DestroyPlayable(removedState.clipPlayable);
    }
}
