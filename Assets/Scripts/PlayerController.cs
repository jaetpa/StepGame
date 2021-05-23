using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;

    [SerializeField] private float jumpTimeout;
    private float _lastJumpTime;

    private float _gravity = -9.81f;
    private float _verticalSpeed = 0;

    [SerializeField] private float runBlendSpeed;
    private float _runBlendValue;

    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Blend = Animator.StringToHash("Blend");


    [Space] [SerializeField] private AudioSource playerAudioSource;

    [SerializeField] [CanBeNull] private List<AudioClip> footstepSounds;
    [SerializeField] [CanBeNull] private List<AudioClip> screamSounds;
    private static readonly int Fall = Animator.StringToHash("Fall");

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var eligibleForJump = Time.time - _lastJumpTime > jumpTimeout;
            if (eligibleForJump)
            {
                animator.SetTrigger(Jump);
                _lastJumpTime = Time.time;
            }
        }

        var horizontalMove = Input.GetAxis("Horizontal");
        var verticalMove = Input.GetAxis("Vertical");


        var blendInput = verticalMove;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _runBlendValue += runBlendSpeed * Time.deltaTime;
            _runBlendValue = Mathf.Clamp(_runBlendValue, 1, 2);
        }
        else
        {
            _runBlendValue -= runBlendSpeed * Time.deltaTime;
            _runBlendValue = Mathf.Clamp(_runBlendValue, 1, 2);
        }


        animator.SetFloat(Blend, blendInput * _runBlendValue);

        if (characterController.isGrounded)
        {
            _verticalSpeed = 0f;
        }
        else
        {
            _verticalSpeed += _gravity * Time.deltaTime;
        }

        var gravityMove = new Vector3(0, _verticalSpeed, 0);
        var move = transform.forward * verticalMove + transform.right * horizontalMove;
        characterController.Move((move * speed * _runBlendValue * Time.deltaTime) + (gravityMove * Time.deltaTime));
    }

    public void PlayStepSound()
    {
        var index = Random.Range(0, footstepSounds.Count);
        playerAudioSource.PlayOneShot(footstepSounds[index]);
    }

    public void PlayScreamSound()
    {
        var index = Random.Range(0, screamSounds.Count);
        playerAudioSource.PlayOneShot(screamSounds[index]);
    }

    public void SetFallState()
    {
        animator.SetTrigger(Fall);
    }
}