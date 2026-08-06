using System;
using System.Collections.Generic;
using UnityEngine;

namespace GM07.Map
{
    public sealed class TableManager : MonoBehaviour
    {
        [SerializeField]
        private Table _tablePrefab;

        [SerializeField]
        private List<Transform> _tableSpawnPointList;

        [SerializeField, Min(1)]
        private int _initialTableCount;

        private readonly List<Table> _tableList = new();
        private int _nextTableId = 1;

        public bool IsAllTablesEmpty
        {
            get
            {
                foreach (Table table in _tableList)
                {
                    if (!table.IsEmpty)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public Action OnAllTablesEmpty;

        private void Start()
        {
            InitTable();
        }

        public Table AddTable()
        {
            if (_tableList.Count >= _tableSpawnPointList.Count)
            {
                return null;
            }

            Transform spawnPoint = _tableSpawnPointList[_tableList.Count];

            Table table = Instantiate(
                _tablePrefab,
                spawnPoint.position,
                spawnPoint.rotation,
                transform);

            int tableId = GetNextTableId();
            table.Initialize(tableId);

            _tableList.Add(table);
            return table;
        }

        public bool TryUseSeat(out Table selectedTable, out Seat selectedSeat)
        {
            int maximumRemainingSeatCount = 0;
            List<Table> candidateTableList = new();

            foreach (Table table in _tableList)
            {
                int remainingSeatCount =
                    table.RemainingSeatsCount;

                if (remainingSeatCount == 0)
                {
                    continue;
                }

                if (remainingSeatCount > maximumRemainingSeatCount)
                {
                    maximumRemainingSeatCount = remainingSeatCount;

                    candidateTableList.Clear();
                    candidateTableList.Add(table);
                    continue;
                }

                if (remainingSeatCount == maximumRemainingSeatCount)
                {
                    candidateTableList.Add(table);
                }
            }

            if (candidateTableList.Count == 0)
            {
                selectedTable = null;
                selectedSeat = null;
                return false;
            }

            int randomIndex = UnityEngine.Random.Range(0, candidateTableList.Count);

            selectedTable = candidateTableList[randomIndex];

            if (selectedTable.TryRandomSeat(out selectedSeat))
            {
                return true;
            }

            selectedTable = null;
            selectedSeat = null;
            return false;
        }

        public void ReleaseSeat(Table table, Seat seat)
        {
            if (table == null || seat == null)
            {
                return;
            }
            table.ReleaseSeat(seat);

            if(IsAllTablesEmpty)
            {
                OnAllTablesEmpty?.Invoke();
            }
        }

        public bool RemoveTable(Table table)
        {
            if (table == null || !table.IsFull)
            {
                return false;
            }

            if (!_tableList.Remove(table))
            {
                return false;
            }

            Destroy(table.gameObject);
            return true;
        }

        private void InitTable()
        {
            for (int index = 0;
                 index < _initialTableCount;
                 index++)
            {
                if (AddTable() == null)
                {
                    return;
                }
            }
        }

        private int GetNextTableId()
        {
            int tableId = _nextTableId;
            _nextTableId++;

            return tableId;
        }
    }
}