
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
	print('������ ������');
	CS.LuaHelper.ViewUtil:OpenWindow("NiuNiuView", this.OnCreate);
end

--�����¼�--
function NiuNiuCtrl.OnCreate()
	--gameObject = obj;
	print("������NIUNIU�ص�");
	
end
--���ط�����Ϣ--
function NiuNiuCtrl.RoomInFo()
	return  CS.LuaHelper.DataResourMgr:RoomData();
end
--���������Ϣ
function NiuNiuCtrl.PlayInFo()
	return  CS.LuaHelper.DataResourMgr:PlayerInfo();
end

--�����¼�--
function NiuNiuCtrl.OpenMessageClick()
	print("����˴���Ϣ��ť");
	
end