using Registrator;
using TemleLogic;
using UnityEngine;
using Zenject;

namespace EntityLogic
{
    public class ExecutorEntity : MonoBehaviour
    {
        private Construction player;//это просто для времено для теста, надо подумать какие параметры к нему добавить или не добавить..
        //позже я сюда через зенжект прокину, будеш получать player по срабатыванию сканера
        
        
        [SerializeField] private ModeMove modeMove;
        [Header("Процент замедления игрока")]
        [SerializeField] private float slowSpeedPercentage;
        [Header("Время замедления игрока")]
        [SerializeField] private float slowdownTime;
        [Header("Урон вере")]
        [SerializeField] private float faithDamage;
        private float timeCheckpoint;

        private int thisHash;

        private ILogicEntityExecutor logicEntityExecutor;
        private IEntityScanerExecutor scan;
        private ITempleExecutor templeExecutor;
        [Inject]
        public void Init(IEntityScanerExecutor _scan,ILogicEntityExecutor _logicEntityExecutor, ITempleExecutor _templeExecutor)
        {
            logicEntityExecutor=_logicEntityExecutor;
            scan = _scan;
            templeExecutor = _templeExecutor;
        }
        private void OnEnable()
        {
            scan.OnFindPlayer += FindPlayer;
            scan.OnLossPlayer += LossPlayer;
        }
        void Start()
        {
            thisHash = this.gameObject.GetHashCode();
            //чудики одинаковые, но разделяются  через режим перемещения, типа по разному двигаются, разные антибафы, хрень полная))
            //ну и разделяй их по этому признаку, каждый скрипт вешаем на чудика
            if (modeMove == ModeMove.Line) { }//чудики которые перемещаются от точки к точке
            if (modeMove == ModeMove.Circle) { }//чудики которые перемещаются по окружности
            timeCheckpoint = Time.time;
        }
        private void FindPlayer(Construction _player, int recipientHash)//в случае нашли объект
        {
            player = _player;
            if (recipientHash == thisHash) 
            {
                /*Debug.Log($"PlayerFind {player.Hash} entyti={recipientHash}"); */
                SlowdownDebuff();
                FaithDamageDebuff();
            }
            //logicEntityExecutor.MinusSpeedPlayer(50f,3f);//передать сюда (процент,время), цифры для примера поставил
        }
        private void LossPlayer(int recipientHash)// в случае потеряли объект
        {
            if (recipientHash == thisHash) { /*Debug.Log($"PlayerLosss {player.Hash} entyti={recipientHash}");*/ }
        }

        private void SlowdownDebuff()
        {
            if (modeMove == ModeMove.Line)
            {
                logicEntityExecutor.MinusSpeedPlayer(slowSpeedPercentage, slowdownTime);
            }
        }

        private void FaithDamageDebuff()
        {
            if (modeMove == ModeMove.Circle && timeCheckpoint + 5 <= Time.time)
            {
                timeCheckpoint = Time.time;
                templeExecutor.FaithDamage(faithDamage);
            }
        }

        void Update()
        {

        }
        //
        private void OnDisable()
        {
            scan.OnFindPlayer -= FindPlayer;
            scan.OnLossPlayer -= LossPlayer;
        }
    }
}
