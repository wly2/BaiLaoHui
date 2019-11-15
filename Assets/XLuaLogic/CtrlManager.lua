--Lua控制器的管理器 作用就是注册所有的控制器

print('启动CtrlManager.lua')

require "XLuaLogic/Common/Define"

require "XLuaLogic/Controller/MessageCtrl"
require "XLuaLogic/Controller/UIRootCtrl"
require "XLuaLogic/Controller/NiuNiuCtrl"
require "XLuaLogic/Controller/ShiSanShuiCtrl"

CtrlManager = {};

local this = CtrlManager;

--控制器列表
local ctrlList = {};

--初始化 往列表中添加所有的控制器
function CtrlManager.Init()
	ctrlList[CtrlNames.MessageCtrl] = MessageCtrl.New();
	ctrlList[CtrlNames.UIRootCtrl] = UIRootCtrl.New();
	ctrlList[CtrlNames.NiuNiuCtrl] = NiuNiuCtrl.New();
	ctrlList[CtrlNames.ShiSanShuiCtrl] = ShiSanShuiCtrl.New();
	
	return this;
end

--根据控制器的名称 获取控制器
function CtrlManager.GetCtrl(ctrlName)
	return ctrlList[ctrlName];
end