using System.Collections;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    [Header("是否支持移动行为预测")]
    [SerializeField]
    private bool IsSupportMovePredict = false;

    [Header("最大移动行为预测数量")]
    [SerializeField]
    private int MaxMovePredictCount = 15;

    private int CurrentMovePredictCount = 0;

    [Header("移动速度")]
    [SerializeField]
    private int MoveSpeed = 15;

    [Header("是否支持预测时使用平滑处理")]
    [SerializeField]
    private bool IsSupportSmooth = true;

    [Header("平滑移动速度")]
    [SerializeField]
    private float MoveSmoothing = 0.8f;

    // 运动方向
    private Vector3 MoveDir = Vector3.zero;
    private Vector3 LoginPos = Vector3.zero;

    // 摇杆是否正在操作
    private bool JoyStickIsOperating = false;

    private void Start()
    {
        LoginPos = transform.position;
        StartCoroutine(WaitForJoyStickInit());
    }
    private IEnumerator WaitForJoyStickInit()
    {
        yield return new WaitForSeconds(1);
        YJoyStick.Instance.Init();
        YJoyStick.Instance.EnableLog(false);
        YJoyStick.Instance.DirChangedAction += (dir) =>
        {
            MoveDir = dir;
            JoyStickIsOperating = true;
        };
    }
}
public partial class Player
{
    // 模拟逻辑帧运动
    private void FixedUpdate()
    {
        if (!JoyStickIsOperating)
        {
            // 轮盘已经停止了操作
            return;
        }
        if (MoveDir == Vector3.zero)
        {
            // 轮盘正在停止操作
            JoyStickIsOperating = false;
            CurrentMovePredictCount = 0;
        }
        else
        {
            // 轮盘正在操作，更新逻辑位置
            CurrentMovePredictCount = 0;
            transform.position = LoginPos;
            LoginPos = transform.position + MoveSpeed * Time.fixedDeltaTime * MoveDir;
        }
    }
    // 模拟表现层平滑移动
    private void Update()
    {
        if (!JoyStickIsOperating)
        {
            // 轮盘已经停止了操作
            return;
        }
        if (IsSupportMovePredict && CurrentMovePredictCount <= MaxMovePredictCount)
        {
            // 支持移动行为预测
            CurrentMovePredictCount += 1;
            if (IsSupportSmooth)
            {
                Vector3 viewPos = transform.position + MoveSpeed * Time.deltaTime * MoveDir;
                transform.position = Vector3.Lerp(transform.position, viewPos, MoveSmoothing);
            }
            else
            {
                transform.position += MoveSpeed * Time.deltaTime * MoveDir;
            }
        }
    }
}
public partial class Player
{
    public void SupportMovePredict(bool support)
    {
        IsSupportMovePredict = support;
    }
    public void SupportMoveSmoothing(bool support)
    {
        IsSupportSmooth = support;
    }
    public bool GetSupportMovePredictState() { return IsSupportMovePredict; }
    public bool GetSupportMoveSmoothingState() { return IsSupportSmooth; }
}