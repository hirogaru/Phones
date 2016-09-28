using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Phones.DataAccess;

namespace Phones.Models
{
    /// <summary>
    /// Метод по очистке строки запроса: убираем разделители типа [,], {,}, (,)
    /// и метод Search, который принимает значения IQueryable, и строку поиска, а возвращает данные по поиску
    /// </summary>
    public class SearchEngine
    {
        private static string CleanContent(string content)
        {
            content =
                content.Replace("\\", string.Empty).
                Replace("|", string.Empty).
                Replace("(", string.Empty).
                Replace(")", string.Empty).
                Replace("[", string.Empty).
                Replace("]", string.Empty).
                Replace("*", string.Empty).
                Replace("?", string.Empty).
                Replace("}", string.Empty).
                Replace("{", string.Empty).
                Replace("^", string.Empty).
                Replace("+", string.Empty);  //убираем элементы regex

            var words = content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries); //избавляемся от разделителей
            var sb = new StringBuilder();
            foreach (var word in
                words.Select(t => t.ToLowerInvariant().Trim()).Where(word => word.Length > 1))
            {
                sb.AppendFormat("{0} ", word);
            }

            return sb.ToString();
        }

        public static IEnumerable<Contacts> Search(string searchString, IQueryable<Contacts> source)
        {
            var term = CleanContent(searchString.ToLowerInvariant().Trim()); //Очищаем строку запроса
            var terms = term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //Создаём массив искомых слов
            var regex = string.Format(CultureInfo.InvariantCulture, "({0})", string.Join("|", terms)); //Создаем regex для поиска

            foreach (var entry in source) //Просматриваем весь список, переданный через IQueryable
            {
                var rank = 0;

                string value1 = entry.phoneNum, //по каким полям будем сравнивать
                    value2 = entry.contactName;

                if ((!string.IsNullOrWhiteSpace(value1)) && (!string.IsNullOrWhiteSpace(value2)))
                {
                    rank += Regex.Matches(value1.ToLowerInvariant(), regex).Count;
                    rank += Regex.Matches(value2.ToLowerInvariant(), regex).Count;
                }
                if (rank > 0) //Если есть совпадение, то выносим его в IEnumerable 
                {
                    yield return entry;
                }
            }
        }
    }
}
