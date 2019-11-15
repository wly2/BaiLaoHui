UIRootView = {}
local this = UIRootView;

local transform;
local gameObject;

function UIRootView.awake(obj)
	gameObject = obj;
	transform = obj.transform;
	
	this.InitView();
	print('UIRootView awake');
end

--初始化面板--
function UIRootView.InitView()
	this.btnOpenMessage = transform:FindChild("Canvas/ContainerBottomRight/btnOpenMessage");
end

function UIRootView.start()
	print('UIRootView start');
end

function UIRootView.update()
end

function UIRootView.ondestroy()
end