using System;
using System.Collections.Generic;
using System.Dynamic;

namespace YouTrackSharp.Admin
{
    public class State : DynamicObject
    {
        readonly IDictionary<string, object> _allFields = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        string _id;

        public string Id
        {
            get { return _id ?? (_id = (string)_allFields["id"]); }
        }

        public ExpandoObject ToExpandoObject()
        {
            IDictionary<string, object> expando = new ExpandoObject();


            foreach (dynamic field in _allFields)
            {
                if (String.Compare(field.Key, "ProjectShortName", StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    expando.Add("project", field.Value);
                }
                else if (String.Compare(field.Key, "permittedGroup", StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    expando.Add("permittedGroup", field.Value);
                }
                else
                {
                    expando.Add(field.Key.ToLower(), field.Value);
                }
            }
            return (ExpandoObject)expando;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_allFields.ContainsKey(binder.Name))
            {
                result = _allFields[binder.Name];
                return true;
            }
            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (String.Compare(binder.Name, "field", StringComparison.OrdinalIgnoreCase) == 0)
            {
                foreach (var val in (IEnumerable<dynamic>)value)
                {
                    _allFields[val.name] = val.value;
                }
                return true;
            }
            _allFields[binder.Name] = value;
            return true;
        }
    }
}