MessageView = {}
local this = MessageView;

local transform;
local gameObject;



function MessageView.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	
	this.InitView();
	print('MessageView awake');
end

--初始化面板--
function MessageView.InitView()
	
	--先把需要用到的组件 找到
	this.txtTitle = transform:FindChild("imgTitleBG/txtTitle");
	this.content = transform:FindChild("Scroll View/Viewport/Content");
	
	this.detailContainer = transform:FindChild("detailContainer");
	
	this.detailContainer.gameObject:SetActive(false);
	
	this.txtTaskId = transform:FindChild("detailContainer/txtTaskId"):GetComponent('UnityEngine.UI.Text');
	this.txtTaskName = transform:FindChild("detailContainer/txtTaskName"):GetComponent('UnityEngine.UI.Text');
	this.txtTaskStatus = transform:FindChild("detailContainer/txtTaskStatus"):GetComponent('UnityEngine.UI.Text');
	this.txtTaskContent = transform:FindChild("detailContainer/txtTaskContent"):GetComponent('UnityEngine.UI.Text');
	
end

function MessageView.start()
	MessageCtrl.OnStart();
end

function MessageView.update()
end

function MessageView.ondestroy()
	print("MessageView 窗口被销毁了");
end