# UpdataForGitHub

C#基于GitHub的Releases实现的软件升级工具，本项目目前处于开发前期阶段，目前无法用于生产。

## 1、原理说明

本项目主要依靠GitHub的API实现的

```
https://api.github.com/repos/用户名/项目名/releases/latest
```

替换上面的“用户名”和“项目名”，例如`https://api.github.com/repos/xiaoxinpro/BleTestTool/releases/latest`

通过Get请求获取项目最新的Releases版本信息，本项目使用`Newtonsoft.Json`解析接收的到JSON数据。

在软件中显示最新的版本信息并引导用户更新。

通过获取到的下载地址，使用`DownLoadFile`类库下载更新包。

下载完成后引用`ICSharpCode.SharpZipLib`类库将更新包解压并覆盖原有软件。

更新完成后删除无用的缓存文件并打开更新后的程序文件，从而实现的软件更新的功能。

## 2、使用的类库说明

后续详细更新...


## 3、未来将添加的功能

使用API动态调用UpdataForGitHub，实现多样化自定义功能。

## 4、已知局限性

### 4.1、安全协议问题

由于Github的API的访问要求为TLS1.2，在C#中必须使用.net4.5框架。(已解决)

```
ServicePointManager.SecurityProtocol = (SecurityProtocolType)3702; // 请求GitHub前需要运行
```

使用框架已改为.Net4.0

### 4.2、其他问题

如有问题请提交[Issues](https://github.com/xiaoxinpro/UpdataForGitHub/issues)，谢谢支持！

