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

        // rootノード
        SequencerNode rootNode = new SequencerNode();
        rootNode.name = "rootノード";

        // 椅子から立つ
        ActionNode standChair = new ActionNode();
        standChair.name = "椅子から立つActionNode";
        standChair.SetRunningFunc(() => {
            Debug.LogError("椅子から立つ");
            return NodeStatus.SUCCESS;
        });

        // 所持金が200円以上あるか確認
        DecoratorNode confirmMoney = new DecoratorNode();
        confirmMoney.name = "所持金が200円以上あるか確認するDecoratorNode";
        confirmMoney.SetConditionFunc(() => {
            return _money >= DRINK_PRICE ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        });

        // 自販機に移動する
        ActionNode moveVendingMachine = new ActionNode();
        moveVendingMachine.name = "自販機に移動するActionNode";
        moveVendingMachine.SetRunningFunc(() => {
            Debug.LogError("自販機に移動する");
            return NodeStatus.SUCCESS;
        });

        // 所持金が200円以上あるか確認するDecoratorNodeの子供登録
        confirmMoney.AddChild(moveVendingMachine);

        // 購入するものをランダムに決める
        SelectorNode rondomPurchase = new SelectorNode();
        rondomPurchase.name = "購入するものをランダムに決めるSelectorNode";

        // 麦茶を買ったか確認する
        DecoratorNode confirmBuyTea = new DecoratorNode();
        confirmBuyTea.name = "麦茶を買ったか確認するDecoratorNode";
        confirmBuyTea.SetConditionFunc(() => {
            int random = Random.Range(0, 100);
            return PROBABILITY > random ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        });

        // 麦茶を買う
        ActionNode buyTea = new ActionNode();
        buyTea.name = "麦茶を買うActionNode";
        buyTea.SetRunningFunc(() => {
            Debug.LogError("麦茶を買う");
            return NodeStatus.SUCCESS;
        });

        // 麦茶を買ったか確認するDecoratorNodeの子供登録
        confirmBuyTea.AddChild(buyTea);

        // 水を買う
        ActionNode buyWater = new ActionNode();
        buyWater.name = "水を買うActionNode";
        buyWater.SetRunningFunc(() => {
            Debug.LogError("水を買う");
            return NodeStatus.SUCCESS;
        });

        // 購入するものをランダムに決めるSelectorNodeの子供登録
        rondomPurchase.AddChild(confirmBuyTea);
        rondomPurchase.AddChild(buyWater);

        // 自席に移動する
        ActionNode moveSeat = new ActionNode();
        moveSeat.name = "自席に移動するActionNode";
        moveSeat.SetRunningFunc(() => {
            Debug.LogError("自席に移動する");
            return NodeStatus.SUCCESS;
        });

        // 椅子に座る
        ActionNode sitChair = new ActionNode();
        sitChair.name = "椅子に座るActionNode";
        sitChair.SetRunningFunc(() => {
            Debug.LogError("椅子に座る");
            return NodeStatus.SUCCESS;
        });

        // rootノードの子供登録
        rootNode.AddChild(standChair);
        rootNode.AddChild(confirmMoney);
        rootNode.AddChild(rondomPurchase);
        rootNode.AddChild(moveSeat);
        rootNode.AddChild(sitChair);

        // ツリー実行
        _behaviorTreeController.Initialize(rootNode);
        _behaviorTreeController.OnStart();
    }

    void Update()
    {
        _behaviorTreeController.OnRunning();
    }
}