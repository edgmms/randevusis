using Hyper.Core.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Hyper.Core.Extensions
{
    public static class SelectListExtensions
    {
        /// <summary>
        /// Convert to select list
        /// </summary>
        /// <typeparam name="TEnum">Enum type</typeparam>
        /// <param name="enumObj">Enum</param>
        /// <param name="markCurrentAsSelected">Mark current value as selected</param>
        /// <param name="valuesToExclude">Values to exclude</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the selectList
        /// </returns>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj,
           bool markCurrentAsSelected = true, int[] valuesToExclude = null) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("An Enumeration type is required.", nameof(enumObj));

            var values = Enum.GetValues(typeof(TEnum)).OfType<TEnum>().Where(enumValue => valuesToExclude == null || !valuesToExclude.Contains(Convert.ToInt32(enumValue)));

            var selectListItems = values.Select(enumValue => new
            {
                ID = Convert.ToInt32(enumValue),
                Name = EnumDescription(enumValue)
            }).ToList();


            object selectedValue = null;
            if (markCurrentAsSelected)
                selectedValue = Convert.ToInt32(enumObj);
            return new SelectList(selectListItems, "ID", "Name", selectedValue);
        }

        /// <summary>
        /// Convert to select list
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="objList">List of objects</param>
        /// <param name="selector">Selector for name</param>
        /// <returns>SelectList</returns>
        public static SelectList ToSelectList<T>(this T objList, Func<BaseEntity<int>, string> selector) where T : IEnumerable<BaseEntity<int>>
        {
            return new SelectList(objList.Select(p => new { ID = p.Id, Name = selector(p) }), "ID", "Name");
        }

        private static string EnumDescription<TEnum>(TEnum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }
    }
}
