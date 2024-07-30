using ReactiveUI;
using System.Windows;

namespace v2rayN.Handler
{
    public class NoticeHandler
    {
        public void ShowMessageBox(string? content)
        {
            if (content.IsNullOrEmpty())
            {
                return;
            }
            MessageBox.Show(content);
        }

        public void SendMessage(string? content)
        {
            if (content.IsNullOrEmpty())
            {
                return;
            }
            MessageBus.Current.SendMessage(content, Global.CommandSendMsgView);
        }

        public void SendMessage(string? content, bool time)
        {
            if (content.IsNullOrEmpty())
            {
                return;
            }
            content = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} {content}";
            SendMessage(content);
        }

        public void SendMessageAndEnqueue(string? msg)
        {
            ShowMessageBox(msg);
            SendMessage(msg);
        }
    }
}