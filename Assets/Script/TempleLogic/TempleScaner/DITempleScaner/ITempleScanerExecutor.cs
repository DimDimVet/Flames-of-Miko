using Registrator;
using System;
using UnityEngine;

namespace TemleLogic
{
    public interface ITempleScanerExecutor
    {
        public void FindPlayer(Collider[] hitColl, int senderHash);
        public Action<Construction, int> OnFindPlayer { get; set; }
        public Action<int> OnLossPlayer { get; set; }
    }
}
