using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//：
public class CameraMove : MonoBehaviour
{
    //摄像机看向的目标
    public Transform target;
    //摄像机相对目标的偏移
    public Vector3 offsetPos;
    //看向位置的y偏移
    public float bodyHeight;

    //移动和旋转速度
    public float moveSpeed;
    public float rotateSpeed;

    //摄像机的目标位置和目标旋转
    private Vector3 targetPos;
    private Quaternion targetRotation;
    private void Start()
    {
        
    }
    private void Update()
    {
        if (target == null)
            return;
        //根据目标对象来计算摄像机当前的位置和角度
        //位置的计算
        //向后偏移
        targetPos = target.position + target.forward * offsetPos.z;
        //向上偏移
        targetPos += Vector3.up * offsetPos.y;
        //左右偏移
        targetPos += target.right * offsetPos.x;
        //插值运算缓动
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, moveSpeed * Time.deltaTime);

        //角度的计算
        //得到最终要看向的点的四元数
        targetRotation = Quaternion.LookRotation(target.position + Vector3.up * bodyHeight - this.transform.position);
        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation,rotateSpeed*Time.deltaTime);
        this.transform.rotation = targetRotation;
    }

    //设置摄像机跟随的角色
    public void SetTarget(Transform player)
    {
        target = player;
    }
}
