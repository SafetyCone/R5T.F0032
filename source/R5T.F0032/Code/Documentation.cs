using System;


namespace R5T.F0032
{
	/// <summary>
	/// JSON functionality (JsonOperator, JSON.NET-based).
	/// </summary>
	public static class Documentation
	{
		/// <summary>
		/// Loading an object from a JSON file means the object *is* identified by a key, and is not the "file itself".
		/// </summary>
		public static readonly object LoadMeansRootObjectUsesKey;

		/// <summary>
		/// Deserializing an object from a JSON file means the object is not identified by a key, and *is* the "file itself".
		/// This is to say, the properties of the object will have keys in the file, but the object itself will not; it will be the root object.
		/// </summary>
		public static readonly object DeserializeMeansRootObjectHasNoKey;
	}
}