
# WebChat聊天室工具

基于.NET Core + SignalR的聊天室工具，本来计划做阅后即焚的功能，但浏览器对页面激活状态的控制能力太差，因此改成了发完消息后10秒后自动隐藏，比较适合对聊天内容隐密性要求较高的同学。


### 主要特性

- 10秒倒计时自动隐藏消息，鼠标移动上去显示消息，鼠标离开隐藏消息；
- 采用WebSocket方式通讯；
- 内存中保存所有聊天记录；
- 优雅的操作界面；
- 2人聊天室模式

### 运行截图	

![帐号登录](https://github.com/coldicelion/Encryption-Web-Chat/blob/master/src/Wesley.Component.WebChat.Example/Resources/login.png?raw=true)

![聊天窗口](https://github.com/coldicelion/Encryption-Web-Chat/blob/master/src/Wesley.Component.WebChat.Example/Resources/chat.png?raw=true)
