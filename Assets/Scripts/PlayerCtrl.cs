using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Header("속성")]
    //1. 캐릭터 이동속도 설정
    [Tooltip("캐릭터 이동속도 설정")]
    public float movedStd = 2.0f;
    
    //2. 캐릭터 이동속도 설정(달리기)
    public float runMoveSpd = 3.5f;

    //3. 캐릭터 이동방향 / 회전 속도 설정
    public float DirectionRotateSpd = 100.0f;

    //4. 캐릭터 몸을 움직이는 회전 속도 설정
    public float BodyRotateSpd = 2.0f;

    //5. 캐릭터 속도 변경 증가 값
    [Range(0.1f, 50.0f)]
    public float VelocityChangeSpd = 0.1f;

    //6. 캐릭터 현재 이동 속도 설정 초기값
    private Vector3 CurrentVelocitySpd = Vector3.zero;

    //7. 캐릭터 현재 이동 방향 초기값 설정
    private Vector3 MoveDirection = Vector3.zero;

    //8. CharacterController 캐싱 준비
    private CharacterController characterCtrl = null;
    
    //9. 충돌체 받을 FLAG
    private CollisionFlags collisionFlags = CollisionFlags.None; 

    void Start()
    {
        characterCtrl = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    /// <summary>
    /// 캐릭터 이동 함수
    /// </summary>
    void Move()
    {
        //메인 카메라 Transform
        Transform cameraTransform = Camera.main.transform;

        //메인 카메라가 바라보는 방향이 월드상에서 어떤 방향인가
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        //벡터 내적
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        //키 값
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        //방향 벡터이자 목표점
        Vector3 targetDirection = vertical * forward + horizontal * right;

        //이동 방향
        MoveDirection = Vector3.RotateTowards(MoveDirection, targetDirection, DirectionRotateSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        MoveDirection = MoveDirection.normalized;

        //이동 속도
        float spd = movedStd;

        //이동하는 프레임 양
        Vector3 moveAmount = (MoveDirection * spd * Time.deltaTime);

        //실제 이동
        collisionFlags = characterCtrl.Move(moveAmount);

    }


    /// <summary>
    /// 속도를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    float GetVelocitySpd()
    {
        if(characterCtrl.velocity == Vector3.zero){
            CurrentVelocitySpd = Vector3.zero;
        }
        else{
            Vector3 retVelocitySpd = characterCtrl.velocity;
            retVelocitySpd.y = 0;
            CurrentVelocitySpd = Vector3.Lerp(CurrentVelocitySpd, retVelocitySpd, VelocityChangeSpd * Time.fixedDeltaTime);
        }
        return CurrentVelocitySpd.magnitude;
    }


    /// <summary>
    /// 화면에 글씨를 띄어주는 함수
    /// </summary>
    private void OnGUI()
    {
        if(characterCtrl != null && characterCtrl.velocity != Vector3.zero)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 1500;
            labelStyle.normal.textColor = Color.black;

            //현재 속도 
            float _getVelocity = GetVelocitySpd();
            GUILayout.Label("현재 속도 : " + _getVelocity.ToString());

            //현재 캐릭터 방향
            GUILayout.Label("현재 방향 : " + characterCtrl.velocity.ToString());

            //현재 캐릭터 속도
            GUILayout.Label("현재 캐릭터 속도 : " + CurrentVelocitySpd.magnitude.ToString());
        }   
    }

}
