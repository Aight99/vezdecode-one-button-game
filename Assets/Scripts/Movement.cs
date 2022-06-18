using System;
using DG.Tweening;
using UnityEngine;
using Color = UnityEngine.Color;

public class Movement : MonoBehaviour
{
    [SerializeField] private Transform followingCameraTransform;
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private Material materialIdle; 
    [SerializeField] private Color colorIdle;
    [SerializeField] private Material materialTouch;
    [SerializeField] private Color colorTouch;
    [SerializeField] private float punchForce;
    [SerializeField] private float punchDelay;
    [SerializeField] private float eyeDistance;
    [SerializeField] private float eyeSpeed;
    [SerializeField] private float cameraVerticalOffset;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Popup levelUpPopup;
    private Rigidbody _rigidbody;
    private MeshRenderer _mesh;
    private float _eyeAngle;
    private float _delay;
    private int _rotationDirectionModifier;
    [SerializeField] private int _size;
    [SerializeField] private int _xp;
    public static event Action OnEnemyEaten;
    private readonly int[] needXp =
    {
        3,
        5,
        10,
        15,
        25,
        50,
        75,
        100,
        100
    };


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _mesh = GetComponent<MeshRenderer>();
        _eyeAngle = 0;
        _rotationDirectionModifier = -1;
        _xp = 0;
        _size = 1;
    }

    private void Update()
    {
        if (transform.localScale.y <= -10)
        {
            Debug.Log("You're won!");
        }
        
        var eyeVector = new Vector3(Mathf.Cos(_eyeAngle * Mathf.Deg2Rad), 0, Mathf.Sin(_eyeAngle * Mathf.Deg2Rad));
        
        if (_delay <= punchDelay)
        {
            _delay += Time.deltaTime;
        }
        if (_delay >= punchDelay)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                DoPunch();
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.AddForce(eyeVector * -1 * punchForce, ForceMode.Impulse);
                _delay = 0;
                _rotationDirectionModifier *= -1;
            }
        }

        _eyeAngle = (_eyeAngle + eyeSpeed * Time.deltaTime * _rotationDirectionModifier) % 360;
        if (Input.GetKey(KeyCode.Space))
        {
            _mesh.material = materialTouch;
            _mesh.material.color = colorTouch;
        }
        else
        {
            _mesh.material = materialIdle;
            _mesh.material.color = colorIdle;
        }

        var transformPosition = transform.position;
        // followingCameraTransform.position = new Vector3(transformPosition.x, followingCameraTransform.position.y, transformPosition.z);
        eyeTransform.position = eyeVector * eyeDistance + transformPosition;
    }

    private void OnMouseDrag()
    {
        if (_delay >= punchDelay)
        {
            var eyeVector = new Vector3(Mathf.Cos(_eyeAngle * Mathf.Deg2Rad), 0, Mathf.Sin(_eyeAngle * Mathf.Deg2Rad));
            DoPunch();
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(eyeVector * -1 * punchForce, ForceMode.Impulse);
            _delay = 0;
            _rotationDirectionModifier *= -1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy t))
        {
            var size = t.GetSize();
            if (size < _size)
            {
                t.PopUp();
                AddXp(t.GetPoints());
                var enemyTransform = other.transform;
                enemyTransform.DOScale(enemyTransform.localScale * 3, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    enemyTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).OnComplete(() =>
                    {
                        Destroy(other.gameObject);
                    });
                });
                OnEnemyEaten?.Invoke();
            }
        }
    }

    private void FixedUpdate()
    {
        var targetPosition = transform.position + new Vector3(0, cameraVerticalOffset, 0);
        var smoothedPosition = Vector3.Lerp(followingCameraTransform.position, targetPosition, punchForce / 30);
        followingCameraTransform.position = smoothedPosition;
    }

    private void AddXp(int xp)
    {
        _xp += xp;
        var lvl = _size - 1;
        if (lvl >= needXp.Length - 1)
        {
            return;
        }
        if (_xp >= needXp[lvl])
        {
            var tr = transform;
            var trPos = tr.localScale;
            transform.DOScale( new Vector3(trPos.x * 1.2f, trPos.y, trPos.z * 1.2f), 0.5f);
            Instantiate(levelUpPopup, tr.position + Vector3.up * 2, Quaternion.AngleAxis(90, Vector3.right));
            _xp = 0;
            _size++;
            punchForce += 2;
            punchDelay -= 0.15f;
            eyeSpeed += 30;
            eyeDistance *= 1.15f;
            cameraVerticalOffset *= 1.15f;
        }
    }

    private void DoPunch()
    {
        var duration = 0.5f;
        transform.DOPunchScale(Vector3.down, duration, 0, 0);
    }
}
