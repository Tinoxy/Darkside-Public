using System.Reflection;

namespace Glide
{
    internal class GlideInfo
    {
        private static BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public string PropertyName { get; private set; }
        public Type PropertyType { get; private set; }

        private FieldInfo field;
        private PropertyInfo prop;
        private object Target;

        public object Value
        {
            get => this.field != null ?
                    this.field.GetValue(this.Target) :
                    this.prop.GetValue(this.Target, null);

            set
            {
                if (this.field != null) this.field.SetValue(this.Target, value);
                else this.prop.SetValue(this.Target, value, null);
            }
        }

        public GlideInfo(object target, PropertyInfo info)
        {
            this.Target = target;
            this.prop = info;
            this.PropertyName = info.Name;
            this.PropertyType = this.prop.PropertyType;
        }

        public GlideInfo(object target, FieldInfo info)
        {
            this.Target = target;
            this.field = info;
            this.PropertyName = info.Name;
            this.PropertyType = info.FieldType;
        }

        public GlideInfo(object target, string property, bool writeRequired = true)
        {
            this.Target = target;
            this.PropertyName = property;

            Type targetType = target as Type ?? target.GetType();

            if ((this.field = targetType.GetField(property, flags)) != null)
            {
                this.PropertyType = this.field.FieldType;
            }
            else if ((this.prop = targetType.GetProperty(property, flags)) != null)
            {
                this.PropertyType = this.prop.PropertyType;
            }
            else
            {
                //	Couldn't find either
                throw new Exception(string.Format("Field or {0} property '{1}' not found on object of type {2}.",
                    writeRequired ? "read/write" : "readable",
                    property, targetType.FullName));
            }
        }
    }
}