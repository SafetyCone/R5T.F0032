using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


        public T ParseFromJsonText<T>(string jsonText, string keyName)
        {
            var jObject = Internal.ParseAsJObject(jsonText);

            var keyedJObject = jObject[keyName];

            var output = keyedJObject.ToObject<T>();
            return output;
        }

        /// <summary>
        /// <inheritdoc cref="Documentation.LoadMeansRootObjectUsesKey" path="/summary"/>
        /// </summary>
        public async Task<T> LoadFromFile<T>(string jsonFilePath, string keyName)
        {
            var jObject = await Internal.LoadAsJObject(jsonFilePath);

            var keyedJObject = jObject[keyName];

            var output = keyedJObject.ToObject<T>();
            return output;
        }

        /// <summary>
        /// <inheritdoc cref="Documentation.LoadMeansRootObjectUsesKey" path="/summary"/>
        /// </summary>
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
        /// <inheritdoc cref="Documentation.LoadMeansRootObjectUsesKey" path="/summary"/>
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

        public async Task Serialize<T>(JsonSerializer jsonSerializer, string jsonFilePath, T value, bool overwrite = true)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream)
            {
                AutoFlush = true,
            };

            jsonSerializer.Serialize(streamWriter, value);

            // Reset.
            memoryStream.Seek(0, SeekOrigin.Begin);

            using var fileStream = F0000.FileStreamOperator.Instance.NewWrite(jsonFilePath, overwrite);
            
            await memoryStream.CopyToAsync(fileStream);
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

        public Task Serialize<T>(string jsonFilePath, T value, bool overwrite = true)
        {
            var jsonSerializer = Internal.GetJsonSerializer();

            return this.Serialize(jsonSerializer, jsonFilePath, value, overwrite);
        }

        public JObject SerializeToJObject<T>(T value)
        {
            var jObject = JObject.FromObject(value);
            return jObject;
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
        public async Task<T> Deserialize<T>(JsonSerializer jsonSerializer, string jsonFilePath)
        {
            var memoryStream = await Instances.MemoryStreamOperator.FromFile(jsonFilePath);

            using var textReader = F0000.StreamReaderOperator.Instance.From(memoryStream);
            using var jsonReader = new JsonTextReader(textReader);

            var value = jsonSerializer.Deserialize<T>(jsonReader);
            return value;
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
        /// Deserialize an object from a stream.
        /// <inheritdoc cref="Serialize_Synchronous{T}(JsonSerializer, string, T, bool)" path="/summary/only-properties-have-keys"/>
        /// </summary>
        public T Deserialize_Synchronous<T>(JsonSerializer jsonSerializer, Stream stream)
        {
            using var textReader = F0000.StreamReaderOperator.Instance.From(stream);
            using var jsonReader = new JsonTextReader(textReader);

            var value = jsonSerializer.Deserialize<T>(jsonReader);
            return value;
        }

        /// <inheritdoc cref="Deserialize{T}(string)"/>
        public T Deserialize_Synchronous<T>(string jsonFilePath)
        {
            var jsonSerializer = Internal.GetJsonSerializer();

            var output = this.Deserialize_Synchronous<T>(
                jsonSerializer,
                jsonFilePath);

            return output;
        }

        /// <inheritdoc cref="Deserialize{T}(string)"/>
        public T Deserialize_Synchronous<T>(Stream stream)
        {
            var jsonSerializer = Internal.GetJsonSerializer();

            var output = this.Deserialize_Synchronous<T>(
                jsonSerializer,
                stream);

            return output;
        }

        /// <summary>
        /// Deserialize an object from a file.
        /// <inheritdoc cref="Serialize_Synchronous{T}(JsonSerializer, string, T, bool)" path="/summary/only-properties-have-keys"/>
        /// </summary>
        public Task<T> Deserialize<T>(string jsonFilePath)
        {
            var jsonSerializer = Internal.GetJsonSerializer();

            return this.Deserialize<T>(
                jsonSerializer,
                jsonFilePath);
        }

        public object Deserialize_FromJObject(Type type, JObject jObject)
        {
            var output = jObject.ToObject(type);
            return output;
        }

        /// <summary>
        /// Deserialize the JObject as the <paramref name="type"/>, but then cast to the <typeparamref name="T"/> type.
        /// </summary>
        public T Deserialize_FromJObjectAs<T>(Type type, JObject jObject)
            where T : class
        {
            var output = jObject.ToObject(type) as T;
            return output;
        }

        public T Deserialize_FromJObject<T>(JObject jObject)
        {
            var output = jObject.ToObject<T>();
            return output;
        }

        public IEnumerable<T> DeserializeSerializationTypes<T>(
            Type[] types,
            IEnumerable<SerializationType> serializationTypes)
            where T : class
        {
            var typesByTypeFullName = types
                .ToDictionary(
                    type => type.FullName,
                    type => type);

            var output = serializationTypes
                .Select(x =>
                {
                    var typeForInstance = typesByTypeFullName[x.TypeName];

                    var instance = Instances.JsonOperator.Deserialize_FromJObjectAs<T>(
                        typeForInstance,
                        x.Object);

                    return instance;
                });

            return output;
        }
    }


    namespace Internal
    {
        public partial interface IJsonOperator : IDraftFunctionalityMarker
        {
            public JObject ParseAsJObject(string jsonText)
            {
                var output = JObject.Parse(jsonText);
                return output;
            }

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