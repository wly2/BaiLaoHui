local List               = require("XLuaLogic/Common/List")
require("XLuaLogic/Common/List")
require "XLuaLogic/Common/Define"
local util = require 'xlua.util'
local yield_return = (require 'cs_coroutine').yield_return
NiuNiuView = {}

local this = NiuNiuView;

local transform;
local gameObject;
local ShouPaiList ={};
local roomdata;
local playerDate={};
local playerList={};


function NiuNiuView.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	roomdata = NiuNiuCtrl.RoomInFo();
	this.InitView();
	print('NiuNiuView awake');
end

--初始化面板--
function NiuNiuView.InitView()
    this.pai=transform:FindChild("GameObject/Image");
    --[[this.roomID = transform:FindChild("bgTopLeft/Txt_1/txtRoomId"):GetComponent('UnityEngine.UI.Text');
	this.rommType = transform:FindChild("bgTopLeft/Txt_2/txtRoomId"):GetComponent('UnityEngine.UI.Text');
	this.rommBeiShu = transform:FindChild("bgTopLeft/Txt_3/txtRoomId"):GetComponent('UnityEngine.UI.Text');
	this.roomJuShu = transform:FindChild("bgTopLeft/Txt_4/txtRoomId"):GetComponent('UnityEngine.UI.Text');--]]
	
	local count=4;
	if count==3 then
		table.insert(playerList,transform:FindChild("RawImage/Player_"..1).gameObject);
		table.insert(playerList,transform:FindChild("RawImage/Player_"..3).gameObject);
		table.insert(playerList,transform:FindChild("RawImage/Player_"..6).gameObject);
		table.insert(ShouPaiList,transform:FindChild("ShouPai/"..1));
		table.insert(ShouPaiList,transform:FindChild("ShouPai/"..3));
		table.insert(ShouPaiList,transform:FindChild("ShouPai/"..6));
	elseif count==4 then
		table.insert(playerList,transform:FindChild("RawImage/Player_"..1).gameObject);
		table.insert(playerList,transform:FindChild("RawImage/Player_"..3).gameObject);
		table.insert(playerList,transform:FindChild("RawImage/Player_"..4).gameObject);
		table.insert(playerList,transform:FindChild("RawImage/Player_"..6).gameObject);
		table.insert(ShouPaiList,transform:FindChild("ShouPai/"..1));
		table.insert(ShouPaiList,transform:FindChild("ShouPai/"..3));
		table.insert(ShouPaiList,transform:FindChild("ShouPai/"..4));
		table.insert(ShouPaiList,transform:FindChild("ShouPai/"..6));
	elseif count==5 then
		table.insert(playerList,transform:FindChild("RawImage/Player_"..1).gameObject);
		table.insert(playerList,transform:FindChild("RawImage/Player_"..3).gameObject);
		table.insert(playerList,transform:FindChild("RawImage/Player_"..4).gameObject);
		table.insert(playerList,transform:FindChild("RawImage/Player_"..5).gameObject);
		table.insert(playerList,transform:FindChild("RawImage/Player_"..6).gameObject);
		table.insert(ShouPaiList,transform:FindChild("ShouPai/"..1));
		table.insert(ShouPaiList,transform:FindChild("ShouPai/"..3));
		table.insert(ShouPaiList,transform:FindChild("ShouPai/"..4));
		table.insert(ShouPaiList,transform:FindChild("ShouPai/"..5));
		table.insert(ShouPaiList,transform:FindChild("ShouPai/"..6));
	elseif count==6 then
		for idx=1,6 do
		table.insert(playerList,transform:FindChild("RawImage/Player_"..idx).gameObject);
		end
		for idx=1,6 do
		table.insert(ShouPaiList,transform:FindChild("ShouPai/"..idx));
		end
	else
		return ;
	end
	
end

function NiuNiuView.start()
	--[[this.roomID.text = roomdata.roomId;
	this.rommType.text = roomdata.gameMode;
	this.rommBeiShu.text = tostring(roomdata.limPlayer);
	this.roomJuShu.text= tostring(roomdata.PlayGameCount).."/"..roomdata.limtNumber;--]]
	for i=1,#playerList do
		playerList[i]:SetActive(true);
	end
	--for i=1,3 do
		--playerList[i].transform:FindChild("bg_Head/Head"):GetComponent('UnityEngine.UI.Image').sprite=data[i-1].headIcocn;
		--playerList[i].transform:FindChild("txtID"):GetComponent('UnityEngine.UI.Text').text=data[i-1].userID;
	--end
	local co = coroutine.create(function()
		ChuPai();
end)
	assert(coroutine.resume(co))
end

function ChuPai()
	local PaiImage= this.pai:GetComponent('UnityEngine.UI.Image');
	--print(ShouPaiList[1].name);
	local ShouPaiListcout=#ShouPaiList;
	for i=1 ,5 do
	   for j=1, ShouPaiListcout do
		   yield_return(CS.UnityEngine.WaitForSeconds(0.1))
	       PaiImage.transform:DOMove(ShouPaiList[j].transform:FindChild("Image"..i):GetComponent('UnityEngine.UI.Image').rectTransform.position,1);
		   yield_return(CS.UnityEngine.WaitForSeconds(1))
		   --PaiImage.transform:DOMove(Vector3(1,1,1),1);
		   ShouPaiList[j].transform:FindChild("Image"..i):GetComponent('UnityEngine.UI.Image').color=Color(1,1,1,1);
		   PaiImage.rectTransform.localPosition = Vector3.zero;
		if j==1 and i<5 then
			ShouPaiList[j].transform:FindChild("Image"..i):GetComponent('UnityEngine.UI.Image').transform:DORotate(Vector3(0, 0, 0), 1)
		end
	   end
	end
	  yield_return(CS.UnityEngine.WaitForSeconds(0.5))
	this.pai.gameObject:SetActive(false);
end
function NiuNiuView.update()
	
end
function NiuNiuView.ondestroy()
	print("NiuNiuView 窗口被销毁了");
end