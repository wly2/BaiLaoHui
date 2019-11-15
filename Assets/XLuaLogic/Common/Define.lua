print('启动了Define.lua')

CtrlNames={
	MessageCtrl = "MessageCtrl",
	UIRootCtrl = "UIRootCtrl",
	NiuNiuCtrl = "NiuNiuCtrl",
	ShiSanShuiCtrl = "ShiSanShuiCtrl";
}

ViewNames={
	"UIRootView",
	"MessageView",
	"NiuNiuView",
	"ShiSanShuiView"
}

--这里要把常用的引擎类型都加入进来
WWW = CS.UnityEngine.WWW;
GameObject = CS.UnityEngine.GameObject;
Color = CS.UnityEngine.Color;
Vector3 = CS.UnityEngine.Vector3;
local color = Color(1,1,1,1);