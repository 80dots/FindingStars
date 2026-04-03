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
            // if (AttitudeSensor.current == null) return;
            // Quaternion attitude = AttitudeSensor.current.attitude.ReadValue();
            Quaternion attitude = context.ReadValue<Quaternion>();
            transform.localRotation = Quaternion.Euler(90, 0, 0) * new Quaternion(attitude.x, attitude.y, -attitude.z, -attitude.w);
        }

//         private void Start()
//         {
//             // 장치에서 자이로스코프(Attitude Sensor)를 지원하는지 확인하고 활성화합니다.
//             if (AttitudeSensor.current != null)
//             {
//                 InputSystem.EnableDevice(AttitudeSensor.current);
//             }
//             else
//             {
//                 Debug.LogWarning("이 장치는 자이로스코프(Attitude Sensor)를 지원하지 않습니다. Editor에서는 마우스 오른쪽 버튼으로 회전을 시뮬레이션할 수 있습니다.");
//             }
//         }
//
//         private void Update()
//         {
//             // 1. 센서가 활성화되어 있는지 확인합니다 (실제 장치).
//             if (gyroSensor != null && AttitudeSensor.current.enabled)
//             {
//                 // 센서로부터 현재 장치의 태도(Attitude) 쿼터니언 값을 읽어옵니다.
//                 Quaternion attitude = AttitudeSensor.current.attitude.ReadValue();
//
//                 // 센서 데이터(오른손 좌표계)를 Unity(왼손 좌표계)에 맞게 변환합니다. (z와 w의 부호를 반전)
//                 // 장치를 세웠을 때 카메라가 정면(휴대폰 뒷면 방향)을 바라보게 하기 위해 x축으로 90도 회전 오프셋을 적용합니다.
//                 transform.localRotation = Quaternion.Euler(90, 0, 0) * new Quaternion(attitude.x, attitude.y, -attitude.z, -attitude.w);
//             }
// #if UNITY_EDITOR
//             // 2. Editor 환경에서 마우스 오른쪽 버튼을 이용한 회전 시뮬레이션.
//             else
//             {
//                 if (Mouse.current != null && Mouse.current.rightButton.isPressed)
//                 {
//                     Vector2 delta = Mouse.current.delta.ReadValue();
//                     _yaw += delta.x * 0.1f;
//                     _pitch -= delta.y * 0.1f;
//                     transform.localRotation = Quaternion.Euler(_pitch, _yaw, 0);
//                 }
//             }
// #endif
        // }
    }
}
