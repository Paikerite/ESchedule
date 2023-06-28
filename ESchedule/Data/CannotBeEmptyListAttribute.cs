using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ESchedule.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CannotBeEmptyListAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IEnumerable;
            return list != null && list.GetEnumerator().MoveNext();
        }
    }
}
