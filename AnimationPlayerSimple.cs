using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AnimationPlayerSimple : AnimationPlayerBase
{
    [SerializeField]
    AnimationClip animClip;

    public override void Init()
    {
        if (_initialized || animClip == null)
            return;

        base.Init();

        AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(_playable.Graph, animClip); ;
        _playable.ConnectInput(0, clipPlayable);
        _playable.SetInputWeight(0, 1.0f);

        _initialized = true;
    }
}
