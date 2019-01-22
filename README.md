# AnimationPlayer
Simple Animation Player for Unity

![AnimationPlayer](https://docs.google.com/uc?id=1l45FF-6tX077jELec0Yz1RluG1u0PgxI)

## About AnimationPlayer

> AnimationPlayerは[SimpleAnimation](https://github.com/Unity-Technologies/SimpleAnimation)がアップデートされなくなったため  
作った簡単なアニメーションプレイヤーです。  
基本的にSimpleAnimationをベースにしましたが、設計から作り直したものです。  
SimpleAnimationはPlayableクラスの中にStateが存在して、外からの変更をそのStateに同期させる構造ですが、  
それはスクリプト上で内部のプレイ状態を得るたびにGCが発生するので、Stateを共通にした仕組みに変わりました。  
AnimationPlayer is a simple animation player since Simpleanimation is no longer updating.  
AnimationPlayer is based on Simpleanimation, but I rebuild from the design.

## Installing AnimationPlayer

> Pathに依存しないのでどこでもいいですが、個人的にはこのパスを使います。  
It doesn`t depend on path, so anywhere is fine, but I use next path personally

    Assets/Plugins/AnimationPlayer

## Using AnimationPlayer

> Animatorが既に設定されている場合はAnimatorのControllerをNoneに設定します。  
持っていない場合はAnimationPlayerコンポーネントを追加したら自動で追加されます。  
プレイヤーは２種類あります。  
if Animator is added already, Set controller to none,  
otherwise adding this component will automatically add Animator component.  
There are 2 available components.

* AnimationPlayerSimple

    単一アニメーションのみ再生する場合使います。  
State管理しない分ちょっとだけ軽くなります。  
Use this component for single animation clip.

* AnimationPlayer

    一つ以上のアニメーションを設定するかプレイ中追加するならこのコンポーネントを使います。  
ランタイム中のPlay、CrossFade、Blendなどの制御が可能で各アニメーションに  
終了コールバックを設定することもできます。  
Use this component for more than one animation clip.  
Can controll Play, CrossFade, Blend, registering end callback by script on runtime.  

** PlayableAPIを使って実装したのでUnity 2017.1以上で動作します。  
[PlayableGraph Visualizer](https://github.com/Unity-Technologies/graph-visualizer)も一緒に使うとデバッグが楽になります。
