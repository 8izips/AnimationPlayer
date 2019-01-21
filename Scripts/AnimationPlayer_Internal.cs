using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public partial class AnimationPlayer : AnimationPlayerBase
{
    public override void Init()
    {
        if (_initialized)
            return;

        base.Init();
        _InitStates();

        _initialized = true;
    }

    void OnValidate()
    {
        if (Application.isPlaying)
            return;

        if (_states == null || _states.Length == 0) {
            _states = new AnimationState[1];
        }
        if (_states[0] == null) {
            _states[0] = new AnimationState() { name = "Default" };
        }
    }

    void Update()
    {
        float elapsedTime = Time.deltaTime;

        for (int i = 0; i < _states.Length; i++) {
            AnimationState state = _states[i];

            if (state == null)
                continue;

            if (state.fading) {
                float weight = Mathf.MoveTowards(state.weight, state.targetWeight, state.fadeSpeed * elapsedTime);
                state.SetWeight(weight);
                if (state.weight == state.targetWeight) {
                    state.ResetFade();

                    if (state.weight == 0.0f) {
                        state.SetEnable(false);
                        state.SetStateTime(0.0f);
                    }
                }
            }

            if (state.enableDirty) {
                if (state.enable)
                    state.clipPlayable.Play();
                else
                    state.clipPlayable.Pause();                    
                state.enableDirty = false;
            }

            if (state.weightDirty) {
                _playable.SetInputWeight(i, state.weight);
                state.weightDirty = false;
            }

            if (state.speedDirty) {
                state.clipPlayable.SetSpeed(state.speed);
                state.speedDirty = false;
            }

            if (state.enable) {
                state.time = (float)state.clipPlayable.GetTime();

                if (state.time >= state.duration && !state.isLooping)
                    state.endCallback?.Invoke();
            }
        }
    }
}
