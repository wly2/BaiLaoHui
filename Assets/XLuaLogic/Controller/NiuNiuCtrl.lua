
NiuNiuCtrl = {};

local this = NiuNiuCtrl;

local root;
local transform;
local gameObject;
local NiuNiuplayerinfo={};


function NiuNiuCtrl.New()
	return this;
end

function NiuNiuCtrl.Awake()
	print('主界面 启动了');
	CS.LuaHelper.ViewUtil:OpenWindow("NiuNiuView", this.OnCreate);
end

--启动事件--
function NiuNiuCtrl.OnCreate()
	--gameObject = obj;
	print("进入了NIUNIU回调");
	
end
--返回房间信息--
function NiuNiuCtrl.RoomInFo()
	return  CS.LuaHelper.DataResourMgr:RoomData();
end
--返回玩家信息
function NiuNiuCtrl.PlayInFo()
	return  CS.LuaHelper.DataResourMgr:PlayerInfo();
end

--单击事件--
function NiuNiuCtrl.OpenMessageClick()
	print("点击了打开消息按钮");
	
end