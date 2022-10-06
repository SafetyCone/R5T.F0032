using System;


namespace R5T.F0032
{
	public class JsonOperator : IJsonOperator
	{
		#region Infrastructure

	    public static IJsonOperator Instance { get; } = new JsonOperator();

	    private JsonOperator()
	    {
        }

	    #endregion
	}


	namespace Internal
    {
		public class JsonOperator : IJsonOperator
		{
			#region Infrastructure

			public static IJsonOperator Instance { get; } = new JsonOperator();

			private JsonOperator()
			{
			}

			#endregion
		}
	}
}