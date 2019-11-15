
UIRootCtrl = {};

local this = UIRootCtrl;

local root;
local transform;
local gameObject;


function UIRootCtrl.New()
	return this;
end

function UIRootCtrl.Awake()
	print('主界面 启动了');
	CS.LuaHelper.ViewUtil:LoadUIRoot(this.OnCreate);
end

--启动事件--
function UIRootCtrl.OnCreate()
	--gameObject = obj;

	print("进入了回调");
	
	btnOpen = UIRootView.btnOpenMessage:GetComponent('UnityEngine.UI.Button');
    btnOpen.onClick:AddListener(this.OpenMessageClick);
end

--单击事件--
function UIRootCtrl.OpenMessageClick()
	print("点击了打开消息按钮");
	
	GameInit.LoadView(CtrlNames.NiuNiuCtrl);
end