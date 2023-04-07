using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.SerializableDictionary;

namespace Structs.Audio
{
	[Serializable]
	public struct EventParameters : IEnumerable<KeyValuePair<string, float>>
	{
		[SerializeField]
		private SerializableDictionary<string, float> parameters;

		/// <summary>
		/// Get a copy of the Parameters dictionary
		/// </summary>
		public Dictionary<string, float> Parameters => new Dictionary<string, float>(parameters);
		
		public EventParameters(params KeyValuePair<string, float>[] values)
		{
			parameters = new SerializableDictionary<string, float>();
			
			foreach (KeyValuePair<string, float> pair in values)
			{
				parameters.Add(pair.Key, pair.Value);
			}
		}

		public float this[string paramName]
		{
			get => parameters[paramName];
			set => parameters[paramName] = value;
		}

		public int Count => parameters.Count;
		
		public void AddParameter(string parameterName, float parameterValue)
		{
			parameters.Add(parameterName, parameterValue);
		}

		public void ChangeParameterValue(string parameterName, float parameterValue)
		{
			parameters[parameterName] = parameterValue;
		}

		public void RemoveParameter(string parameterName)
		{
			parameters.Remove(parameterName);
		}

		public float GetParameterValue(string parameterName)
		{
			return parameters[parameterName];
		}

		public IEnumerator<KeyValuePair<string, float>> GetEnumerator() => parameters.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}