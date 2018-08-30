using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace RestFul.Controllers
{
    public class PutController : ApiController
    {
        

        [RequestAuthorize]
        public HttpResponseMessage Put(string table, string where)
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象
            ResultJson content = new ResultJson();
            try
            {
                SqlParameter[] cmdParms = new SqlParameter[request.Form.Count];
                string fieldValues = "";
                for (int i = 0; i < request.Form.Count; i++)
                {
                    fieldValues += request.Form.GetKey(i) + "=@" + request.Form.GetKey(i) + ",";
                    SqlParameter p = new SqlParameter("@" + request.Form.GetKey(i), request.Form[i]);
                    cmdParms[i] = p;
                }
                fieldValues = fieldValues.Substring(0, fieldValues.LastIndexOf(","));
                fieldValues = "(" + fieldValues + ")";

                string tsql = "UPDATE " + table + " SET " + fieldValues + " where" + where + " " + Environment.NewLine;
                tsql += "select * from " + table + " where id=@@Identity";

                int rows = SqlDbHelper.ExecuteSqlUpdate(tsql, cmdParms);
                content.code = "success";
                content.message = "";
                content.info = "成功更新" + rows + "行";
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
