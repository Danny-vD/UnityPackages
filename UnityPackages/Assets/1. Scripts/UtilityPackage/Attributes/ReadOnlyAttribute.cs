using System;
using UnityEngine;

namespace Attributes
{
	/// <summary>
	///   <para>Prevent this field from being edited in the inspector.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ReadOnlyAttribute : PropertyAttribute
	{
	}
}