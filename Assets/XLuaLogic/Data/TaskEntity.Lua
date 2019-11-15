
--编号 标题 状态 描述内容
TaskEntity =  {Id = 0, Title = "", Status=0, Content=""}

--这句是重定义元表的索引，就是说有了这句，这个才是一个类。
TaskEntity.__index = TaskEntity

--构造体，构造体的名字是随便起的，习惯性改为New()
function TaskEntity.New(Id, Title, Status, Content) 
    local self = {};    --初始化self，如果没有这句，那么类所建立的对象改变，其他对象都会改变
    setmetatable(self, TaskEntity);  --将self的元表设定为Class
    self.Id = Id;
    self.Title = Title;
	self.Status = Status;
	self.Content = Content;
    return self;    --返回自身
end