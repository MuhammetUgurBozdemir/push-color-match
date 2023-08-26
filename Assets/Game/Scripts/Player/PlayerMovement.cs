using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed;
        public DynamicJoystick _variableJoystick;
        public Rigidbody rb;


        [Inject]
        private void Construct([Inject(Id = "Dynamic")] DynamicJoystick variableJoystick)
        {
            _variableJoystick = variableJoystick;
        }


        public void FixedUpdate()
        {
            Vector3 direction = Vector3.forward * _variableJoystick.Vertical +
                                Vector3.right * _variableJoystick.Horizontal;

            rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}