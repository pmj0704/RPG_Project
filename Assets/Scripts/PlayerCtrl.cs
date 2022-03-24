using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Header("�Ӽ�")]
    //1. ĳ���� �̵��ӵ� ����
    [Tooltip("ĳ���� �̵��ӵ� ����")]
    public float movedStd = 2.0f;
    
    //2. ĳ���� �̵��ӵ� ����(�޸���)
    public float runMoveSpd = 3.5f;

    //3. ĳ���� �̵����� / ȸ�� �ӵ� ����
    public float DirectionRotateSpd = 100.0f;

    //4. ĳ���� ���� �����̴� ȸ�� �ӵ� ����
    public float BodyRotateSpd = 2.0f;

    //5. ĳ���� �ӵ� ���� ���� ��
    [Range(0.1f, 50.0f)]
    public float VelocityChangeSpd = 0.1f;

    //6. ĳ���� ���� �̵� �ӵ� ���� �ʱⰪ
    private Vector3 CurrentVelocitySpd = Vector3.zero;

    //7. ĳ���� ���� �̵� ���� �ʱⰪ ����
    private Vector3 MoveDirection = Vector3.zero;

    //8. CharacterController ĳ�� �غ�
    private CharacterController characterCtrl = null;
    
    //9. �浹ü ���� FLAG
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
    /// ĳ���� �̵� �Լ�
    /// </summary>
    void Move()
    {
        //���� ī�޶� Transform
        Transform cameraTransform = Camera.main.transform;

        //���� ī�޶� �ٶ󺸴� ������ ����󿡼� � �����ΰ�
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        //���� ����
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        //Ű ��
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        //���� �������� ��ǥ��
        Vector3 targetDirection = vertical * forward + horizontal * right;

        //�̵� ����
        MoveDirection = Vector3.RotateTowards(MoveDirection, targetDirection, DirectionRotateSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        MoveDirection = MoveDirection.normalized;

        //�̵� �ӵ�
        float spd = movedStd;

        //�̵��ϴ� ������ ��
        Vector3 moveAmount = (MoveDirection * spd * Time.deltaTime);

        //���� �̵�
        collisionFlags = characterCtrl.Move(moveAmount);

    }


    /// <summary>
    /// �ӵ��� ��ȯ�ϴ� �Լ�
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
    /// ȭ�鿡 �۾��� ����ִ� �Լ�
    /// </summary>
    private void OnGUI()
    {
        if(characterCtrl != null && characterCtrl.velocity != Vector3.zero)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 1500;
            labelStyle.normal.textColor = Color.black;

            //���� �ӵ� 
            float _getVelocity = GetVelocitySpd();
            GUILayout.Label("���� �ӵ� : " + _getVelocity.ToString());

            //���� ĳ���� ����
            GUILayout.Label("���� ���� : " + characterCtrl.velocity.ToString());

            //���� ĳ���� �ӵ�
            GUILayout.Label("���� ĳ���� �ӵ� : " + CurrentVelocitySpd.magnitude.ToString());
        }   
    }

}
