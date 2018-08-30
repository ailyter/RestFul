using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace RestFul.Controllers
{
    public class AddController : ApiController
    {
        [RequestAuthorize]
        public HttpResponseMessage Post(string table)
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象
            ResultJson content = new ResultJson();
            try
            {
                SqlParameter[] cmdParms = new SqlParameter[request.Form.Count];
                string fields = "";
                string values = "";
                for (int i = 0; i < request.Form.Count; i++)
                {
                    fields += request.Form.GetKey(i) + ",";
                    values += "@" + request.Form.GetKey(i) + ",";
                    SqlParameter p = new SqlParameter("@" + request.Form.GetKey(i), request.Form[i]);
                    cmdParms[i] = p;
                }
                fields = fields.Substring(0, fields.LastIndexOf(","));
                fields = "(" + fields + ")";
                values = values.Substring(0, values.LastIndexOf(","));
                values = "(" + values + ")";

                string tsql = "INSERT INTO " + table + " " + fields + " values" + values + Environment.NewLine;
                tsql += "select * from " + table + " where id=@@Identity";

                DataTable dt = SqlDbHelper.ExecuteSqlInsert(tsql, cmdParms);
                string str = StrHelper.ToJson(dt);
                str = str.Replace("[", "").Replace("]", "");
                string json = "{\"code\":\"success\",\"message\":\"\",\"info\":" + str + "}";
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
