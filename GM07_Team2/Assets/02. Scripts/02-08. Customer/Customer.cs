using System.Collections.Generic;

using GM07.Map;
using GM07.Order;

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    [Header("손님 데이터")]
    [SerializeField]
    private CustomerData _data;
    [Header("네비게이션")]
    [SerializeField]
    private NavMeshAgent _agent;

    private float _eatTimer = 0.0f;
    private TableManager _tableManager;
    private Recipe _recipe;

    public CustomerStateMachine StateMachine { get; private set; }
    public Table Table { get; private set; }
    public Seat Seat { get; private set; }
    public Vector3 StartPos { get; private set; }
    public bool IsAte => _eatTimer >= _data.EatTime;
    public bool IsReceived { get; private set; }

    #region Test Fields
    private float _receiveTimer = 0.0f; // test
    private float _receiveTime = 2.0f; // test
    public bool IsReceiveFood => _receiveTimer >= _receiveTime; // test
    #endregion

    private void Update()
    {
        StateMachine.UpdateState();
    }

    // 스폰 시 호출되는 초기화 메서드
    public void Init(TableManager tableManager, Table table, Seat seat)
    {
        _tableManager = tableManager;
        Table = table;
        Seat = seat;

        _eatTimer = 0.0f;
        IsReceived = false;

        if(_agent == null)
        {
            TryGetComponent(out _agent);
        }
        StartPos = transform.position;

        if (StateMachine == null)
        {
            StateMachine = new CustomerStateMachine(this);
        }
        StateMachine.Initialize(StateMachine.EnterState);
    }

    // parameter로 주어진 목적지 까지의 경로를 생성하는 메서드
    public void SetDestination(Transform target)
    {
        // 방어 코드
        if(target == null)
        {
            Debug.LogWarning("NULL target 접근");
            return;
        }

        if(!_agent.SetDestination(target.position))
        {
            Debug.LogWarning(transform.name + " 경로 찾기 실패");
        }
    }
    // 오버로딩
    public void SetDestination(Vector3 target)
    {
        if(!_agent.SetDestination(target))
        {
            Debug.LogWarning(transform.name + " 경로 찾기 실패");
        }
    }

    // 현재 설정된 목적지 까지의 거리를 반환하는 메서드
    public float CalculateSqrMagnitude()
    {
        return Vector3.SqrMagnitude(transform.position -_agent.destination);
    }

    // 메뉴 주문하는 메서드
    public void OrderMenu()
    {
        if (RecipeManager.Instance != null && Table.TryGetComponent(out TableOrderController order))
        {
            List<Recipe> recipes = new List<Recipe>();
            for (int i = 0; i < RecipeManager.Instance.Count; i++)
            {
                if(RecipeManager.Instance.TryGetRecipeIndex(i, out Recipe recipe) && recipe.Unlocked)
                {
                    recipes.Add(recipe);
                }
            }

            // 해금된 레시피 랜덤으로 선택
            Recipe selectRecipe = recipes[Random.Range(0, recipes.Count)];
            // 주문 요청 (레시피 제공 예정)
            order.RequestOrder(Seat);
        }

        _receiveTimer = 0.0f; // test
    }
    public void Watting() // test
    {
        _receiveTimer += Time.deltaTime;
    }
    public void Receive()
    {
        IsReceived = true;
    }
    public void PayMoney()
    {
        if (CurrencyManager.Instance != null && _recipe != null)
        {
            CurrencyManager.Instance.AddMoney(_recipe.Data.Price, ECurrencyTransactionType.Sale);
        }
    }

    public void Eating()
    {
        _eatTimer += Time.deltaTime;
    }

    public void Release()
    {
        // 자리 반환
        if (_tableManager != null && Table != null && Seat != null)
        {
            _tableManager.ReleaseSeat(Table, Seat);
        }
        _tableManager = null;
        Table = null;
        Seat = null;
        
        // 임시로 파괴 로직으로 구현(풀링 예정)
        Destroy(gameObject);
    }
    // 애니메이션 적용 전, 상태 변경 시각화를 위한 색상 변경 메서드
    public void SetColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
}
