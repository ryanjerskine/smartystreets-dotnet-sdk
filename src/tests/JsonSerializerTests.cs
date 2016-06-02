﻿using System;
using System.Runtime.Serialization;
using NUnit.Framework;
using System.Text;
using System.IO;

namespace SmartyStreets
{
	[TestFixture]
	public class JsonSerializerTests
	{
		private readonly ISerializer serializer = new JsonSerializer();

		[Test]
		public void TestSerializationOfNullValues()
		{
			var results = this.serializer.Serialize(null);

			Assert.IsNull(results);
		}

		[Test]
		public void TestSerializationOfKnownType()
		{
			var results = this.serializer.Serialize(new JsonSerializerTestObject() {
				Property1 = "Name",
				Property2 = 42,
				Property3 = true,
			});

			Assert.AreEqual(Encoding.UTF8.GetString(results), "{'Property2':42,'Property3':true,'property_1':'Name'}".Replace("'", "\""));
		}

		[Test]
		public void TestDeserializationOfNullStream()
		{
			var result = this.serializer.Deserialize<JsonSerializerTestObject>(null);

			Assert.IsNull(result);
		}

		[Test]
		public void TestDeserializationOfKnownType()
		{
			var expected = new JsonSerializerTestObject() {
				Property1 = "Name",
				Property2 = 42,
				Property3 = true,
			};

			var stream = new MemoryStream(Encoding.UTF8.GetBytes("{'Property2':42,'Property3':true,'property_1':'Name'}".Replace("'", "\"")));
			var actual = this.serializer.Deserialize<JsonSerializerTestObject>(stream);

			Assert.AreEqual(expected.Property1, actual.Property1);
			Assert.AreEqual(expected.Property2, actual.Property2);
			Assert.AreEqual(expected.Property3, actual.Property3);
		}
	}

	[DataContract]
	public class JsonSerializerTestObject {

		[DataMember(Name = "property_1")]
		public string Property1 { get; set; }

		[DataMember(Name = "Property2")]
		public int Property2 { get; set; }

		[DataMember(Name = "Property3")]
		public bool Property3 { get; set; }
	}
}