using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    [Tooltip("����ƶ��ٶ�")] public float CamMoveSpeed = 5f;

    [Tooltip("��������ٶ�")] public float CamScaleSpeed = 5f;
    private float MinScale = 1f;
    private float MaxScale = 150f;
    private float CurrentScale;



    private void Awake()
    {
        CurrentScale = Camera.main.orthographicSize;


    }

    public void Update()
    {
        // �Ҽ��ƶ�
        if (Input.GetMouseButton(1))
        {
            // ��ȡ����x��y��ֵ�������ٶȺ�Time.deltaTime����Ϊ����������˶�������ƽ��  
            float h = Input.GetAxis("Mouse X") * CamMoveSpeed * Time.deltaTime;
            float v = Input.GetAxis("Mouse Y") * CamMoveSpeed * Time.deltaTime;
            // ���õ�ǰ������ƶ���y�Ტ���ı�  
            // ��Ҫ������������������ƶ��������ǰ���������������ƶ������Լ���Spance.World
            this.transform.Translate(-h, -v, 0, Space.World);
        }



        //�����ƶ�
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(new Vector2(0, 1) * CamMoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(new Vector2(0, -1) * CamMoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(new Vector2(-1, 0) * CamMoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(new Vector2(1, 0) * CamMoveSpeed * Time.deltaTime);
        }


        //����
        CurrentScale -= Input.GetAxis("Mouse ScrollWheel") * CamScaleSpeed;
        CurrentScale = Mathf.Clamp(CurrentScale, MinScale, MaxScale);
        Camera.main.orthographicSize = CurrentScale;


    }
}
