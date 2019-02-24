namespace JiaHang.NetCore.Web.Projects.MXBI.Model
{
    public class FuncResult
    {
        public string Message { get; set; }
        public object Content { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class FuncResult<T>
    {
        public string Message { get; set; }
        public T Content { get; set; }
        public bool IsSuccess { get; set; }
    }
}
