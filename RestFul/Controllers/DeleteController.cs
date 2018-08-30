using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace RestFul.Controllers
{
    public class DeleteController : ApiController
    {
        [RequestAuthorize]
        public HttpResponseMessage Delete(string table,string where)
        {
            ResultJson content = new ResultJson();
            try
            {
                string tsql = "delete from {0} where {where}";
                tsql = string.Format(tsql, table, where);
                int n = SqlDbHelper.ExecuteSql(tsql);
                content.code = "success";
                content.message = "成功删除" + n + "条";
                string json = JsonConvert.SerializeObject(content);
                HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json") };
                return result;
            }
            catch (Exception ex)
            {
                content.code = "fail";
                content.message = ex.Message;
                string json = JsonConvert.SerializeObject(content);
                HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json") };
                return result;
            }
        }
    }
}
