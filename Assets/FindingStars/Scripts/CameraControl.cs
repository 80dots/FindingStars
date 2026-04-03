using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

namespace FindingStars
{
    /// <summary>
    /// Android 장치의 자이로스코프 센서를 이용하여 카메라를 회전시키는 클래스입니다.
    /// 휴대폰의 뒷면이 바라보는 방향을 화면에 보여주도록 설계되었습니다.
    /// </summary>
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private InputActionReference attSensor;
#if UNITY_EDITOR
        private float _yaw = 0.0f;
        private float _pitch = 0.0f;
#endif

        private void OnEnable()
        {
            if (AttitudeSensor.current != null)
            {
                InputSystem.EnableDevice(AttitudeSensor.current);
            }

            if (attSensor != null && attSensor.action != null)
            {
                attSensor.action.performed += OnGyroPerformedHandler;
                attSensor.action.Enable();
            }
        }

        private void OnDisable()
        {
            if (attSensor != null && attSensor.action != null)
            {
                attSensor.action.performed -= OnGyroPerformedHandler;
                attSensor.action.Disable();
            }
        }

        private void OnGyroPerformedHandler(InputAction.CallbackContext context)
        {
            if (context.valueType != typeof(Quaternion)) return;

            // AttitudeSensor의 데이터(오른손 좌표계)를 유니티(왼손 좌표계)에 맞게 변환합니다.
            // z와 w의 부호를 반전시키는 것이 일반적인 변환 방식입니다.
            Quaternion attitude = context.ReadValue<Quaternion>();
            Vector3 eulerAngles = attitude.eulerAngles;
            transform.localRotation = Quaternion.Euler(eulerAngles.x, 0f, 0f);
        }

#if UNITY_EDITOR
        private void Update()
        {
            // 에디터 환경에서 마우스 오른쪽 버튼을 이용한 회전 시뮬레이션.
            if (Mouse.current != null && Mouse.current.rightButton.isPressed)
            {
                Vector2 delta = Mouse.current.delta.ReadValue();
                _yaw += delta.x * 0.1f;
                _pitch -= delta.y * 0.1f;
                transform.localRotation = Quaternion.Euler(_pitch, _yaw, 0);
            }
        }
#endif
    }
}
