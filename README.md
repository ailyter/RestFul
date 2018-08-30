# RestFul
基于WebApi的RestFul服务
用别人的接口总感觉心里不踏实，增加需求还得求人干脆自已整一个；

本接口使用C# webapi 2.0，编译环境为vs2017 net4.5.2；
使用了两个包，Newtonsoft.Json(11.0.0) 和 MySql.Data(6.10.8)大家可以自行在nuget中增加

目前支持sql Server 和 Mysql 两种数据库；
支持身份验证，代码未实现大家可根据自已情况实现；
调用方式：

新增:
接口地址：http://youweb/api/add/表名
数据Form传输，新增成功会返回此行完整数据；

更新:
接口地址：http://youweb/api/put/表名/where条件(不含where关键字)
数据Form传输，新增成功会返回此行完整数据；

查询：
Sqlserver：http://youweb/api/Query/表名/每页行数/页码
Mysql：http://youweb/api/MysqlQuery/表名/每页行数/页码
里面还有两个重载方法大家可以自已研究

删除：
接口地址：http://youweb/api/delete/表名/where语句(不含where关键字)

分享此代码希望对大家有用  2018-08-30 by ailyter



