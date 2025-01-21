using UnityEngine;

namespace ClassicGame
{
    public class SpawnArea : MonoBehaviour
    {
        [SerializeField] private float _distanceFromScreenEdges = 1f;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        
        public Vector2 GetRandomPositionAroundScreen()
        {
            Vector3 screenBottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
            Vector3 screenTopRight = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, _mainCamera.nearClipPlane));

            float randomEdge = Random.Range(0, 4);
            float xPosition, yPosition;

            switch ((int)randomEdge)
            {
                case 0:
                    xPosition = Random.Range(screenBottomLeft.x, screenTopRight.x);
                    yPosition = screenTopRight.y + _distanceFromScreenEdges;
                    break;

                case 1:
                    xPosition = Random.Range(screenBottomLeft.x, screenTopRight.x);
                    yPosition = screenBottomLeft.y - _distanceFromScreenEdges;
                    break;

                case 2:
                    xPosition = screenBottomLeft.x - _distanceFromScreenEdges;
                    yPosition = Random.Range(screenBottomLeft.y, screenTopRight.y);
                    break;

                default:
                    xPosition = screenTopRight.x + _distanceFromScreenEdges;
                    yPosition = Random.Range(screenBottomLeft.y, screenTopRight.y);
                    break;
            }

            return new Vector2(xPosition, yPosition);
        }
    }
}
