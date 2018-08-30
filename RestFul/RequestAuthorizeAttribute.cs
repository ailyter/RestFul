using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;
using Newtonsoft.Json;

namespace RestFul
{
    public class RequestAuthorizeAttribute : AuthorizeAttribute
    {
        //重写基类的验证方式，加入我们自定义的Ticket验证
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //从http请求的头里面获取身份验证信息，验证是否是请求发起方的ticket
            var authorization = actionContext.Request.Headers.Authorization;
            //Debug.WriteLine(authorization);
            if ((authorization != null) && (authorization.Scheme != ""))
            {
                //解密用户ticket,并校验用户名密码是否匹配
                var encryptTicket = authorization.Scheme;
                //Debug.WriteLine(encryptTicket);
                if (ValidateTicket(encryptTicket))
                {
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            //如果取不到身份验证信息，并且不允许匿名访问，则返回未验证401
            else
            {
                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                if (isAnonymous) base.OnAuthorization(actionContext);
                else HandleUnauthorizedRequest(actionContext);
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actioncontext)
        {
            base.HandleUnauthorizedRequest(actioncontext);

            var response = actioncontext.Response = actioncontext.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Forbidden;
            var content = new
            {
                code = "fail",//success
                message = "服务端拒绝访问"
            };
            //HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json") };
            //response.Content = new StringContent(Json.Encode(content), Encoding.UTF8, "application/json");
            response.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.GetEncoding("UTF-8"), "application/json");
        }

        //校验用户名密码（正式环境中应该是数据库校验）
        private bool ValidateTicket(string encryptTicket)
        {
            //解密Ticket
            //var strTicket = FormsAuthentication.Decrypt(encryptTicket).UserData;

            return true;

            
        }
    }
}