using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("카메라 설정")]
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private float _borderOffset = 17.0f;
    [SerializeField]
    private float _sensitivity = 1.0f;
    [SerializeField]
    private float _zoomScale = 0.5f;
    [SerializeField]
    private float _maxZoom = 10.0f;
    [SerializeField]
    private float _minZoom = 4.0f;
    [SerializeField]
    private MeshCollider _ground;

    private bool _isPressed;
    private Vector3 _prevPosition = Vector3.zero;
    private Vector3 _max = Vector3.zero;
    private Vector3 _min = Vector3.zero;

    private void Start()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }

        if(_ground != null)
        {
            // Ground 콜라이더 기반 경계 형성
            Bounds bounds = _ground.bounds;
            _max = bounds.max;
            _min = bounds.min;
        }
    }

    private void Update()
    {
        Pressed();
        Move();
        Zoom();
    }

    private void Pressed()
    {
        // 마우스 좌클릭을 하는 경우
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 마우스 포인터가 UI 위에 없을 시
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                _isPressed = true;
                _prevPosition = Mouse.current.position.ReadValue();
            }
        }
        // 마우스 좌클릭을 해제 하는 경우
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _isPressed = false;
        }
    }
    private void Move()
    {
        if (!_isPressed)
        {
            return;
        }

        // 마우스 움직임 벡터 구하기
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Vector3 mouseVector = _prevPosition - mousePosition;

        // 2차원 마우스 좌표에서 3차원 카메라 벡터로 변환
        Vector3 moveVector = new Vector3(mouseVector.x, 0.0f, mouseVector.y);

        // 벡터 방향 돌리기
        Vector3 rotateVector = Quaternion.AngleAxis(45f, Vector3.up) * moveVector;

        // 민감도 적용된 최종 벡터 구하기
        Vector3 resultVector = rotateVector * _sensitivity * Time.deltaTime;

        // 최종 벡터 값대로 움직이기
        _camera.transform.position += resultVector;

        // 경게 제한
        _camera.transform.position = new Vector3
            (
                Mathf.Clamp(_camera.transform.position.x, _min.x - _borderOffset, _max.x - _borderOffset),
                _camera.transform.position.y,
                Mathf.Clamp(_camera.transform.position.z, _min.z - _borderOffset, (_max.z * 2.0f) - _borderOffset)
            );

        // 다음 이동을 위해 현재 위치 저장
        _prevPosition = mousePosition;
    }
    private void Zoom()
    {
        // 마우스가 UI위에 있으면 줌X
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        float size = Mouse.current.scroll.ReadValue().y;
        if (size == 0)
        {
            return;
        }
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - (size * _zoomScale), _minZoom, _maxZoom);
    }
}
