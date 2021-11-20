using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header("移动行为预测")]
    [SerializeField]
    private Toggle TogPredict;

    [Header("移动行为预测平滑")]
    [SerializeField]
    private Toggle TogSmoothing;

    [Header("玩家")]
    [SerializeField]
    private Player Player;

    private void Start()
    {
        TogPredict.isOn = Player.GetSupportMovePredictState();
        TogSmoothing.isOn = Player.GetSupportMoveSmoothingState();
        TogPredict.onValueChanged.AddListener((isOn) =>
        {
            Player.SupportMovePredict(isOn);
        });
        TogSmoothing.onValueChanged.AddListener((isOn) =>
        {
            Player.SupportMoveSmoothing(isOn);
        });
    }
}