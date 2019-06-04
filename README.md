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

### 2.1 文件下载类库 DownLoadFile

DownLoadFile.dll负责更新文件下载任务，支持多线程与下载进度回调，调用方便快捷。

### 2.2 压缩文件处理类库 ICSharpCode.SharpZipLib

ICSharpCode.SharpZipLib.dll是一款ZIP文件压缩解压缩的类库，配合ZipHelper使用，轻松完成更新文件的解压缩工作。

### 2.3 JSON解析类库 Newtonsoft.Json

Newtonsoft.Json.dll是一款.NET中开源的Json序列化和反序列化类库，根据Github的API完成JsonGitHub的解析工作。

## 3、使用说明

使用API动态调用UpdataForGitHub，实现多样化自定义功能。

参数 | 说明 | 备注  
-|-|-
Username | Github用户名 | 必填，例如：xiaoxinpro |
Project | Github项目名 | 必填，例如：UpdataForGitHub |
Version | 当前版本号 | 必填，例如：0.1.0.0 |
Title | 显示在窗口的标题 | 选填，默认：软件升级工具 |
IsAutoRun | 是否更新完成自动运行 |选填，默认：False |
Run | 需要运行的文件名 | 若选择IsAutoRun则填写相应的文件名 |

## 4、已知局限性

### 4.1、安全协议问题 (已解决)

~~由于Github的API的访问要求为TLS1.2，在C#中必须使用.net4.5框架。~~

```
ServicePointManager.SecurityProtocol = (SecurityProtocolType)3702; // 请求GitHub前需要运行
```

使用框架已改为.Net4.0

### 4.2、其他问题

如有问题请提交[Issues](https://github.com/xiaoxinpro/UpdataForGitHub/issues)，谢谢支持！

