using System;
using System.Linq;

using Newtonsoft.Json.Linq;

using R5T.T0140;
using R5T.T0141;


namespace R5T.F0032.Q000
{
	[ExplorationsMarker]
	public partial interface IExplorations : IExplorationsMarker
	{
        public void DeserializeInterface02sFromJObjects()
        {
            var serializationTypes = Instances.JsonOperator.Deserialize_Synchronous<SerializationType[]>(
                Instances.FilePaths.OutputJsonFilePath);

            var interface02s = serializationTypes
                .Select(x =>
                {
                    IInterface02 output;

                    if (x.TypeName == typeof(Class04).FullName)
                    {
                        output = Instances.JsonOperator.Deserialize_FromJObject<Class04>(x.Object);
                        return output;
                    }

                    if (x.TypeName == typeof(Class05).FullName)
                    {
                        output = Instances.JsonOperator.Deserialize_FromJObject<Class05>(x.Object);
                        return output;
                    }

                    throw new Exception($"Unrecognized type name: {x.TypeName}");
                })
                .Now();
        }

        /// <summary>
        /// Since serialization of instances with JObject properties was a success, is deserialization?
        /// Result: Yes!
        /// </summary>
        public void DeserializeInterfaceInstancesFromJObjects()
        {
            var serializationTypes =  Instances.JsonOperator.Deserialize_Synchronous<SerializationType[]>(
                Instances.FilePaths.OutputJsonFilePath);

            Console.WriteLine(serializationTypes);
        }

        /// <summary>
        /// See if it's possible to serialize interface instances as instances of a type that identifies the implementation type, but also includes the data as a JObject.
        /// Result: Yes, yes it is!
        /// </summary>
        public void SerializeInterfaceInstancesAsJObject()
        {
            var interface02s = new IInterface02[]
            {
                new Class04
                {
                    Integer = 1,
                    String = "Class04",
                },
                new Class05
                {
                    Double = 2.0,
                    String = "Class05",
                }
            };

            var serializationTypes = interface02s
                .Select(x =>
                {
                    var typeName = x.GetType().FullName;

                    var jObject = Instances.JsonOperator.SerializeToJObject(x);

                    var serializationType = new SerializationType
                    {
                        TypeName = typeName,
                        Object = jObject,
                    };

                    return serializationType;
                })
                .Now();

            Instances.JsonOperator.Serialize_Synchronous(
                Instances.FilePaths.OutputJsonFilePath,
                serializationTypes);

            Instances.NotepadPlusPlusOperator.Open(
                Instances.FilePaths.OutputJsonFilePath);
        }

        /// <summary>
        /// See if it's possible to serialize interface instances.
        /// Result: serialization is a success!
        /// </summary>
        public void SerializeInterfaceInstances()
        {
            var interface02s = new IInterface02[]
            {
                new Class04
                {
                    Integer = 1,
                    String = "Class04",
                },
                new Class05
                {
                    Double = 2.0,
                    String = "Class05",
                }
            };

            Instances.JsonOperator.Serialize_Synchronous(
                Instances.FilePaths.OutputJsonFilePath,
                interface02s);

            //var serializationTypes = interface02s
            //    .Select(x =>
            //    {
            //        var typeName = x.GetType().FullName;

            //        var jObject = Instances.JsonOperator.SerializeToJObject(x);

            //        var serializationType = new SerializationType
            //        {
            //            TypeName = typeName,
            //            Object = jObject,
            //        };

            //        return serializationType;
            //    })
            //    .Now();

            //Instances.JsonOperator.Serialize_Synchronous(
            //    Instances.FilePaths.OutputJsonFilePath,
            //    serializationTypes);

            Instances.NotepadPlusPlusOperator.Open(
                Instances.FilePaths.OutputJsonFilePath);
        }

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