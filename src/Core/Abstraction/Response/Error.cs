

namespace Abstraction.Base.Response
{
    public class Error
    {
        public int code { get; set; } = 0;
        public string message { get; set; }
        public string details { get; set; }=null;
        public object validationErrors { get; set; } = null;

    }
}
