
ShiSanShuiCtrl = {};

local this = ShiSanShuiCtrl;

local root;
local transform;
local gameObject;


function ShiSanShuiCtrl.New()
	return this;
end

function ShiSanShuiCtrl.Awake()
	print('shishanshui������ ������');
	CS.LuaHelper.ViewUtil:OpenWindow("ShiSanShuiView", this.OnCreate);
end

--�����¼�--
function ShiSanShuiCtrl.OnCreate()
	--gameObject = obj;
	print("������ShiSanShui�ص�");
	
end
--���ط�����Ϣ--
function ShiSanShuiCtrl.RoomInFo()
	return  CS.LuaHelper.DataResourMgr:RoomData();
end
--���������Ϣ--
function ShiSanShuiCtrl.PlayInFo()
	return  CS.LuaHelper.DataResourMgr:PlayerInfo();
end

--�����¼�--
function ShiSanShuiCtrl.OpenMessageClick()
	print("����˴���Ϣ��ť");
	
end