using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using AppLog4Net;
using DataAccess.Access;
using DataAccess.Models;
using BusinessManager.Helps;

namespace BusinessManager.DataAccess
{
    class LoginDeal
    {
        public static bool DealLogin(string username, string password, AppLog applog, ref string message)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                message = "账号或密码不能为空！";
                return false;
            }

            UserInfo userInfo = null;

            //查看账号是否存在 
            if(!Access.GetUserByCard(username,ref userInfo))
            {
                applog.InfoFormat("用户:{0}--->登陆时查询账号异常.", username);
                message = "很抱歉,操作异常请重试！";

                return false;
            }

            if (userInfo == null)
            {
                applog.InfoFormat("用户:{0}--->登陆账号不存在.", username);
                message = "很抱歉,该账号不存在！";

                return false;
            }

            /*
#if Release
            if (userInfo.Status == "Y")
            {
                applog.InfoFormat("用户:{0}--->该账号已登录.", username);
                message="很抱歉,该账号已登录！";

                return false;
            }
#endif
            */
            //查看密码是否匹配
            if (Md5Hash.VerifyMd5Hash(password, userInfo.LoginPassWord))
            {
                applog.InfoFormat("用户:{0}--->登陆成功.", username);

                /*
                //更新状态
                if (!Access.UpdateStatus_Y(username))
                {
                    applog.InfoFormat("用户:{0}--->登陆时修改状态异常：{1}", username);
                    message = "很抱歉,操作异常请重试！";

                    return false;
                }
                
                applog.InfoFormat("用户:{0}--->更新在线状态为：Y.", username);
                 * */
                return true;
            }
            else
            {
                applog.InfoFormat("用户:{0}--->密码错误.", username);
                message = "很抱歉,密码输入不正确！";

                return false;
            }
        }

        public static bool DealFunc(string username, string password, AppLog applog, ref string message)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                message = "账号或密码不能为空！";
                return false;
            }

            UserInfo userInfo = null;

            //查看账号是否存在 
            if (!Access.GetUserByCard(username, ref userInfo))
            {
                applog.InfoFormat("用户:{0}--->登陆时查询账号异常.", username);
                message = "很抱歉,操作异常请重试！";

                return false;
            }

            if (userInfo == null)
            {
                applog.InfoFormat("用户:{0}--->登陆账号不存在.", username);
                message = "很抱歉,该账号不存在！";

                return false;
            }

            //查看密码是否匹配
            if (Md5Hash.VerifyMd5Hash(password, userInfo.FuncPassWord))
            {
                applog.InfoFormat("用户:{0}--->功能密码正确.", username);
                return true;
            }
            else
            {
                applog.InfoFormat("用户:{0}--->密码错误.", username);
                message = "很抱歉,功能密码输入不正确！";

                return false;
            }
        }

        public static bool DealChangeLoginPassword(string username, string password,string newpassword, AppLog applog, ref string message)
        {
            UserInfo userInfo = null;

            //查看账号是否存在 
            if (!Access.GetUserByCard(username, ref userInfo))
            {
                applog.InfoFormat("用户:{0}--->修改密码时查询账号异常.", username);
                message = "很抱歉,操作异常请重试！";

                return false;
            }

            if (userInfo == null)
            {
                applog.InfoFormat("用户:{0}--->修改密码时账号不存在.", username);
                message = "很抱歉,该账号不存在！";

                return false;
            } 

            //查看密码是否匹配
            if (Md5Hash.VerifyMd5Hash(password, userInfo.LoginPassWord))
            {
                applog.InfoFormat("用户:{0}--->帐号存在.", username);

                //更新密码
                if (Access.ResetLoginPassword(username,Md5Hash.GetMd5Hash(newpassword))==0)
                {
                    applog.InfoFormat("用户:{0}--->修改登录密码异常：{1}", username);
                    message = "很抱歉,操作异常请重试！";

                    return false;
                }

                applog.Info("用户:{0}--->修改登录密码成功.");
                return true;
            }
            else
            {
                applog.InfoFormat("用户:{0}--->原始密码错误.", username);
                message = "很抱歉,原始密码输入有误！";

                return false;
            }
        }

        public static bool DealChangeFuncPassword(string username, string password, string newpassword, AppLog applog, ref string message)
        {
            UserInfo userInfo = null;

            //查看账号是否存在 
            if (!Access.GetUserByCard(username, ref userInfo))
            {
                applog.InfoFormat("用户:{0}--->修改功能密码时查询账号异常.", username);
                message = "很抱歉,操作异常请重试！";

                return false;
            }

            if (userInfo == null)
            {
                applog.InfoFormat("用户:{0}--->修改功能密码时账号不存在.", username);
                message = "很抱歉,该账号不存在！";

                return false;
            }

            //查看密码是否匹配
            if (Md5Hash.VerifyMd5Hash(password, userInfo.FuncPassWord))
            {
                applog.InfoFormat("用户:{0}--->帐号存在.", username);

                //更新密码
                if (Access.ResetFuncPassword(username, Md5Hash.GetMd5Hash(newpassword)) == 0)
                {
                    applog.InfoFormat("用户:{0}--->修改功能密码异常：{1}", username);
                    message = "很抱歉,操作异常请重试！";

                    return false;
                }

                applog.Info("用户:{0}--->修改功能密码成功.");
                return true;
            }
            else
            {
                applog.InfoFormat("用户:{0}--->原始密码错误.", username);
                message = "很抱歉,原始密码输入有误！";

                return false;
            }
        }
    }
}
