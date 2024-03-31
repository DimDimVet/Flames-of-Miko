using Registrator;
using System;
using UnityEngine;
using Zenject;

namespace TemleLogic
{
    public class TempleScanerExecutor : ITempleScanerExecutor
    {
        private Construction player;
        private int tempHash;
        private Construction[] templeExecutorOn;
        public Action<Construction, int> OnFindPlayer { get { return onFindPlayer; } set { onFindPlayer = value; } }
        private Action<Construction, int> onFindPlayer;
        public Action<int> OnLossPlayer { get { return onLossPlayer; } set { onLossPlayer = value; } }
        private Action<int> onLossPlayer;

        private ITempleExecutor templeExecutor;
        private IListDataExecutor dataList;
        [Inject]
        public void Init(IListDataExecutor _dataList, ITempleExecutor _templeExecutor)
        {
            dataList = _dataList;
            templeExecutor = _templeExecutor;
            OnEnable();
        }
        private void OnEnable()
        {
            templeExecutor.OnOffTemples += TemplesOff;
        }
        private void TemplesOff(Construction[] _templeExecutorOn)
        {
            templeExecutorOn = _templeExecutorOn;
        }
        public void FindPlayer(Collider[] hitColl, int senderHash)
        {
            if (player.Hash == 0) { player = dataList.GetPlayer(); }
            else
            {
                for (int i = 0; i < hitColl.Length; i++)
                {
                    tempHash = hitColl[i].gameObject.GetHashCode();
                    if (player.Hash == tempHash && OnExecuterButton(senderHash))
                    {
                        RezultFindPlayer(player, senderHash);
                        return;
                    }
                }
                LossPlayer(senderHash);
            }
        }

        private bool OnExecuterButton(int senderHash)
        {
            if (templeExecutorOn == null) { templeExecutorOn = templeExecutor.GetTemples(); }

            if (templeExecutorOn != null)
            {
                for (int i = 0; i < templeExecutorOn.Length; i++)
                {
                    if (templeExecutorOn[i].Hash == senderHash)
                    {
                        if (templeExecutorOn[i].StatusTemle == StatusTemple.Two || templeExecutorOn[i].StatusTemle == StatusTemple.Destoy)
                        { LossPlayer(senderHash); return false; }
                    }
                }
            }
            return true;
        }

        private void RezultFindPlayer(Construction player, int recipientHash)
        {
            onFindPlayer?.Invoke(player, recipientHash);
        }
        private void LossPlayer(int recipientHash)
        {
            onLossPlayer?.Invoke(recipientHash);
        }
    }
}

