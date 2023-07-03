# 解决方案
|类库|说明|内容|主要依赖|
|---|---|---|---|
|Kean.Infrastructure.Utilities|通用工具|工具类|无|
|Kean.Infrastructure.Cache|高速缓存访问|缓存驱动类库、缓存实例|无|
|Kean.Infrastructure.Database|数据库访问|数据库驱动类库、数据库实例、实体映射|无|
|Kean.Infrastructure.Repository|数据仓库|对缓存及数据库的实际业务操作|Cache、Database、Domain|
|Kean.Domain.Seedwork|领域基础|CQRS的基本实现|无|
|Kean.Domain.App|全局应用领域|程序启停时的全局操作|Seedwork|
|Kean.Domain.Identity|身份领域|关于账号、权限等系统运行的基本业务|Seedwork|
|Kean.Application.Command|命令应用|写操作集成|Domain|
|Kean.Application.Query|查询应用|读操作集成|Cache、Database|
|Kean.Presentation.Rest|Rest服务|可执行程序|Application|
---
# 数据库
|表|必须字段|
|---|---|
|T_SYS_MENU|MENU_ID，MENU_PARENT_ID，MENU_HEADER，MENU_URL，MENU_ICON，MENU_FLAG，MENU_ORDER|
|T_SYS_PARAM|PARAM_KEY，PARAM_VALUE|
|T_SYS_ROLE|ROLE_ID|
|T_SYS_ROLE_MENU|REL_ID，ROLE_ID，MENU_ID|
|T_SYS_SECURITY|SECURITY_ID，SECURITY_TYPE，SECURITY_VALUE，SECURITY_STATUS，SECURITY_TIMESTAMP|
|T_SYS_SECURITY_LOG|LOG_ID，LOG_TAG，LOG_TIME，LOG_CONTENT|
|T_SYS_USER|USER_ID，USER_NAME，USER_ACCOUNT，USER_PASSWORD，USER_AVATAR，USER_PASSWORD_TIME|
|T_SYS_USER_ROLE|REL_ID，USER_ID，ROLE_ID|
---
# 缓存
|键|内容|说明|
|---|---|---|
|params|{ account_security:0, ... }|系统参数。启动时从数据库同步|
|blacklist|{ 1:{}, 2:{}, ... }|黑名单。启动时从数据库同步|
|identity_index|{ 1:time, 2:time ... }|账号索引。首个会话创建时产生，同时创建详细信息以及会话索引|
|identity_x...|{ password_expired:time, password_initial:bool, url_version:$v, url_xxx:$v, url_yyy:$v, session_xxx:time, session_yyy: time... }|账号信息，包括密码过期时间、是否初始密码、权限、会话索引|
|session_x...|{ identity:1, timestamp:time, ... }|会话信息。包含账号id及最后活跃时间|
---
# 系统启动
- Kean.Presentation.Restful.StartupFilter
    - 执行 Kean.Domain.App.Commands.LoadParamCommand，从数据库中读取系统参数并写入缓存
    - 执行 Kean.Domain.App.Commands.LoadBlacklistCommand，从数据库中读取锁定的地址并写入缓存
    - 执行 Kean.Domain.Identity.Commands.FinalizeCommand，清理过期会话
- 中间件 Kean.Presentation.Restful.BlacklistMiddleware，每次请求时比对黑名单
- 中间件 Kean.Presentation.Restful.AuthenticationMiddleware，每次请求验证身份，会话不存在或过期时触发401
- 过滤器 Kean.Presentation.Restful.ActionFilter，向Action传递令牌（string token）、会话（int session）、IP（string ip）、UA（string ua）
- 过滤器 Kean.Presentation.Restful.ExceptionFilter，全局异常处理
---
# 登录：开始
- 登录命令：Kean.Domain.Identity.Commands.LoginCommand
- 命令处理程序：Kean.Domain.Identity.CommandHandlers.LoginCommandHandler
    1. 认证身份
    2. 身份认证成功后检查账号是否冻结（如果冻结，不触发成功和失败事件）
    3. 得出三种结果：token-登录成功（201）；null-登录失败（401）；String.Empty-账号冻结（423）
- 登录成功事件
    - 检查密码是否初始化，计算密码过期时间，并将结果写入缓存
    - 从数据库中获取读取权限，并写入缓存
    - 记录会话日志，并重置安全性信息
- 登录失败事件
    - 记录会话日志，并根据配置更新安全性信息
- 令牌的创建：new Kean.Domain.Identity.Models.Token()：新的 GUID
- 会话的创建：new Kean.Domain.Identity.Models.Session(token)：对令牌进行 SHA256 加密生成
- 密码的加密：SHA256 ，单向
---
# 注销：结束
- 注销命令：Kean.Domain.Identity.Commands.LogoutCommand
- 命令处理程序：Kean.Domain.Identity.CommandHandlers.LogoutCommandHandler，删除会话
- 完成事件（只有成功事件）：记录会话日志
---
# 导航：403、419、428的产生
- 提供Action，由前端决定如何调用
- 权限校验命令：Kean.Domain.Identity.Commands.NavigateCommand
- 命令处理程序：Kean.Domain.Identity.CommandHandlers.NavigateCommandHandler
    1. 检查密码是否初始化（428）
    1. 检查密码是否过期（419）
    1. 检查访问权限（403）
- ***登录成功后，前端执行第一次导航，在导航时引发419和428校验，而不是登陆时***
- ***权限更新：仅缓存全局版本，若非重登录，则在下次导航时更新具体 route版本：***
    1. *检查 route是否忽略*
    1. *检查缓存中 route版本是否匹配（一般不匹配）*
    1. *重新从数据库中同步，逐 route更新版本，再检查*
---
# 交互：401的产生
- 在中间件 Kean.Presentation.Restful.AuthenticationMiddleware 中，命令 Kean.Domain.Identity.Commands.AuthenticateCommand
- 命令处理程序：Kean.Domain.Identity.CommandHandlers.AuthenticateCommandHandler，校验会话是否存在或过期
- 成功事件（200）：更新缓存中会话的时间戳，以此刷新会话过期事件
- 失败事件（401）：如果会话存在，删之
---
# 账号维护
|内容|命令|事件|
|---|---|---|
|创建初始密码|Kean.Domain.Identity.Commands.InitializePasswordCommand|更新状态及过期时间|
|修改密码|Kean.Domain.Identity.Commands.ModifyPasswordCommand|更新过期时间|
|修改头像|Kean.Domain.Identity.Commands.ModifyAvatarCommand|无|
---
# 消息
TODO