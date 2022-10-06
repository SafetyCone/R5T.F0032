using System;

using Newtonsoft.Json.Linq;

using R5T.T0141;


namespace R5T.F0032.Q000
{
	[ExplorationsMarker]
	public partial interface IExplorations : IExplorationsMarker
	{
		public void SerializeJsonObject()
        {
            var jObject = new JObject
            {
                { "TestKey", 3 }
            };

            Instances.JsonOperator.Serialize(@"C:\Temp\temp.json", jObject);
        }
	}
}