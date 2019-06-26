using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Animator))]
public abstract partial class AnimationPlayerBase : MonoBehaviour
{
    public bool PlayOnAwake = true;
    public bool DeactivateOnEnd = false;

    public bool IsPlaying()
    {
        if (!_graph.IsValid())
            return false;
        return _graph.IsPlaying();
    }

    bool _graphConnected = false;
    public void Play()
    {
        if (!_graphConnected) {
            AnimationPlayableUtilities.Play(Animator, _playable.Playable, _playable.Graph);
            _graphConnected = true;
        }
        else {
            _graph.Play();
        }
    }

    public void Stop()
    {
        if (_graph.IsValid())
            _graph.Stop();
    }

    public void Pause()
    {
        if (_graph.IsPlaying())
            _graph.Stop();
    }

    public void Resume()
    {
        if (!_graph.IsPlaying())
            _graph.Play();
    }

    public void Evaluate()
    {
        if (_graph.IsValid())
            _graph.Evaluate();
    }
}
