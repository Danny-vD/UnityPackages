using System;
using UnityEngine;

namespace Attributes
{
	/// <summary>
	///   <para>Use this attribute on serialized fields to change the display name shown in the Inspector.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class DisplayNameAttribute : PropertyAttribute
	{
		/// <summary>
		///   <para>Name to display in the Inspector.</para>
		/// </summary>
		public readonly string DisplayName;
		
		/// <summary>
		///   <para>Specify a display name for a field.</para>
		/// </summary>
		/// <param name="newName">The name to display.</param>
		public DisplayNameAttribute(string newName)
		{
			DisplayName = newName;
		}
	}
}