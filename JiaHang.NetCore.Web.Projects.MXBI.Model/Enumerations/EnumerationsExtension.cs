using System;
using System.ComponentModel;
using System.Reflection;

namespace JiaHang.NetCore.Web.Projects.MXBI.Model.Enumerations
{
    public static class EnumerationsExtension
    {
        /// <summary>
            /// 获取枚举的描述信息
            /// </summary>
        public static string GetDescription(this Enum em)
        {
            Type type = em.GetType();
            FieldInfo fd = type.GetField(em.ToString());
            if (fd == null)
            {
                return string.Empty;
            }

            object[] attrs = fd.GetCustomAttributes(typeof(DescriptionAttribute), false);
            string name = string.Empty;
            foreach (DescriptionAttribute attr in attrs)
            {
                name = attr.Description;
            }
            return name;
        }

        
    }
}
