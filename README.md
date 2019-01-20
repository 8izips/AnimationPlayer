# AnimationPlayer
Simple Animation Player for Unity

![AnimationPlayer](https://docs.google.com/uc?id=1l45FF-6tX077jELec0Yz1RluG1u0PgxI)

## About AnimationPlayer

> AnimationPlayerは[SimpleAnimation](https://github.com/Unity-Technologies/SimpleAnimation)がアップデートされなくなったため  
作った簡単なアニメーションプレイヤーです。  
基本的にSimpleAnimationをベースにしましたが、設計から作り直したものです。  
SimpleAnimationはEditor上で編集したStateをPlayableに同期させる構造ですが、  
スクリプト上で内部のプレイ状態を得るたびにGCが発生するので、  
Stateを共通にした仕組みに変わりました。

## Installing AnimationPlayer

> 内部でPathを使わコードはないのでどこでもいいですが個人的にはこのパスを使います。

    Assets/Plugins/AnimationPlayer

## Using AnimationPlayer

> Animatorを持っている場合AnimatorをNoneに設定します。  
持っていない場合はAnimationPlayerコンポーネントを追加したら自動で追加されます。  
プレイヤーは２種類あります。

* AnimationPlayerSimple

    単一アニメーションのみ再生する場合使います。  
State管理しない分ちょっとだけ軽くなります。

* AnimationPlayer

    一つ以上のアニメーションを設定するかプレイ中追加するならこのコンポーネントを使います。  
ランタイム中のPlay、CrossFade、Blendなどの制御が可能で各アニメーションに  
終了コールバックを設定することもできます。


** PlayableAPIを使って実装したのでUnity 2017.1以上で動作します。  
[PlayableGraph Visualizer](https://github.com/Unity-Technologies/graph-visualizer)も一緒に使うとデバッグが楽になります。
