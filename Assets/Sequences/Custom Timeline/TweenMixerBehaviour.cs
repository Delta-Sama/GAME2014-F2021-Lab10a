using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TweenMixerBehaviour : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        base.ProcessFrame(playable, info, playerData);

        /*int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            // get the input connected to the mixer
            Playable input = playable.GetInput(i);

            // get the weight of the connection
            float inputWeight = playable.GetInputWeight(i);

            // get the clip's behaviour
            TweenBehaviour tweenInput = GetTweenBehaviour(input);
        }*/
    }
}
