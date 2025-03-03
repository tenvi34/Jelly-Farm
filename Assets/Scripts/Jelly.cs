using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Jelly : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Coroutine _wanderCoroutine;

    [Header("Jelly Setting")] 
    [SerializeField] private float moveDelay = 6f; // 다음 이동까지의 딜레이
    [SerializeField] private float moveTime = 3f; // 이동 시간
    [SerializeField] private float moveSpeedRange = 0.8f; // 최대 이동 속도

    private Vector2 _moveDirection; // 이동 방향 벡터
    private bool _isWalking;

    private void Awake()
    {
        // 컴포넌트 초기화
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // 시작 시 움직임 코루틴 시작
        StartWandering();
    }

    private void FixedUpdate()
    {
        if (_isWalking)
        {
            Move();
        }
    }

    private void StartWandering()
    {
        // 이미 실행 중인 코루틴이 있다면 중지
        if (_wanderCoroutine != null)
        {
            StopCoroutine(_wanderCoroutine);
        }

        _wanderCoroutine = StartCoroutine(WanderCoroutine());
    }

    private IEnumerator WanderCoroutine()
    {
        while (true)
        {
            // 대기 상태
            _isWalking = false;
            _animator.SetBool("isWalk", false);
            yield return new WaitForSeconds(moveDelay);

            // 랜덤 방향 설정 (정규화된 방향 벡터)
            _moveDirection = new Vector2(
                Random.Range(-moveSpeedRange, moveSpeedRange),
                Random.Range(-moveSpeedRange, moveSpeedRange)
            ).normalized * moveSpeedRange;

            // 방향에 따라 스프라이트 뒤집기
            if (_moveDirection.x != 0)
            {
                _spriteRenderer.flipX = _moveDirection.x < 0;
            }

            // 이동 상태
            _isWalking = true;
            _animator.SetBool("isWalk", true);
            yield return new WaitForSeconds(moveTime);
        }
    }

    private void Move()
    {
        // deltaTime을 여기서 적용하여 프레임 독립적인 이동 구현
        transform.Translate(_moveDirection * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 태그를 사용하여 경계 충돌 처리
        if (collision.CompareTag("Boundary"))
        {
            // 경계면의 방향에 따라 반사 방향 계산
            if (collision.gameObject.name.Contains("Bottom") || collision.gameObject.name.Contains("Top"))
            {
                _moveDirection.y = -_moveDirection.y;
            }
            else if (collision.gameObject.name.Contains("Left") || collision.gameObject.name.Contains("Right"))
            {
                _moveDirection.x = -_moveDirection.x;
            }

            // 방향 변경 시 스프라이트 업데이트
            _spriteRenderer.flipX = _moveDirection.x < 0;
        }
    }

    // 추가: 게임 오브젝트가 비활성화될 때 코루틴 정리
    private void OnDisable()
    {
        if (_wanderCoroutine != null)
        {
            StopCoroutine(_wanderCoroutine);
            _wanderCoroutine = null;
        }
    }
}