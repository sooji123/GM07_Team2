using UnityEngine;

// 손님이 요리를 받고 식사 중인 상태 클래스
public class CustomerEat : CustomerStateBase
{
    public CustomerEat(Customer customer) : base(customer) { }
    public override void Enter()
    {
        // 식사 애니메이션
        _customer.SetColor(Color.green);
    }
    public override void Update()
    {
        // 다 먹었는지 확인, 아직 다 못먹었으면 종료
        if (!_customer.IsAte)
        {
            _customer.Eating();
            return;
        }
        // 일정 시간 지나면 식사를 마치고 Exit상태로 변경
        _customer.StateMachine.TransitionTo(_customer.StateMachine.ExitState);
    }
    public override void Exit()
    {
        
    }
}
