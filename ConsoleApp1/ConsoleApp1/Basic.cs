using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public class ServiceRow : Dictionary<string, object>
    {
        ServiceRow parent;

        public ServiceRow()
        { }

        public ServiceRow(ServiceRow sr)
        {
            parent = sr;
        }

        /// <summary>
        /// Holds the original maped value (i.e. before additional processing logic)
        /// Format: Entity, Sequence, Field, Value
        /// </summary>
        public List<Tuple<string, int, string, string>> RawValues { get; set; }

        public string ServiceType { get; set; }

        public string Source { get; set; }

        public bool Removed { get; set; }

        private bool enabled = true;

        /// <summary>
        /// Determines if this is supported
        /// </summary>
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                if (parent != null && !enabled) parent.Enabled = enabled;
            }
        }

        private string disabledReason = string.Empty;

        /// <summary>
        /// Reason why this isn't supported
        /// </summary>
        public string DisabledReason
        {
            get
            {
                return disabledReason;
            }
            set
            {
                disabledReason = value;
                if (parent != null && !enabled) parent.DisabledReason = value;
            }
        }

        public void AddOrUpdate(string key, object value)
        {
            if (this.ContainsKey(key)) this[key] = value;
            else this.Add(key, value);
        }

        /// <summary>
        /// For js call
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void SetItem(string name, string val)
        {
            if (string.Equals(name, "enabled", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(val, "false", StringComparison.OrdinalIgnoreCase))
                    this.Enabled = false;
                else
                    this.Enabled = true;
            }
            else if (string.Equals(name, "removed", StringComparison.OrdinalIgnoreCase))
            {
                if (this.parent != null)
                {
                    if (string.Equals(val, "false", StringComparison.OrdinalIgnoreCase))
                        this.Removed = false;
                    else
                        this.Removed = true;
                }
            }
            if (this.ContainsKey(name)) this[name] = val;
            else this.Add(name, val);
        }

        /// <summary>
        /// For js Call 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetItem(string name)
        {
            if (this.ContainsKey(name) && this[name].GetType() == name.GetType()) return this[name].ToString();
            else return string.Empty;
        }

        /// <summary>
        /// For js call
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetListCount(string name)
        {
            if (this.ContainsKey(name) && this[name].GetType() == typeof(List<ServiceRow>))
                return ((List<ServiceRow>)this[name]).Count;
            else
                return 0;
        }

        /// <summary>
        /// For js call
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ServiceRow GetListItem(string name, int index)
        {
            if (this.ContainsKey(name) && this[name].GetType() == typeof(List<ServiceRow>))
                return ((List<ServiceRow>)this[name])[index];
            else
                return null;
        }

        /// <summary>
        /// For js call
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<ServiceRow> GetList(string name)
        {
            if (this.ContainsKey(name) && this[name].GetType() == typeof(List<ServiceRow>)) return this[name] as List<ServiceRow>;
            else return new List<ServiceRow>();

        }

        /// <summary>
        /// For js call
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ServiceRow GetParent()
        {
            if (parent == null) return this;
            else return parent;
        }

    }
}
