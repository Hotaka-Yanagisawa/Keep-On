日本ゲーム大賞アマチュア部門2021に提出した作品です。

※作品に関する説明は「Keep On_作品紹介.pdf」、「Keep On_操作説明.pdf」をご確認ください。

使用ツール：Unity
開発期間：2021年2月～2021年5月 (4か月)
動作環境：Windows(XInputコントローラ必須)

苦労した点：モデルのアニメーション制御が初めてだったので苦労しました。
工夫した点：敵キャラクターを複数作る予定があったので敵キャラクター共通のステートを作成し、
	    他に人にも作業しやすいように設計しました。

制作箇所:
Enemy関連クラスのスクリプト、アニメーション制御や行動パターンのコーディングの作成
(雑魚敵共通の挙動、ピッケルを持った雑魚敵の挙動作成、その他雑魚敵の挙動改善)
BOSS関連クラス全てのスクリプト、アニメーション制御などの作成
ダメージ表示UIのスクリプト作成

アピールポイント：
■ゲーム部分
・敵キャラクターに索敵視野と攻撃可能視野を実装することで、思考して行動しているように作成しました。
・ボスがダウン状態の時に当たり判定の数を増やしました。通常時より多くダメージが入りやすくなり、
　ヒットストップが多く起きるのでプレイヤーに良い感触を与えるように制作しました。
■プログラム部分
・ボスの状態遷移を有限オートマトンで実装することで意図しない遷移が起こらないように実装しました。
・ステートとビヘイビアツリーを組み合わせてボスAIを複雑な処理を行えるように設計しました。

参考にしたもの
・3Dゲームをおもしろくする技術　実例から解き明かすゲームメカニクス・レベルデザイン・カメラのノウハウ Kindle版
https://miyagame.net/prefab/
https://tech.pjin.jp/blog/2016/02/16/unity_vector3_1/
https://nekopro99.com/move-rigidbody-addforce/#toc4
https://www.slideshare.net/sindharta/behaviour-treeingriffon?ref=http://developer.aiming-inc.com/study/griffon-behaviour-tree/
https://soramamenatan.hatenablog.com/entry/2020/06/28/162440
https://qiita.com/Hirai0827/items/c8bc643c0bcfe5ca9c17
https://xr-hub.com/archives/11515
https://docs.unity3d.com/ja/2018.4/Manual/ExecutionOrder.html
https://gametukurikata.com/program/onlyforwardsearch
http://kuromikangames.com/article/476445361.html#agenda-5vk64x__1-1
https://pafu-of-duck.hatenablog.com/entry/2017/11/02/182306
https://qiita.com/Egliss/items/3dd6fad0a7f183d47f45
https://www.youtube.com/watch?v=k-X4bqcc4Mg
https://unity-shoshinsha.biz/archives/987
https://gametukurikata.com/program/charaattackenemy
https://qiita.com/Armyporoco/items/391776d4c79d25cfbbfe
https://ftvoid.com/blog/post/752
https://qiita.com/nkjzm/items/5994a31bcabfd06e9593
https://tsubakit1.hateblo.jp/entry/2016/03/01/020510
https://yttm-work.jp/unity/unity_0018.html
https://qiita.com/toRisouP/items/b6540b7f514d18b9a426
https://techblog.kayac.com/unity_advent_calendar_2016_15
https://light11.hatenadiary.com/entry/2019/04/18/003100
https://www.youtube.com/watch?v=DJngVUD66K0
https://www.youtube.com/watch?v=PbtJt5tnnI8









