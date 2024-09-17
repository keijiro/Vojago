using UnityEngine;
using UnityEngine.Playables;

public sealed class AppConfig : MonoBehaviour
{
    [SerializeField] PlayableDirector _mainTimeline = null;
    [SerializeField] PlayableDirector _editorTimeline = null;
    [SerializeField] PlayableDirector _playerTimeline = null;

#if UNITY_EDITOR
    void OnEnable()
    {
        _editorTimeline.gameObject.SetActive(true);
        _playerTimeline.gameObject.SetActive(false);
    }
#else
    void OnEnable()
    {
        _editorTimeline.gameObject.SetActive(false);
        _playerTimeline.gameObject.SetActive(true);
    }

    void Start()
      => Cursor.visible = false;
#endif

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) _mainTimeline.Play();
    }
}
