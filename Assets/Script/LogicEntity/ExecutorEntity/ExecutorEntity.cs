using Registrator;
using TemleLogic;
using UnityEngine;
using Zenject;

namespace EntityLogic
{
    public class ExecutorEntity : MonoBehaviour
    {
        private Construction player;//��� ������ ��� ������� ��� �����, ���� �������� ����� ��������� � ���� �������� ��� �� ��������..
        //����� � ���� ����� ������� �������, ����� �������� player �� ������������ �������
        
        
        [SerializeField] private ModeMove modeMove;
        [Header("������� ���������� ������")]
        [SerializeField] private float slowSpeedPercentage;
        [Header("����� ���������� ������")]
        [SerializeField] private float slowdownTime;
        [Header("���� ����")]
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
            //������ ����������, �� �����������  ����� ����� �����������, ���� �� ������� ���������, ������ ��������, ����� ������))
            //�� � �������� �� �� ����� ��������, ������ ������ ������ �� ������
            if (modeMove == ModeMove.Line) { }//������ ������� ������������ �� ����� � �����
            if (modeMove == ModeMove.Circle) { }//������ ������� ������������ �� ����������
            timeCheckpoint = Time.time;
        }
        private void FindPlayer(Construction _player, int recipientHash)//� ������ ����� ������
        {
            player = _player;
            if (recipientHash == thisHash) 
            {
                /*Debug.Log($"PlayerFind {player.Hash} entyti={recipientHash}"); */
                SlowdownDebuff();
                FaithDamageDebuff();
            }
            //logicEntityExecutor.MinusSpeedPlayer(50f,3f);//�������� ���� (�������,�����), ����� ��� ������� ��������
        }
        private void LossPlayer(int recipientHash)// � ������ �������� ������
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
