using System;
using System.Collections.Generic;
using UnityEngine;

namespace GM07.Map
{
    public sealed class Table : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> _seatAnchorList;

        private readonly List<Seat> _seats = new();

        public int TableId { get; private set; }
        public int RemainingSeatsCount
        {
            get
            {
                int count = 0;
                foreach (Seat seat in _seats)
                {
                    if (!seat.IsUsing)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public bool IsFull => RemainingSeatsCount == 0;
        public bool IsEmpty => RemainingSeatsCount == _seats.Count;

        public void Initialize(int id)
        {
            TableId = id;
            _seats.Clear();

            for(int i=0;i< _seatAnchorList.Count; i++)
            {
                Transform anchor = _seatAnchorList[i];
                if(anchor == null)
                {
                    continue;
                }
                Seat seat = new Seat(i, anchor);
                _seats.Add(seat);
            }
        }

        public bool TryRandomSeat(out Seat seat)
        {
            List<Seat> availableSeatList = new();
            foreach (Seat s in _seats)
            {
                if (!s.IsUsing)
                {
                    availableSeatList.Add(s);
                }
            }
            if (availableSeatList.Count == 0)
            {
                seat = null;
                return false;
            }
            int randomIndex = UnityEngine.Random.Range(0, availableSeatList.Count);
            seat = availableSeatList[randomIndex];
            return seat.TryUse();
        }

        public void ReleaseSeat(Seat seat)
        {
            if (seat == null || !_seats.Contains(seat))
            {
                return;
            }
            seat.TryRelease();
        }
    }
}
