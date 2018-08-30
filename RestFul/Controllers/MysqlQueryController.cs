using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace RestFul.Controllers
{
    public class MysqlQueryController : ApiController
    {
        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            var content = new
            {
                code = "success",
                message = "哈哈哈"
            };
            string json = JsonConvert.SerializeObject(content);
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

        [AllowAnonymous]
        public HttpResponseMessage Get(string table, int pageSize, int page)
        {
            try
            {
                DataTable dt = MysqlHelper.QueryTable(table, "id asc", pageSize, page);
                string str = StrHelper.ToJson(dt);
                string json = "{\"code\":\"success\",\"message\":\"\",\"info\":" + str + "}";
                HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json") };
                return result;
            }
            catch (Exception ex)
            {
                ResultJson content = new ResultJson();
                content.code = "fail";
                content.message = ex.Message;
                string json = JsonConvert.SerializeObject(content);
                HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json") };
                return result;
            }
        }
        [AllowAnonymous]
        public HttpResponseMessage Get(string table, int pageSize, int page, string where)
        {
            try
            {
                DataTable dt = MysqlHelper.QueryTable(table, "id asc", pageSize, page, where);
                string str = StrHelper.ToJson(dt);
                string json = "{\"code\":\"success\",\"message\":\"\",\"info\":" + str + "}";
                HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json") };
                return result;
            }
            catch (Exception ex)
            {
                ResultJson content = new ResultJson();
                content.code = "fail";
                content.message = ex.Message;
                string json = JsonConvert.SerializeObject(content);
                HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json") };
                return result;
            }
        }
        [AllowAnonymous]
        public HttpResponseMessage Get(string table, int pageSize, int page, string where, string orderField)
        {
            try
            {
                DataTable dt = MysqlHelper.QueryTable(table, orderField, pageSize, page, where);
                string str = StrHelper.ToJson(dt);
                string json = "{\"code\":\"success\",\"message\":\"\",\"info\":" + str + "}";
                HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json") };
                return result;
            }
            catch (Exception ex)
            {
                ResultJson content = new ResultJson();
                content.code = "fail";
                content.message = ex.Message;
                string json = JsonConvert.SerializeObject(content);
                HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json") };
                return result;
            }
        }



    }
}
