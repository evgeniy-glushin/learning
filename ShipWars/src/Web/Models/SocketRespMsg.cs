
using Web.Enums;

namespace Web.Models
{
    public class SocketRespMsg
    {
        public object Content { get; set; }
        public string ErrorMessage { get; set; }
        public RespMsgState State { get; set; }
    }
}
