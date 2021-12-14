using SocialballWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SocialballWebAPI.Extensions
{
    public static class EnumExtensions
    {
        public static List<EnumValue> GetValues<T>()
        {
            List<EnumValue> values = new List<EnumValue>();
            foreach (var itemType in Enum.GetValues(typeof(T)))
            {
                values.Add(new EnumValue()
                {
                    Name = itemType.GetType().GetMember(itemType.ToString()).First().GetCustomAttribute<DisplayAttribute>() != null ? itemType.GetType().GetMember(itemType.ToString()).First().GetCustomAttribute<DisplayAttribute>().Name : Enum.GetName(typeof(T), itemType),
                    Value = (int)itemType
                });
            }
            return values;
        }
    }
}
