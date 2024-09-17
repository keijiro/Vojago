using UnityEngine;

public sealed class AppConfig : MonoBehaviour
{
#if !UNITY_EDITOR
    void Start()
      => Cursor.visible = false;
#endif
}
