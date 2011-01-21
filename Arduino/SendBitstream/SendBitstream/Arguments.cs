// Copyright (C) Prototype Engineering, LLC. All rights reserved.

using System;
using System.Linq;

namespace SendBitstream
{
    public class Arguments
    {
        public static void Parse(string[] args, object target)
        {
            for (int counter = 0; counter < args.Length; ++counter)
            {
                if (args[counter].StartsWith("/"))
                {
                    var argumentName = args[counter].Substring(1);
                    var argumentValue = args[counter + 1];
                    object finalValue = argumentValue;

                    var propertyAndAttribute = (from p in target.GetType().GetProperties()
                                                from a in p.GetCustomAttributes(typeof(ArgumentAttribute), true)
                                                where (((ArgumentAttribute)a).Name == argumentName)
                                                select new { Property = p, Attribute = a }).First();

                    if (propertyAndAttribute.Property.PropertyType != typeof(string))
                    {
                        finalValue = propertyAndAttribute.Property.PropertyType.GetMethod("Parse", new Type[] { typeof(string) } ).Invoke(null, new object[] { argumentValue });
                    }
                    propertyAndAttribute.Property.SetValue(target, finalValue, null);
                }
            }
        }
    }
}
