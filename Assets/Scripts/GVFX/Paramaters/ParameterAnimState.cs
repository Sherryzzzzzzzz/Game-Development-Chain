//为什么Playable所有都是struct，神秘

using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[Serializable]
public class ParameterAnimState
{
        public Animator animator;
        public AnimationMixerPlayable targetMixer;
        public PlayableGraph graph;
        public AnimationClipPlayable selfCilp;
        public int mixerSlotIndex;
        public float weight;
        public float time;
        public float length;
        public enum AnimStates
        {
                Loop,
                OncePlaying,
                OnceEnd
        }
        
        public AnimStates animState;

        public ParameterAnimState(Animator animator,AnimationMixerPlayable targetMixer, PlayableGraph graph, AnimationClip selfCilp, int mixerSlotIndex, float weight, float time)
        {
                this.animator = animator;
                this.targetMixer = targetMixer;
                this.graph = graph;
                this.selfCilp = AnimationClipPlayable.Create(graph, selfCilp);
                this.graph.Connect(this.selfCilp, 0, targetMixer, this.mixerSlotIndex);
                this.targetMixer.SetInputWeight(mixerSlotIndex,weight);
                this.mixerSlotIndex = mixerSlotIndex;
                this.weight = weight;
                this.time = time;
                length = selfCilp.length;
                if (selfCilp.isLooping)
                {
                        animState = AnimStates.Loop;
                }
                else
                {
                        animState = AnimStates.OncePlaying;
                }
        }

        public void Update(float deltaTime)
        {
                this.time += deltaTime;
                this.targetMixer.SetInputWeight(mixerSlotIndex,weight);
                selfCilp.SetTime(time);

                if (animState == AnimStates.OncePlaying)
                {
                        if (time >= length)
                        {
                                animState = AnimStates.OnceEnd;
                        }
                }
        }

        public void Dispose()
        {
                //取消链接就完事了
                targetMixer.DisconnectInput(mixerSlotIndex);
                //重置动画绑定，不然没法自己修改
                animator.Rebind();
        }
}