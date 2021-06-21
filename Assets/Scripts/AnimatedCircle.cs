using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedCircle : MonoBehaviour
{
    void Start()
    {
        Animate();
    }

    void Animate()
    {
        LeanTween.scale(gameObject, new Vector3(15, 15), 3f).setEaseShake().setLoopPingPong();
    }
}
