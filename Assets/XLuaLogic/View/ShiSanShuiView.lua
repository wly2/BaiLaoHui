local List               = require("XLuaLogic/Common/List")
require("XLuaLogic/Common/List")
require "XLuaLogic/Common/Define"
local util = require 'xlua.util'
local yield_return = (require 'cs_coroutine').yield_return
ShiSanShuiView = {}

local this = ShiSanShuiView;

local transform;
local gameObject
local ShouPaiList ={};
local roomdata;
local playerDate={};
local playerList={};


function ShiSanShuiView.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	roomdata = ShiSanShuiCtrl.RoomInFo()
	
	this.InitView();
	print('ShiSanShuiView awake');
end

--初始化面板--
function ShiSanShuiView.InitView()
   
    this.roomID = transform:FindChild("TopLeft/txtRoomNumber/txtRoomNumber"):GetComponent('UnityEngine.UI.Text');
	this.rommCost = transform:FindChild("TopLeft/txtRoomCost/txtRoomCost"):GetComponent('UnityEngine.UI.Text');
	this.rommPeoPel = transform:FindChild("TopLeft/txtPeople/txtPeople"):GetComponent('UnityEngine.UI.Text');
	this.roomJuShu = transform:FindChild("TopLeft/txtJushu/txtJushu"):GetComponent('UnityEngine.UI.Text');
	this.MaCard = transform:FindChild("MaCard"):GetComponent('UnityEngine.UI.Image');
	btnReady = transform:FindChild("Centre/butReady"):GetComponent('UnityEngine.UI.Button');
	btnReady.onClick:AddListener(this.SendReady);
	ShiSanShuiView.RefrshPlayerList( 3 )
	
end

function ShiSanShuiView.start()
	this.roomID.text = roomdata.roomId;
	if roomdata.payType==0 then
		this.rommCost.text="免费";
	elseif roomdata.payType==1 then
		this.rommCost.text="AA支付";
	else
		this.rommCost.text="房主付费";
	end
	this.rommPeoPel.text = tostring(roomdata.playerNum);
	this.roomJuShu.text = tostring(roomdata.PlayGameCount).."/"..roomdata.limtNumber;
	if roomdata.maPaiId > 0then
		this.MaCard.sprite = roomdata.maPaiSprite;
	else
		this.MaCard.gameObject:SetActive(false);
	end
	for i=1,#playerList do
		playerList[i]:SetActive(true);
	end
	--[[for i=1,3 do
		playerList[i].transform:FindChild("bg_Head/Head"):GetComponent('UnityEngine.UI.Image').sprite=data[i-1].headIcocn;
		playerList[i].transform:FindChild("txtID"):GetComponent('UnityEngine.UI.Text').text=data[i-1].userID;
	end
	local co = coroutine.create(function()
		ChuPai();
end)
	assert(coroutine.resume(co))--]]
end

function ChuPai()
	--[[local PaiImage= this.pai:GetComponent('UnityEngine.UI.Image');
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
	this.pai.gameObject:SetActive(false);--]]
end
function ShiSanShuiView.update()
	
end
function ShiSanShuiView.RefrshPlayerList( count )
	if count==3 then
		table.insert(playerList,transform:FindChild("Players/Player_"..1).gameObject);
		table.insert(playerList,transform:FindChild("Players/Player_"..3).gameObject);
		table.insert(playerList,transform:FindChild("Players/Player_"..6).gameObject);
		--[[table.insert(ShouPaiList,transform:FindChild("Players/Player_1/Cards"));
		table.insert(ShouPaiList,transform:FindChild("Players/Player_3/Cards"));
		table.insert(ShouPaiList,transform:FindChild("Players/Player_6/Cards"));--]]
	elseif count==1 then
		table.insert(playerList,transform:FindChild("Players/Player_"..1).gameObject);
		--table.insert(ShouPaiList,transform:FindChild("Players/Player_1/Cards"));
	elseif count==2 then
		table.insert(playerList,transform:FindChild("Players/Player_"..1).gameObject);
		table.insert(playerList,transform:FindChild("Players/Player_"..3).gameObject);
		--[[table.insert(ShouPaiList,transform:FindChild("Players/Player_1/Cards"));
		table.insert(ShouPaiList,transform:FindChild("Players/Player_3/Cards"));--]]
	elseif count==4 then
		table.insert(playerList,transform:FindChild("Players/Player_"..1).gameObject);
		table.insert(playerList,transform:FindChild("Players/Player_"..3).gameObject);
		table.insert(playerList,transform:FindChild("Players/Player_"..4).gameObject);
		table.insert(playerList,transform:FindChild("Players/Player_"..6).gameObject);
		--[[table.insert(ShouPaiList,transform:FindChild("Players/Player_1/Cards"));
		table.insert(ShouPaiList,transform:FindChild("Players/Player_3/Cards"));
		table.insert(ShouPaiList,transform:FindChild("Players/Player_4/Cards"));
		table.insert(ShouPaiList,transform:FindChild("Players/Player_6/Cards"));--]]
	elseif count==5 then
		table.insert(playerList,transform:FindChild("Players/Player_"..1).gameObject);
		table.insert(playerList,transform:FindChild("Players/Player_"..3).gameObject);
		table.insert(playerList,transform:FindChild("Players/Player_"..4).gameObject);
		table.insert(playerList,transform:FindChild("Players/Player_"..5).gameObject);
		table.insert(playerList,transform:FindChild("Players/Player_"..6).gameObject);
		--[[table.insert(ShouPaiList,transform:FindChild("Players/Player_1/Cards"));
		table.insert(ShouPaiList,transform:FindChild("Players/Player_3/Cards"));
		table.insert(ShouPaiList,transform:FindChild("Players/Player_4/Cards"));
		table.insert(ShouPaiList,transform:FindChild("Players/Player_5/Cards"));
		table.insert(ShouPaiList,transform:FindChild("Players/Player_6/Cards"));--]]
	elseif count==6 then
		for idx=1,6 do
		table.insert(playerList,transform:FindChild("Players/Player_"..idx).gameObject);
		end
		--for idx=1,6 do
			--table.insert(ShouPaiList,transform:FindChild("Players/Player_"..idx.."/"));
		--end
	else
		return ;
	end
end
function ShiSanShuiView.SendReady()
	CS.LuaHelper.DataResourMgr:Readly();
end
function ShiSanShuiView.ondestroy()
	print("ShiSanShuiView 窗口被销毁了");
end