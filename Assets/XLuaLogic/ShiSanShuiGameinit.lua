print('Æô¶¯GameInit.lua')

require "XLuaLogic/CtrlManager"

ShiSanShuiGameinit = {};
local this = ShiSanShuiGameinit;

function ShiSanShuiGameinit.InitViews()
	local self={};
	for i=1, #ViewNames do
		require('XLuaLogic/View/'..tostring(ViewNames[i]));
	end
	
end


function ShiSanShuiGameinit.Init()
	this.InitViews();
	CtrlManager.Init();
	ShiSanShuiGameinit.LoadView(CtrlNames.ShiSanShuiCtrl);
end

function ShiSanShuiGameinit.LoadView(type)
	local ctrl = CtrlManager.GetCtrl(type);
	if ctrl ~= nil then
		ctrl.Awake();
	end
end