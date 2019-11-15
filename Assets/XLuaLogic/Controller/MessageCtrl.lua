require "XLuaLogic/Data/TaskEntity"
require "XLuaLogic/Common/Define"

MessageCtrl = {};
local this = MessageCtrl;

function MessageCtrl.New()
	return this;
end

function MessageCtrl.Awake()
	print('执行MessageCtrl 的Awake 方法');
	
	CS.LuaHelper.ViewUtil:OpenWindow("MessageView", this.OnCreate);
end

--启动事件--
function MessageCtrl.OnCreate()
	txtTitle = MessageView.txtTitle:GetComponent('UnityEngine.UI.Text');
end


function MessageCtrl.OnStart()
	print("进入了OnStart回调");
	
	--定义列表
	local taskTable ={};
	
	taskTable[#taskTable+1] = TaskEntity.New(1001, "小试身手", 0, "我看少侠骨骼惊奇，天赋异禀，实在是个修仙的好料子，快去帮我去狼群杀死一些狼吧！");
	taskTable[#taskTable+1] = TaskEntity.New(1002, "武器考验", 1, "好徒儿，现在的你仍然菜的很，这样吧，你去岐山西郊之地帮我对付几只小牛");
	taskTable[#taskTable+1] = TaskEntity.New(1003, "技能传授", 2, "玄兵斗法，阵列纲常。万法无极，心法合一！");
	taskTable[#taskTable+1] = TaskEntity.New(1004, "修为精进", 0, "这事来的巧。前几日西郊频现异变蛇妖，这些妖蛇在附近村庄作祟。我派你前往除妖卫道，你可愿意？");
	taskTable[#taskTable+1] = TaskEntity.New(1005, "强化武器", 1, "徒儿，你可知磨刀不误砍柴工这个道理？你天赋异于常人，但仍需让你的武器和装备时时保持贴己趁手！");
	taskTable[#taskTable+1] = TaskEntity.New(1006, "困难挑战", 2, "师弟的能力提升之后，那些简单的关卡已经不再适合你了，你可以试试更高难度的关卡，奖励上也会更加丰富！");
	taskTable[#taskTable+1] = TaskEntity.New(1007, "护身项链", 0, "好徒儿，我为你准备了一样礼物，但是呢…！");
	taskTable[#taskTable+1] = TaskEntity.New(1008, "师门考验", 1, "既然装备【进阶】一层了，快去试试效果如何！");
	taskTable[#taskTable+1] = TaskEntity.New(1009, "打扫后山", 2, "方才得到师父的命令需要我前去击杀一些魔怪，但我目前还有其他要事，师弟能否代我前去一探！");
	taskTable[#taskTable+1] = TaskEntity.New(1010, "刺探", 0, "我国需要一些有志之士，去收集一些情报，我们才能指定合理的发展战略！");
	
	
	--[[for i=1, 10, 1 do
		local id = i;
		local task = TaskEntity.New(id, "任务名称"..id.."A", (((i%2) == 0) and 0 )or 1, "任务描述"..id);
		
		taskTable[#taskTable+1] = task; --在末尾追加数据
		
		print("追加了数据"..id);
	end--]]
	
	--循环列表
	for i=1, #taskTable, 1 do
		
		
		local j=i; --循环委托注意事项 此处一定要重新声明 local j=i
		local task = taskTable[j]; --然后重新定义task 从表中获取数据
		
		--克隆预设
		local obj = CS.LuaHelper.ResourcesMgr:Load("UIPrefab/UIWindows/TaskItemView");
		obj.transform.parent = MessageView.content;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale=Vector3.one;
		
		
		local txtTaskTitle = obj.transform:FindChild("txtTitle"):GetComponent('UnityEngine.UI.Text');
		txtTaskTitle.text = task.Title;
		print("数据Title"..task.Title);
		
		local txtTaskStatus = obj.transform:FindChild("txtStatus"):GetComponent('UnityEngine.UI.Text');
		txtTaskStatus.text = this.GetTaskStatusName(task.Status);
		
		local btnItem = obj.transform:GetComponent('UnityEngine.UI.Button');

		--写法
		btnItem.onClick:AddListener(
			function ()
				MessageView.detailContainer.gameObject:SetActive(true);
				MessageView.txtTaskId.text = tostring(task.Id);
				MessageView.txtTaskName.text = task.Title;
				MessageView.txtTaskStatus.text = this.GetTaskStatusName(task.Status);
				MessageView.txtTaskContent.text = "";
				MessageView.txtTaskContent:DOText(task.Content, 0.5);
			end
		);
	end
	
end



function MessageCtrl.GetTaskStatusName(status)
	
	if (status == 0)
	then
		return "<color=#06C300FF>未接</color>";
	elseif (status == 1)
	then
		return "<color=#00CAFFFF>已接</color>";
	elseif (status == 2)
	then
		return "<color=#FFAE00FF>已完成</color>";
	end
end

function MessageCtrl.btnOKClick()
	txtTitle.text = "修改了标题";
end