using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AnimationPlayerSimple : AnimationPlayerBase
{
    [SerializeField]
    AnimationClip animClip;
    AnimationClipPlayable clipPlayable;

    public override void Init()
    {
        if (_initialized || animClip == null)
            return;

        base.Init();

        clipPlayable = AnimationClipPlayable.Create(_playable.Graph, animClip);

        _playable.ConnectInput(0, clipPlayable);
        _playable.SetInputWeight(0, 1.0f);

        _initialized = true;
    }

    void Update()
    {
        if (!_initialized || !_graph.IsValid())
            return;

        if (DeactivateOnEnd) {
            if (clipPlayable.GetTime() > animClip.length) {
                gameObject.SetActive(false);
            }
        }
    }
}