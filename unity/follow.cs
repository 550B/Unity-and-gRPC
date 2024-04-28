using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    public Transform target;
    public float distanceUp = 2f;//�����Ŀ�����ֱ�߶Ȳ���
    public float distanceAway = 2f;//�����Ŀ���ˮƽ�������
    public float smooth = 2f;//λ��ƽ���ƶ���ֵ����ֵ
    public float camDepthSmooth = 20f;

    void Update()
    {
        // �������������Զ��
        if ((Input.mouseScrollDelta.y < 0 && Camera.main.fieldOfView >= 3) || Input.mouseScrollDelta.y > 0 && Camera.main.fieldOfView <= 80)
        {
            Camera.main.fieldOfView += Input.mouseScrollDelta.y * camDepthSmooth * Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        //����������λ��
        Vector3 disPos = target.position + Vector3.up * distanceUp - target.forward * distanceAway;

        transform.position = Vector3.Lerp(transform.position, disPos, Time.deltaTime * smooth);
        //����ĽǶ�
        transform.LookAt(target.position);
    }
}