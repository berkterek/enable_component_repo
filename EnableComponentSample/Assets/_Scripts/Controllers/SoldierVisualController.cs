using UnityEngine;

namespace EnableComponents.Controllers
{
    public class SoldierVisualController : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] Transform _transform;
        
        static readonly int Walking = Animator.StringToHash("isWalking");
        static readonly int Attacking = Animator.StringToHash("isAttacking");

        void OnValidate()
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }

            if (_transform == null) _transform = GetComponent<Transform>();
        }

        public void IsWalking(bool value)
        {
            if (_animator.GetBool(Walking) == value) return;
            _animator.SetBool(Walking, value);
        }

        public void IsAttacking(bool value)
        {
            if (_animator.GetBool(Attacking) == value) return;
            _animator.SetBool(Attacking, value);
        }

        public void SetPosition(Vector3 position)
        {
            _transform.position = position;
        }
    }
}