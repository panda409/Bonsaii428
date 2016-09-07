using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Bonsaii.Models;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Linq.Expressions;
using System.Collections.Generic;
using Bonsaii.Models.App;

namespace Bonsaii.Controllers
{
    public class VerifyCodeTool
    {
        private int VerifyCodeOverTimeSeconds = 180;        //验证码的过期时间
        //发送短信验证码的文本信息
        public string Message { get; set; }
        //4位数的验证码
        public string Code { get; set; }

        private struct BalanceResult
        {
            public int nResult;
            public long dwCorpId;
            public int nStaffId;
            public float fRemain;
        }

        // 接口地址
        private static readonly string POST_URL = "http://smsapi.c123.cn/OpenPlatform/OpenApi";

        // 接口帐号, 请换成你的帐号, 格式为 1001@+8位登录帐号+0001
        private static readonly string ACCOUNT = "1001@501209470001";

        // 接口密钥, 请换成你的帐号对应的接口密钥
        private static readonly string AUTHKEY = "02D0FD280193B79827453D90F1152E4B";

        // 通道组编号, 请换成你的帐号可以使用的通道组编号
        private static readonly uint CGID = 52;

        /************************************************************************/
        /* UrlEncode
        /* 对指定字符串进行URL标准化转码
        /************************************************************************/
        private static string UrlEncode(string text, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byData = encoding.GetBytes(text);
            for (int i = 0; i < byData.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byData[i], 16));
            }
            return sb.ToString();
        }

        /************************************************************************/
        /* sendQuery
        /* 向指定的接口地址POST数据并返回响应数据
        /************************************************************************/
        private static string sendQuery(string url, string param)
        {
            // 准备要POST的数据
            byte[] byData = Encoding.UTF8.GetBytes(param);

            // 设置发送的参数
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "POST";
            req.Timeout = 5000;
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = byData.Length;

            // 提交数据
            Stream rs = req.GetRequestStream();
            rs.Write(byData, 0, byData.Length);
            rs.Close();

            // 取响应结果
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);

            try
            {
                return sr.ReadToEnd();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }

            sr.Close();
            return null;
        }

        /************************************************************************/
        /* 分析返回的结果值
        /************************************************************************/
        private static int parseResult(string sResult)
        {
            if (sResult != null)
            {
                try
                {
                    // 对返回值分析
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(sResult);
                    XmlElement root = xml.DocumentElement;
                    string sRet = root.GetAttribute("result");
                    return Convert.ToInt32(sRet);
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return -100;
        }

        private static BalanceResult parseBalanceResult(string sResult)
        {
            BalanceResult ret = new BalanceResult();
            int nRet = parseResult(sResult);
            ret.nResult = nRet;
            if (nRet < 0) return ret;

            try
            {
                // 对返回值分析
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(sResult);
                XmlElement root = xml.DocumentElement;
                if (nRet > 0)
                {
                    XmlNode item = xml.SelectSingleNode("/xml/Item");
                    if (item != null)
                    {
                        ret.dwCorpId = Convert.ToInt64(item.Attributes["cid"].Value);
                        ret.nStaffId = Convert.ToInt32(item.Attributes["sid"].Value);
                        ret.fRemain = (float)Convert.ToDouble(item.Attributes["remain"].Value);
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return ret;
        }

        /************************************************************************/
        /* 群发接口
        /* 手机号码, 如有多个使用逗号分隔, 支持1~3000个号码
        /* 内容, 500字以内 
        /************************************************************************/
        public static int sendOnce(string mobile, string content, uint uCgid = 0, uint uCsid = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("action=sendOnce&ac=");
            sb.Append(ACCOUNT);
            sb.Append("&authkey=");
            sb.Append(AUTHKEY);
            sb.Append("&cgid=");
            sb.Append(uCgid > 0 ? uCgid : CGID);
            if (uCsid > 0)
            {
                sb.Append("&csid=");
                sb.Append(uCsid);
            }
            sb.Append("&m=");
            sb.Append(mobile);
            sb.Append("&c=");
            sb.Append(UrlEncode(content, Encoding.UTF8));

            string sResult = sendQuery(POST_URL, sb.ToString());
            return parseResult(sResult);
        }



        /// <summary>
        /// 针对某一个电话号码发送验证码
        /// </summary>
        /// <param name="PhoneNumber">要发送验证码的手机号</param>
        /// <returns>大于0说明发送成功，否则是失败</returns>
        public int sendVerifyCode(string PhoneNumber)
        {
  //          this.Code = (new Random().Next(1111, 9999)).ToString();
            this.Code = "1984";
            string content = this.Message + this.Code;
            //向用户发送验证码
     //       int rect  = sendOnce(PhoneNumber, content);
            int rect = 1;
            //短信验证发送失败！
            if (rect <= 0)
            {
                return rect;
            }

            using (var vCode = new SystemDbContext())
            {
                VerifyCode tmp = vCode.VerifyCodes.Find(PhoneNumber);
                DateTime createTime = System.DateTime.Now;
                //电话在数据库中已经存在，说明这个用户已经注册过了,更新信息，照常发送验证码
                if (tmp != null)
                {
                    tmp.Code = Code;
                    tmp.CreateTime = createTime;
                    tmp.OverTime = createTime.AddSeconds(VerifyCodeOverTimeSeconds);
                    vCode.Entry(tmp).State = System.Data.Entity.EntityState.Modified;
                    vCode.SaveChanges();
                }
                else
                {
                    VerifyCode tmpCode = new VerifyCode();
                    tmpCode.Code = Code;
                    tmpCode.PhoneNumber = PhoneNumber;
                    tmpCode.CreateTime = createTime;
                    tmpCode.OverTime = createTime.AddSeconds(VerifyCodeOverTimeSeconds);
                    vCode.VerifyCodes.Add(tmpCode);
                    vCode.SaveChanges();
                }
            }
            return 1;
        }
    }
}