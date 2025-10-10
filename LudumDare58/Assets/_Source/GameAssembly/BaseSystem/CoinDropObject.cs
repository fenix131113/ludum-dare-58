using UnityEngine;
using Random = UnityEngine.Random;

namespace BaseSystem
{
    public class CoinDropObject : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float minRndForceX;
        [SerializeField] private float rndForceX;
        [SerializeField] private float minRndForceY;
        [SerializeField] private float rndForceY;
        [SerializeField] private float rotateForce;
        [SerializeField] private float gravityStep;
        
        private float _gravity;
        
        private void Start()
        {
            var forceX = Random.Range(minRndForceX, rndForceX);
            rb.AddForce(new Vector2(Random.Range(0, 2) == 0 ? forceX : -forceX, Random.Range(minRndForceY, rndForceY)),
                ForceMode2D.Impulse);
            
              rb.AddTorque(Random.Range(0, 2) == 0 ? Random.Range(rotateForce / 2, rotateForce) : -Random.Range(rotateForce / 2, rotateForce), ForceMode2D.Impulse);
            Destroy(gameObject, 4f);
        }

        private void FixedUpdate()
        {
            _gravity += gravityStep;
            transform.position += Vector3.down * (Time.fixedDeltaTime * _gravity);
        }
    }
}