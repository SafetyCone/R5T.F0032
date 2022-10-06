using System;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using R5T.T0132;


namespace R5T.F0032
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Prior work in R5T.L0019.X001.
    /// </remarks>
    [FunctionalityMarker]
    public partial interface IJsonOperator : IDraftFunctionalityMarker
    {
        private static Internal.IJsonOperator Internal { get; } = F0032.Internal.JsonOperator.Instance;


        public async Task<T> LoadFromFile<T>(string jsonFilePath, string keyName)
        {
            var jObject = await Internal.LoadAsJObject(jsonFilePath);

            var keyedJObject = jObject[keyName];

            var output = keyedJObject.ToObject<T>();
            return output;
        }

        public T LoadFromFile_Synchronous<T>(string jsonFilePath, string keyName)
        {
            var jObject = Internal.LoadAsJObject_Synchronous(jsonFilePath);

            var keyedJObject = jObject[keyName];

            var output = keyedJObject.ToObject<T>();
            return output;
        }

        public void SaveToFile<T>(string jsonFilePath, T @object, bool overwrite = true)
        {
            var jsonSerializer = Internal.GetJsonSerializer();

            this.Serialize_Synchronous(jsonSerializer, jsonFilePath, @object, overwrite);
        }

        /// <summary>
        /// Serialize an object to a file.
        /// <inheritdoc cref="Documentation.LoadMeansRootObjectUsesKey" path="/summary"/>
        /// </summary>
        public void SaveToFile_Synchronous<T>(string jsonFilePath, string keyName, T value, bool overwrite = true)
        {
            var jObject = new JObject
            {
                { keyName, JObject.FromObject(value) },
            };

            this.Serialize_Synchronous(jsonFilePath, jObject, overwrite);
        }

        /// <summary>
        /// Loads an object from a file, using the type name of the type parameter as the key.
        /// <type-name-is-key>The type name of the type parameters is used as the top-level key.</type-name-is-key>
        /// </summary>
        public T LoadFromFile_Synchronous<T>(string jsonFilePath)
        {
            var keyName = F0000.TypeOperator.Instance.GetNameOf<T>();

            var @object = this.LoadFromFile_Synchronous<T>(jsonFilePath, keyName);
            return @object;
        }

        /// <summary>
        /// Serialize an object to a file.
        /// <only-properties-have-keys>The object itself will not have a key in the JSON file. It's properties will have keys, but it will not.</only-properties-have-keys>
        /// </summary>
        public void Serialize_Synchronous<T>(JsonSerializer jsonSerializer, string jsonFilePath, T value, bool overwrite = true)
        {
            using var fileStream = F0000.FileStreamOperator.Instance.NewWrite(jsonFilePath, overwrite);
            using var streamWriter = new StreamWriter(fileStream);

            jsonSerializer.Serialize(streamWriter, value);
        }

        /// <summary>
        /// Serialize an object to a file.
        /// <inheritdoc cref="Serialize_Synchronous{T}(JsonSerializer, string, T, bool)" path="/summary/only-properties-have-keys"/>
        /// </summary>
        public void Serialize_Synchronous<T>(string jsonFilePath, T value, bool overwrite = true)
        {
            var jsonSerializer = Internal.GetJsonSerializer();

            this.Serialize_Synchronous<T>(jsonSerializer, jsonFilePath, value, overwrite);
        }

        public void Serialize_Synchronous(string jsonFilePath, JObject jObject, bool overwrite = true)
        {
            using var fileStream = F0000.FileStreamOperator.Instance.NewWrite(jsonFilePath, overwrite);
            using var streamWriter = new StreamWriter(fileStream);

            var jsonSerializer = Internal.GetJsonSerializer();

            jsonSerializer.Serialize(streamWriter, jObject);
        }

        /// <summary>
        /// Deserialize an object from a file.
        /// <inheritdoc cref="Serialize_Synchronous{T}(JsonSerializer, string, T, bool)" path="/summary/only-properties-have-keys"/>
        /// </summary>
        public T Deserialize_Synchronous<T>(JsonSerializer jsonSerializer, string jsonFilePath)
        {
            using var textReader = File.OpenText(jsonFilePath);
            using var jsonReader = new JsonTextReader(textReader);

            var value = jsonSerializer.Deserialize<T>(jsonReader);
            return value;
        }

        /// <summary>
        /// Deserialize an object from a file.
        /// <inheritdoc cref="Serialize_Synchronous{T}(JsonSerializer, string, T, bool)" path="/summary/only-properties-have-keys"/>
        /// </summary>
        public T Deserialize_Synchronous<T>(string jsonFilePath)
        {
            var jsonSerializer = Internal.GetJsonSerializer();

            var output = this.Deserialize_Synchronous<T>(
                jsonSerializer,
                jsonFilePath);

            return output;
        }
    }


    namespace Internal
    {
        public partial interface IJsonOperator : IDraftFunctionalityMarker
        {
            public JsonSerializer GetJsonSerializer(Formatting formatting = Formatting.Indented)
            {
                var jsonSerializer = new JsonSerializer
                {
                    Formatting = formatting,
                };

                return jsonSerializer;
            }

            public async Task<JObject> LoadAsJObject(string jsonFilePath)
            {
                using var streamReader = F0002.Instances.StreamReaderOperator.GetNew(jsonFilePath);
                using var jsonTextReader = new JsonTextReader(streamReader);

                var output = await JObject.LoadAsync(jsonTextReader);
                return output;
            }

            public JObject LoadAsJObject_Synchronous(string jsonFilePath)
            {
                using var streamReader = F0002.Instances.StreamReaderOperator.GetNew(jsonFilePath);
                using var jsonTextReader = new JsonTextReader(streamReader);

                var output = JObject.Load(jsonTextReader);
                return output;
            }
        }
    }
}