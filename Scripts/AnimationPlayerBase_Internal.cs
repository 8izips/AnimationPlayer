using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Animator))]
public abstract partial class AnimationPlayerBase : MonoBehaviour
{
    #region Animator Property
    public Animator Animator {
        get {
            if (_animator == null)
                _animator = GetComponent<Animator>();
            return _animator;
        }
    }

    public Avatar Avatar {
        get { return Animator.avatar; }
        set { Animator.avatar = value; }
    }
    public bool ApplyRootMotion {
        get { return Animator.applyRootMotion; }
        set { Animator.applyRootMotion = value; }
    }

    public AnimatorUpdateMode UpdateMode {
        get { return Animator.updateMode; }
        set { Animator.updateMode = value; }
    }

    public AnimatorCullingMode CullingMode {
        get { return Animator.cullingMode; }
        set { Animator.cullingMode = value; }
    }
    Animator _animator;
    #endregion

    protected PlayableGraph _graph;
    protected AnimationPlayable _playable;

    void Awake()
    {
        Init();
    }

    void OnEnable()
    {
        Init();        

        if (PlayOnAwake)
            Play();
    }

    void OnDisable()
    {
        if (_initialized) {
            Stop();
            //_graph.Stop();
        }
    }

    void OnDestroy()
    {
        if (_graph.IsValid())
            _graph.Destroy();
    }

    protected bool _initialized = false;
    public virtual void Init()
    {
        if (_initialized)
            return;

        _animator = GetComponent<Animator>();
        _graph = PlayableGraph.Create();
        _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        _graphConnected = false;

        AnimationPlayable template = new AnimationPlayable();
        var playable = ScriptPlayable<AnimationPlayable>.Create(_graph, template, 1);
        _playable = playable.GetBehaviour();
        
        _initialized = true;
    }
}
