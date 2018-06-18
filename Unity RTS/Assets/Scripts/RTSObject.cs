using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSObject : MonoBehaviour
{
	/// <summary>
	/// The alignment of the damager
	/// </summary>
	public SerializableIAlignmentProvider alignment;
	
	/// <summary>
	/// Gets the <see cref="IAlignmentProvider"/> of this instance
	/// </summary>
	public IAlignmentProvider alignmentProvider
	{
		get
		{
			return alignment != null ? alignment.GetInterface() : null;
		}
	}
}
