using System;
using UnityEngine;

namespace Attributes
{
	/// <summary>
	///   <para>Use this attribute on serialized fields to show the value as readonly.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ReadOnlyAttribute : PropertyAttribute
	{
	}
}