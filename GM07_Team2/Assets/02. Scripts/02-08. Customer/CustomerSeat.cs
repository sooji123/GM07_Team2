using UnityEngine;

// 빈 자리에 도착하고 나서 자리에 앉고 주문 요청하는 클래스
public class CustomerSeat : CustomerStateBase
{
    public CustomerSeat(Customer customer) : base(customer) { }
    public override void Enter()
    {
        // 자리에 앉기(임시로 색상만 변경, 추후에 앉는 자세와 Seat위치에 배치 로직 추가 예정)
        _customer.SetColor(Color.yellow);

        // 주문 신청 알림
        _customer.OrderMenu();
    }
    public override void Update()
    {
        // 음식 받으면 식사 상태로 변경
        if (_customer.IsReceived)
        {
            _customer.StateMachine.TransitionTo(_customer.StateMachine.EatState);
            return;
        }

        // test
        if (!_customer.IsReceiveFood)
        {
            _customer.Watting();
            return;
        }
        _customer.StateMachine.TransitionTo(_customer.StateMachine.EatState);
    }
    public override void Exit()
    {

    }
}
