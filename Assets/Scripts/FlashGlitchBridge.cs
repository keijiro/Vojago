using UnityEngine;
using UnityEngine.Video;
using FlashGlitch;
using Metavido.Decoder;

namespace Vojago {

public sealed class FlashGlitchBridge : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float _randomize = 0.2f;

    public void OnNoteChannel0(int note, float velocity)
      => OnTrigger(0, velocity);

    public void OnNoteChannel1(int note, float velocity)
      => OnTrigger(1, velocity);

    void OnTrigger(int index, float value)
    {
        if (value == 0 || _controller == null) return;
        value *= Random.Range(1 - _randomize, 1.0f);
        _controller.TriggerEffect(index, value);
        _controller.RandomizeSeed();
        if (Random.value < 0.01f) _controller.RandomizeHue();
    }

    FlashGlitchController _controller;
    TextureDemuxer _demuxer;

    void Start()
    {
        _controller = GetComponent<FlashGlitchController>();
        _demuxer = FindFirstObjectByType<TextureDemuxer>();
    }

    void Update()
    {
        if (_demuxer.ColorTexture == null) return;
        _controller.Source = _demuxer.ColorTexture;
    }
}

} // namespace BibcamStage
