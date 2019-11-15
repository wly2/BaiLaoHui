
ShiSanShuiCtrl = {};

local this = ShiSanShuiCtrl;

local root;
local transform;
local gameObject;


function ShiSanShuiCtrl.New()
	return this;
end

function ShiSanShuiCtrl.Awake()
	print('shishanshui主界面 启动了');
	CS.LuaHelper.ViewUtil:OpenWindow("ShiSanShuiView", this.OnCreate);
end

--启动事件--
function ShiSanShuiCtrl.OnCreate()
	--gameObject = obj;
	print("进入了ShiSanShui回调");
	
end
--返回房间信息--
function ShiSanShuiCtrl.RoomInFo()
	return  CS.LuaHelper.DataResourMgr:RoomData();
end
--返回玩家信息--
function ShiSanShuiCtrl.PlayInFo()
	return  CS.LuaHelper.DataResourMgr:PlayerInfo();
end

--单击事件--
function ShiSanShuiCtrl.OpenMessageClick()
	print("点击了打开消息按钮");
	
end