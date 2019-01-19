using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public abstract partial class AnimationPlayerBase : MonoBehaviour
{
    public class AnimationPlayable : PlayableBehaviour
    {
        public Playable Playable { get; private set; }
        public PlayableGraph Graph { get { return Playable.GetGraph(); } }
        public AnimationMixerPlayable Mixer { get { return _mixer; } }
        AnimationMixerPlayable _mixer;

        public override void OnPlayableCreate(Playable playable)
        {
            Playable = playable;
            _mixer = AnimationMixerPlayable.Create(Graph, 1, true);

            Playable.SetInputCount(1);
            Playable.SetInputWeight(0, 1.0f);
            Graph.Connect(_mixer, 0, Playable, 0);
        }

        public void SetInputCount(int count)
        {
            _mixer.SetInputCount(count);
        }

        public Playable GetInput(int index)
        {
            if (index >= _mixer.GetInputCount())
                return Playable.Null;

            return _mixer.GetInput(index);
        }

        public void ConnectInput(int index, Playable playable)
        {
            Graph.Connect(playable, 0, _mixer, index);
        }

        public void DisconnectInput(int index)
        {
            Graph.Disconnect(_mixer, index);
        }

        public void DisconnectInputs()
        {
            for (int i = 0; i < _mixer.GetInputCount(); i++)
                DisconnectInput(i);
        }

        public void SetInputWeight(int index, float weight)
        {
            _mixer.SetInputWeight(index, weight);
        }
    }
}
