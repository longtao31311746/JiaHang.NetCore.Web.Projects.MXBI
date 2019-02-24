namespace JiaHang.NetCore.Web.Projects.MXBI.Model.SysRoute
{
    public class MethodRouteModel
    {
        public string Id { get; set; }

        /// <summary>
        /// 控制器ID
        /// </summary>
        public string ControllerId { get; set; }

        /// <summary>
        /// 具体方法路径  
        /// 控制器后面所有路径
        /// </summary>
        public string MethodPath { get; set; }


        /// <summary>
        /// 方法别名
        /// </summary>
        public string MethodAlias { get; set; }

        /// <summary>
        /// 请求类型
        /// 先匹配类型 有多个类型相同，再匹配路径
        /// </summary>
        public string MethodType { get; set; }
    }
}
