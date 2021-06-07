using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateRadar : MonoBehaviour {

    private float rim;

    public bool AnimatingRim = false;

    [Range(0.001f, 1.0f)]
    public float RimMargin;
    public float RimSpeed;

    public bool AnimatingNoise = false;
    public Vector2 NoiseSpeed;

    Material Mat;

    private float RimWidth;
    private float RimFalloff;
    private Vector2 NoiseOffset = new Vector2(0, 0);
    // Use this for initialization
    void Awake () {
        Mat = GetComponent<Renderer>().material;
        RimFalloff = Mat.GetFloat("_RimFalloff");
        RimWidth = Mat.GetFloat("_RimWidth");
    }
	
	// Update is called once per frame
	void Update () {

        if (AnimatingRim) { AnimateRim(AnimatingRim, RimMargin, RimSpeed); }
        if (AnimatingNoise) { AnimateNoise(AnimatingNoise, NoiseOffset); }

    }

    public void AnimateRim(bool animate, float margin, float speed)
    {
        rim = Mathf.PingPong(Time.time * (speed* RimFalloff), RimFalloff * margin);
        Mat.SetFloat("_animateRimWidth",(-RimWidth / 2)+((rim/2)-rim));
    }
    public void AnimateNoise(bool animate, Vector3 NoiseOffset)
    {
        NoiseOffset.x += Time.deltaTime * NoiseSpeed.x;
        NoiseOffset.y += Time.deltaTime * NoiseSpeed.y;
        Mat.SetTextureOffset("_NoiseMap", NoiseOffset);
    }
}
