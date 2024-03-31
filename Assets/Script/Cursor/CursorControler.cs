using UnityEngine;

namespace UI
{
    public class CursorControler : MonoBehaviour
    {
        [SerializeField] private Texture2D defaultCur;
        [SerializeField, Range(0, 512)] private float positionDefaultCurX, positionDefaultCurY;
        private Vector2 setDefaultCur;

        private void Start()
        {
            setDefaultCur = new Vector2(positionDefaultCurX, positionDefaultCurY);
            Cursor.SetCursor(defaultCur, setDefaultCur, CursorMode.Auto);
        }
    }
}

