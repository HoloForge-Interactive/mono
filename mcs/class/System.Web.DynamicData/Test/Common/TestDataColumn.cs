﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using MonoTests.DataSource;
using MonoTests.ModelProviders;

namespace MonoTests.Common
{
	public class TestDataColumn <DataType> : DynamicDataColumn
	{
		public TestDataColumn (MemberInfo member)
		{
			if (member == null)
				throw new ArgumentNullException ("member");

			string name = this.Name = member.Name;
			switch (member.MemberType) {
				case MemberTypes.Field:
					var fi = member as FieldInfo;
					this.DataType = fi.FieldType;
					break;

				case MemberTypes.Property:
					var pi = member as PropertyInfo;
					this.DataType = pi.PropertyType;
					break;

				default:
					throw new ArgumentException ("Member information must refer to either a field or a property.", "member");
			}

			this.PrimaryKey = name.StartsWith ("PrimaryKeyColumn", StringComparison.Ordinal);
			object[] attrs = member.GetCustomAttributes (typeof (DynamicDataAssociationAttribute), true);
			if (attrs != null && attrs.Length > 0) {
				var attr = attrs[0] as DynamicDataAssociationAttribute;
				if (attr != null) {
					AssociatedTo = attr.ColumnName;
					AssociationDirection = attr.Direction;
				}
			}
		}
	}
}
