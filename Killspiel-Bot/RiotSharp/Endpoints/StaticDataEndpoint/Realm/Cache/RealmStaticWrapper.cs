using Newtonsoft.Json;

namespace RiotSharp.Endpoints.StaticDataEndpoint.Realm.Cache
{
	internal class RealmStaticWrapper
	{
		[JsonProperty]
		public RealmStatic RealmStatic { get; private set; }

		public RealmStaticWrapper(RealmStatic realm)
		{
			RealmStatic = realm;
		}
	}
}