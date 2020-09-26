using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(DAL.UserInfo.Instance.GetCount());
        }
        [HttpPut]
        public ActionResult Put([FormBody]Model.UserInfo users)
        {
            try
            {
                var n = DAL.UserInfo.Instance.Update(users);
                if (n != 0)
                    return Ok(Result.Ok("修改成功"));
                else
                    return Ok(Result.Err("用户名不存在"));
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("null"))
                    return Ok(Result.Err("密码、身份证不能为空"));
                else
                    return Ok(Result.Err(ex.Message));
            }
        }
        [HttpPost]
        public ActionResult Post([FormBody]Model.UserInfo users)
        {
            try
            {
                int n = DAL.UserInfo.Instance.Add(users);
                return Ok(Result.Ok("添加成功"));
            }
            catch(Exception ex)
            {
                if (ex.Message.ToLower().Contains("primary"))
                    return Ok(Result.Err("用户名、密码、身份证不能为空"));
                else
                    return Ok(Result.Err(ex.Message));
            }
            [HttpDelete("{username}")]
            public ActionResult Delete(string username)
            {
                try
                {
                    var n = DAL.UserInfo.Instance.Update(username);
                    if (n != 0)
                        return Ok(Result.Ok("删除成功"));
                    else
                        return Ok(Result.Err("用户名不存在"));
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToLower().Contains("foreign"))
                        return Ok(Result.Err("发布了作品或活动的用户不能删除"));
                    else
                        return Ok(Result.Err(ex.Message));
                }
                [HttpPost("check")]
                public ActionResult Usercheck([FormBody] Model.UserInfo users)
                {
                    try
                    {
                        var result = DAL.UserInfo.Instance.GetModel(users.userName); ;
                        if (result == null)
                            return Ok(Result.Err("用户名错误"));
                        else if (result.passWord == users.passWord)
                        {
                            if (result.type == "管理员")
                            {
                                result.passWord = "*********";
                                return Ok(Result.Ok("管理员登录成功", result));
                            }
                            else
                                return Ok(Result.Err("只有管理员能够进入后台管理"));
                        }
                        else
                                return Ok(Result.Err("密码错误"));
                        }
                    catch (Exception ex)
                    {
                        return Ok(Result.Err(ex.Message));
                    }
                    [HttpPost("gencheck")]
                    public ActionResult genUsercheck([FormBody] Model.UserInfo users)
                    {
                        try
                        {
                            var result = DAL.UserInfo.Instance.GetModel(users.userName); ;
                            if (result == null)
                                return Ok(Result.Err("用户名错误"));
                            else if (result.passWord == users.passWord)
                            { 
                                    result.passWord = "*********";
                                    return Ok(Result.Ok("登录成功", result));
                                }
                                else
                                return Ok(Result.Err("密码错误"));
                        }
                        catch (Exception ex)
                        {
                            return Ok(Result.Err(ex.Message));
                        }
                        [HttpPost("page")]
                        public ActionResult getPage([FormBody] Model.Page page)
                        {
                            var result = DAL.UserInfo.Instance.GetPage(page);
                            if (result.Count() == 0)
                                return Ok(Result.Err("返回记录为0"));
                            else
                                return Ok(Result.Ok(result));
                        }
                    }
                }
        }
    }
}
