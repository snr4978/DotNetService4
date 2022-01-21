using Kean.Application.Command.Interfaces;
using Kean.Application.Command.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kean.Presentation.Rest.Controllers
{
    /// <summary>
    /// 会话服务
    /// </summary>
    [ApiController, Route("api/sessions")]
    public class SessionsController : ControllerBase
    {
        private readonly IIdentityService _identityCommandService; // 身份命令服务

        /// <summary>
        /// 依赖注入
        /// </summary>
        public SessionsController(
            IIdentityService identityCommandService)
        {
            _identityCommandService = identityCommandService;
        }

        /// <summary>
        /// 创建资源（登录）
        /// </summary>
        /// <response code="201">成功</response>
        /// <response code="401">账号或密码错误</response>
        /// <response code="423">账号被冻结</response>
        [HttpPost, Anonymous]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(423)]
        public async Task<IActionResult> Login(User user, [FromMiddleware] string ip, [FromMiddleware] string ua)
        {
            var token = await _identityCommandService.Login(user, ip, ua);
            return token switch
            {
                null => StatusCode(401),
                "" => StatusCode(423),
                _ => StatusCode(201, new { token })
            };
        }

        /// <summary>
        /// 删除资源（注销）
        /// </summary>
        /// <response code="204">成功</response>
        [HttpDelete, Anonymous]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Logout(string token, string reason)
        {
            await _identityCommandService.Logout(token, reason);
            return StatusCode(204);
        }








        //[HttpGet, Anonymous]
        //public async Task<IActionResult> Get()
        //{
        //    var driver = Infrastructure.Database.Configuration.Configure(null);
        //    using (var context = driver.CreateContext())
        //    {
        //        var result = await context.From<
        //            Infrastructure.Database.Repository.Default.Entities.T_SYS_USER_ROLE,
        //            T_SYS_USER<T1>, T_SYS_USER<T2>>()
        //            .Join<Infrastructure.Database.Repository.Default.Entities.T_SYS_USER_ROLE, T_SYS_USER<T1>>(Infrastructure.Database.Join.Inner, (a, b) => a.USER_ID == b.USER_ID)
        //            .Join<Infrastructure.Database.Repository.Default.Entities.T_SYS_USER_ROLE, T_SYS_USER<T2>>(Infrastructure.Database.Join.Inner, (a, b) => a.ROLE_ID == b.USER_ID)
        //            .Where((a, b, c) => c.USER_ID > 0)
        //            .Select((a, b, c) => new { id=a.REL_ID,user1=b.USER_ID,user2=c.USER_ID });
        //        return Ok();
        //    }
        //}
        //[HttpGet]
        //[Route("{name}")]
        //public async Task<IActionResult> Get(string name)
        //{
        //    //await _command.Set(5, name);
        //    //if (_notification.Count == 0)
        //    //{
        //    //    return Ok(await _query.Get(5));
        //    //}
        //    //else
        //    //{
        //    //    return Ok(_notification[0].ErrorMessage);
        //    //}



        //    //var driver = Infrastructure.Repository.Configuration.Configure(null);
        //    //using (var context = driver.CreateContext())
        //    //{
        //    //int i = 0, j = 5;
        //    //var list = context.From<T_SYS_USER>().Where(u => u.USER_ID > i && u.USER_ID <= j)
        //    //    .OrderBy(u=>u.USER_ID,Infrastructure.Repository.Order.Descending)
        //    //    .OrderBy(u => u.USER_NAME, Infrastructure.Repository.Order.Ascending)
        //    //    .Select();

        //    //var person = new T_PERSON
        //    //{
        //    //    ID = 5,
        //    //    CODE = "0051",
        //    //    NAME = "E1",
        //    //    GENDER = "0"
        //    //};
        //    //context.From<T_PERSON>().Update(person);
        //    //var param = new Parameters();
        //    //param.AddDynamicParams(new
        //    //{
        //    //    CODE = "E005",
        //    //    NAME = "EE"
        //    //});
        //    //context.From<T_PERSON>().Where(r => r.ID == 5).Update(param);
        //    //context.From<T_PERSON>().Where(r => r.ID == 5).Update(new
        //    //{
        //    //    CODE = "005",
        //    //    NAME = "E"
        //    //});


        //    //var gender = new string[] { "0", "1" };
        //    //var list = context.From<T_PERSON>()
        //    //    .GroupBy(u => u.GENDER)
        //    //    .Having(u => gender.Contains(u.GENDER))
        //    //    .Select(u => new
        //    //    {
        //    //        gender = u.GENDER,
        //    //        count = Infrastructure.Repository.Function.Count(u.ID)
        //    //    });

        //    //var gender = new string[] { "男", "女" };
        //    //var list = context.From<T_PERSON, T_GENDER>()
        //    //    .Join(Infrastructure.Repository.Join.Inner, (p, g)=> p.GENDER == g.ID)
        //    //    .GroupBy((p, g) => g.NAME)
        //    //    .Having((p, g) => gender.Contains(g.NAME))
        //    //    .Select((p, g) => new
        //    //    {
        //    //        gender = g.NAME,
        //    //        count = Infrastructure.Repository.Function.Count(p.ID)
        //    //    });

        //    //var list = context.From<T_PERSON, T_GENDER, T_GROUP>()
        //    //    .Join<T_PERSON, T_GENDER>(Join.Inner, (p, g) => p.GENDER == g.ID)
        //    //    .Join<T_PERSON, T_GROUP>(Join.Inner, (p, g) => p.GROUP == g.ID)
        //    //    .Select((pe, ge, gr) => new
        //    //    {
        //    //        name = pe.NAME,
        //    //        gender = ge.NAME,
        //    //        group = gr.NAME
        //    //    });
        //    //return Ok(list);
        //    return Ok();
        //    //}
        //}
    }
}
