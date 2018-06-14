using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A simple scriptable object that defines which other alignments it can harm. It can never harm
/// any other alignment that's not a SimpleAlignment
/// </summary>
[CreateAssetMenu(fileName = "Alignment.asset", menuName = "StarterKit/Simple Alignment", order = 1)]
public class SimpleAlignment : ScriptableObject, IAlignmentProvider
{
	/// <summary>
	/// A collection of other alignment objects that we can harm
	/// </summary>
	public List<SimpleAlignment> opponents;

	/// <summary>
	/// Gets whether the given alignment is in our known list of opponents
	/// </summary>
	public bool CanHarm(IAlignmentProvider other)
	{
		if (other == null)
		{
			return true;
		}
			
		var otherAlignment = other as SimpleAlignment;
			
		return otherAlignment != null && opponents.Contains(otherAlignment);
	}
}

/// <summary>
/// Abstract base for serializable interface wrapper objects
/// </summary>
public abstract class SerializableInterface
{
	/// <summary>
	/// Unity component that gets serialized that is of our interface type
	/// </summary>
	public UnityEngine.Object unityObjectReference;
}

/// <summary>
/// A generic solution to allow the serialization of interfaces in Unity game objects
/// </summary>
/// <typeparam name="T">Any interface implementing ISerializableInterface</typeparam>
[Serializable]
public class SerializableInterface<T> : SerializableInterface where T: ISerializableInterface
{
	T m_InterfaceReference;
		
	/// <summary>
	/// Retrieves the interface from the unity component and caches it
	/// </summary>
	public T GetInterface()
	{
		if (m_InterfaceReference == null && unityObjectReference != null)
		{
			m_InterfaceReference = (T)(ISerializableInterface)unityObjectReference;
		}

		return m_InterfaceReference;
	}
}

/// <summary>
/// Base interface from which all serializable interfaces must derive
/// </summary>
public interface ISerializableInterface
{
}

/// <summary>
/// An interface for objects which can provide a team/alignment for damage purposes
/// </summary>
public interface IAlignmentProvider : ISerializableInterface
{
	/// <summary>
	/// Gets whether this alignment can harm another
	/// </summary>
	bool CanHarm(IAlignmentProvider other);
}

/// <summary>
/// Concrete serializable version of interface above
/// </summary>
[Serializable]
public class SerializableIAlignmentProvider : SerializableInterface<IAlignmentProvider>
{
}

