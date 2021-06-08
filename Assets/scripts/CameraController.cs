using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    [Tooltip("相机移动速度")] public float CamMoveSpeed = 5f;

    [Tooltip("相机缩放速度")] public float CamScaleSpeed = 5f;
    private float MinScale = 1f;
    private float MaxScale = 150f;
    private float CurrentScale;



    private void Awake()
    {
        CurrentScale = Camera.main.orthographicSize;


    }

    public void Update()
    {
        // 右键移动
        if (Input.GetMouseButton(1))
        {
            // 获取鼠标的x和y的值，乘以速度和Time.deltaTime是因为这个可以是运动起来更平滑  
            float h = Input.GetAxis("Mouse X") * CamMoveSpeed * Time.deltaTime;
            float v = Input.GetAxis("Mouse Y") * CamMoveSpeed * Time.deltaTime;
            // 设置当前摄像机移动，y轴并不改变  
            // 需要摄像机按照世界坐标移动，而不是按照它自身的坐标移动，所以加上Spance.World
            this.transform.Translate(-h, -v, 0, Space.World);
        }



        //键盘移动
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


        //缩放
        CurrentScale -= Input.GetAxis("Mouse ScrollWheel") * CamScaleSpeed;
        CurrentScale = Mathf.Clamp(CurrentScale, MinScale, MaxScale);
        Camera.main.orthographicSize = CurrentScale;


    }
}
