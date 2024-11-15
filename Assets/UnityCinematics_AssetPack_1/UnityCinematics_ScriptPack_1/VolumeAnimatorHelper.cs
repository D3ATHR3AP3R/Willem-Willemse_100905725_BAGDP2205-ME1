using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
[RequireComponent(typeof(Volume))]
public class VolumeAnimatorHelper : MonoBehaviour
{
    private Volume volume;
    private MotionBlur motionBlur;
    [Range(0.0f, 1.0f )] [SerializeField] float mBlurIntensity = 0.0f;
    void OnEnable()
    {
        volume = GetComponent<Volume>();
        if(volume.profile.TryGet<MotionBlur>(out var mBlur))
        {
            motionBlur = mBlur;
        }

    }
    void Update()
    {
        if(motionBlur != null)
        {
            motionBlur.intensity.value = mBlurIntensity;
        }
    }
}
