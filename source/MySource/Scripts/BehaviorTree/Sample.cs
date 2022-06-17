using UnityEngine;

public class Sample : MonoBehaviour
{

    [SerializeField]
    private int _money;
    private BehaviorTreeController _behaviorTreeController;
    private const int DRINK_PRICE = 200;
    private const int PROBABILITY = 50;
    void Start()
    {
        _behaviorTreeController = new BehaviorTreeController();

        // root�m�[�h
        SequencerNode rootNode = new SequencerNode();
        rootNode.name = "root�m�[�h";

        // �֎q���痧��
        ActionNode standChair = new ActionNode();
        standChair.name = "�֎q���痧��ActionNode";
        standChair.SetRunningFunc(() => {
            Debug.LogError("�֎q���痧��");
            return NodeStatus.SUCCESS;
        });

        // ��������200�~�ȏ゠�邩�m�F
        DecoratorNode confirmMoney = new DecoratorNode();
        confirmMoney.name = "��������200�~�ȏ゠�邩�m�F����DecoratorNode";
        confirmMoney.SetConditionFunc(() => {
            return _money >= DRINK_PRICE ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        });

        // ���̋@�Ɉړ�����
        ActionNode moveVendingMachine = new ActionNode();
        moveVendingMachine.name = "���̋@�Ɉړ�����ActionNode";
        moveVendingMachine.SetRunningFunc(() => {
            Debug.LogError("���̋@�Ɉړ�����");
            return NodeStatus.SUCCESS;
        });

        // ��������200�~�ȏ゠�邩�m�F����DecoratorNode�̎q���o�^
        confirmMoney.AddChild(moveVendingMachine);

        // �w��������̂������_���Ɍ��߂�
        SelectorNode rondomPurchase = new SelectorNode();
        rondomPurchase.name = "�w��������̂������_���Ɍ��߂�SelectorNode";

        // �����𔃂������m�F����
        DecoratorNode confirmBuyTea = new DecoratorNode();
        confirmBuyTea.name = "�����𔃂������m�F����DecoratorNode";
        confirmBuyTea.SetConditionFunc(() => {
            int random = Random.Range(0, 100);
            return PROBABILITY > random ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        });

        // �����𔃂�
        ActionNode buyTea = new ActionNode();
        buyTea.name = "�����𔃂�ActionNode";
        buyTea.SetRunningFunc(() => {
            Debug.LogError("�����𔃂�");
            return NodeStatus.SUCCESS;
        });

        // �����𔃂������m�F����DecoratorNode�̎q���o�^
        confirmBuyTea.AddChild(buyTea);

        // ���𔃂�
        ActionNode buyWater = new ActionNode();
        buyWater.name = "���𔃂�ActionNode";
        buyWater.SetRunningFunc(() => {
            Debug.LogError("���𔃂�");
            return NodeStatus.SUCCESS;
        });

        // �w��������̂������_���Ɍ��߂�SelectorNode�̎q���o�^
        rondomPurchase.AddChild(confirmBuyTea);
        rondomPurchase.AddChild(buyWater);

        // ���ȂɈړ�����
        ActionNode moveSeat = new ActionNode();
        moveSeat.name = "���ȂɈړ�����ActionNode";
        moveSeat.SetRunningFunc(() => {
            Debug.LogError("���ȂɈړ�����");
            return NodeStatus.SUCCESS;
        });

        // �֎q�ɍ���
        ActionNode sitChair = new ActionNode();
        sitChair.name = "�֎q�ɍ���ActionNode";
        sitChair.SetRunningFunc(() => {
            Debug.LogError("�֎q�ɍ���");
            return NodeStatus.SUCCESS;
        });

        // root�m�[�h�̎q���o�^
        rootNode.AddChild(standChair);
        rootNode.AddChild(confirmMoney);
        rootNode.AddChild(rondomPurchase);
        rootNode.AddChild(moveSeat);
        rootNode.AddChild(sitChair);

        // �c���[���s
        _behaviorTreeController.Initialize(rootNode);
        _behaviorTreeController.OnStart();
    }

    void Update()
    {
        _behaviorTreeController.OnRunning();
    }
}