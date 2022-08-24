using jp.co.jpsys.util;
using Jpsys.SagyoManage.Model.DALExceptions;
using Jpsys.SagyoManage.Property;
using Jpsys.SagyoManage.SQLServerDAL;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jpsys.SagyoManage.FrameLib
{
    /// <summary>
    /// メール送信のヘルパークラスです。
    /// </summary>
    public class MailHelper
    {
        /// <summary>
        /// メールを一括送信します。
        /// </summary>
        /// <param name="apiKey">SendGridのAPI key</param>
        /// <param name="sender">送信者アドレス</param>
        /// <param name="recipientList">受信者アドレスのリスト</param>
        /// <param name="titles">メールタイトルのリスト</param>
        /// <param name="body">メール本文</param>
        /// <param name="filePath">添付ファイルパス</param>
        /// <param name="substitutions">送信先毎にメール本文を置換する為のディクショナリ</param>
        /// <returns></returns>
        public async Task<Response> SendMail(
            string apiKey,
            EmailAddress sender,
            List<EmailAddress> recipientList,
            List<string> titles,
            string body,
            string filePath,
            List<Dictionary<string, string>> substitutions = null)
        {
            //try
            //{
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    throw new Jpsys.SagyoManage.Model.DALExceptions.CanRetryException("SendGridのAPI keyがシステム設定テーブルに未登録です。");
                }

                var client = new SendGridClient(apiKey);

                // 受信者数
                int recipientCount = recipientList.Count;

                // 送信先毎の置換文字列がnullだった場合、ダミーデータを準備する必要がある。
                if (substitutions == null)
                {
                    substitutions = new List<Dictionary<string, string>>();
                    for (int i = 0; i < recipientCount; i++)
                    {
                        substitutions.Add(new Dictionary<string, string>());
                    }
                }

                // 送信
                var msg = SendGrid.Helpers.Mail.MailHelper.CreateMultipleEmailsToMultipleRecipients(sender, recipientList, titles, body, string.Empty, substitutions);
                var str = msg.Serialize();

                using (var fileStream = File.OpenRead(filePath))
                {
                    await msg.AddAttachmentAsync(Path.GetFileName(@filePath), fileStream);
                }

                var response = client.SendEmailAsync(msg).Result;

                return response;


                ////送信処理判定
                //if (response.StatusCode != HttpStatusCode.Accepted)
                //{
                //    throw new CanRetryException("メール送信処理に失敗しました。\r\n\r\n結果コード："
                //        + ((int)response.StatusCode).ToString() + " " + response.StatusCode.ToString()
                //        + "\r\n\r\nSendGrid管理者画面より送信状況をご確認の上、\r\n再度メール送信処理を行ってください。", MessageBoxIcon.Warning);
                //}
                //else
                //{
                //    MessageBox.Show(
                //        FrameUtilites.GetDefineMessage("MI2001001"),
                //        f.Text,
                //        MessageBoxButtons.OK,
                //        MessageBoxIcon.Information
                //        );
                //}

            //}
            //catch (Exception e)
            //{
            //    FrameUtilites.ShowExceptionMessage(e, f);
            //}
        }
    }
}
