# KCore
KCore Lib

**注意,暂不再更新**
**注意,暂不再更新**
**注意,暂不再更新**
**UI生成的代码有缺憾,暂不再更新这个**

基于Unity 5.3 Persional Version的简易游戏开发库

小目标: 游戏原型快速开发

**注意: 这不是Unity工程**

## 如何使用

1. 安装git (windows下安装git)

2. 使用git bash: $ git clone https://github.com/beckheng/GameCreator

3. $ cd GameCreator

4. $ perl scripts/project_create.pl "D:\MyDemo" BBTowerDefense

5. 使用Unity打开工程: D:\MyDemo\BBTowerDefense\BBTowerDefense_Client

## 如何开始Demo

1. 在菜单执行: 游戏开发设定 先定一个基准

2. 新建场景: SplashScene

3. 在Scenes in build添加SplashScene为首个场景

4. 新建View-Window: LoadingProgressWindow, 添加UI相关内容, 为有需要的控制添加KUIPropertyDefine组件,每个View-Window都自动带了一个名为CodeGenDummy的GameObject.确保总是能生成窗体代码.

5. 将LoadingProgressWindow存为预制,根目录为Prefabs/View

6. 执行生成窗体代码,生成LoadingProgressWindow相关的代码

7. 将LoadingProgressWindow生成为AssetBundle

8. 在SplashSceneManager.LoadData方法添加逻辑代码: 

	StartCoroutine(KAssetBundle.LoadPersistentAB(new string[] { KAssetBundle.GetViewPath(typeof(LoadingProgressWindow).Name) }, onSucc));
	
	其中onSucc: 
		LoadingProgressWindow loadingProgressWindow = KAssetBundle.InstantiateView<LoadingProgressWindow>();
		loadingProgressWindow.SetContent(null);

9. 运行菜单: 从首场景开始游戏

## 常用的API/变量介绍

1. KAssetBundle.LoadPersistentAB 异步加载AB,加载完成后,执行onSucc回调

2. KAssetBundle.InstantiateView 从AB实例化一个View

3. KAssetBundle.GetViewPath

4. KAssetBundle.GetEffectPath

5. KAssetBundle.GetSoundPath

6. KAssetBundle.GetScenePath 这个方法的场景名称使用小写没有问题, 但是要 **特别特别特别** 注意使用SceneManager.LoadSceneAsync的场景名字,是要保持和场景文件一致的

7. KSceneManager.SwitchScene

8. KConfigLoader.allConfigLoaded 表示配置表是否加载完成

## 使用Unity开发的一些小建议

1. 资源要有唯一的名字

2. 菜单的快捷键,要避免与Unity本身的冲突

3. EDITOR中的游戏行为,与真机上保持一致,避免浪费时间在真机上调试(原生相关的行为除外)

## 感谢

1. https://github.com/NtreevSoft/psd-parser
