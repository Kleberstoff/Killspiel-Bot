﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace RiotSharp.Endpoints.LeagueEndpoint.Enums.Converters
{
	internal class TierConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return typeof(string).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
			JsonSerializer serializer)
		{
			var token = JToken.Load(reader);
			var str = token.Value<string>();
			if (str == null) return null;

			if (Enum.TryParse<Tier>(str, true, out var result))
			{
				return result;
			}
			return null;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			serializer.Serialize(writer, ((Tier)value).ToString().ToUpper());
		}
	}
}