using GM07.Map;
using GM07.Order;
using UnityEngine;

namespace GM07.Test
{
    public class TestOrderTrigger : MonoBehaviour
    {
        [SerializeField]
        private TableOrderController _table;

        [SerializeField]
        private Table _sourceTable;

        private void Start()
        {
            _sourceTable.Initialize(1);
        }

        public void OnClickTestButton()
        {
            if (_sourceTable.TryRandomSeat(out Seat seat))
            {
                _table.RequestOrder(seat);
            }
            else
            {
                Debug.Log("ºó ÁÂ¼®ÀÌ ¾ø¾î¿ä");
            }
        }
    }
}