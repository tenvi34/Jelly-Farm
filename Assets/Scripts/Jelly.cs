using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Jelly : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    [Header("Jelly Setting")] 
    public int move_delay = 6; // 다음 이동까지의 딜레이
    public int move_time = 3; // 이동 시간

    private float _speedX; // x축 이동 속도
    private float _speedY; // y축 이동 속도
    private bool isWandering;
    private bool isWalking;
    
    private void Awake()
    {
        // 초기화
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        isWandering = false;
        isWalking = false;
    }

    private void FixedUpdate()
    {
        if (!isWandering)
            StartCoroutine(Wander());
        if (isWalking)
            Move();
    }

    private IEnumerator Wander()
    {
        // Translate로 이동할 시 Object가 텔레포트 하는 것을 방지하기 위해 Time.deltaTime을 곱해줌
        _speedX = Random.Range(-0.8f, 0.8f) * Time.deltaTime;
        _speedY = Random.Range(-0.8f, 0.8f) * Time.deltaTime;

        isWandering = true;

        yield return new WaitForSeconds(move_delay);

        isWalking = true;
        _animator.SetBool("isWalk", true);	// 이동 애니메이션 실행

        yield return new WaitForSeconds(move_time);
        
        isWalking = false;
        _animator.SetBool("isWalk", false); // 이동 애니메이션 종료

        isWandering = false;
    }

    private void Move()
    {
        if (_speedX != 0)
        {
            _spriteRenderer.flipX = _speedX < 0; // x축 속도에 따라 Spite 이미지를 뒤집음
        }
            
        transform.Translate(_speedX, _speedY, _speedY);	// 젤리 이동
    }
}
