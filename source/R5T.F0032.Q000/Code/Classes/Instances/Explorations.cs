using System;


namespace R5T.F0032.Q000
{
	public class Explorations : IExplorations
	{
		#region Infrastructure

	    public static IExplorations Instance { get; } = new Explorations();

	    private Explorations()
	    {
        }

	    #endregion
	}
}