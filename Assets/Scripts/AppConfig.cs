using UnityEngine;

public sealed class AppConfig : MonoBehaviour
{
    [SerializeField] GameObject _editorOnlyObject = null;
    [SerializeField] GameObject _playerOnlyObject = null;

#if UNITY_EDITOR
    void OnEnable()
    {
        _editorOnlyObject.SetActive(true);
        _playerOnlyObject.SetActive(false);
    }
#else
    void OnEnable()
    {
        _editorOnlyObject.SetActive(false);
        _playerOnlyObject.SetActive(true);
    }

    void Start()
      => Cursor.visible = false;
#endif
}
