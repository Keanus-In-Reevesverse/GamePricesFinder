namespace GamePriceFinder.Responses
{
    /// <summary>
    /// Represents the response from http request to PlaystationNetwork API.
    /// </summary>
    public class PsnResponse
    {
        public int age_limit { get; set; }
        public Attributes attributes { get; set; }
        public int content_origin { get; set; }
        public bool dob_required { get; set; }
        public object[] images { get; set; }
        public Link[] links { get; set; }
        public Metadata metadata { get; set; }
        public object[] promomedia { get; set; }
        public bool restricted { get; set; }
        public int revision { get; set; }
        public string searchExperimentId { get; set; }
        public int size { get; set; }
        public object[] sku_links { get; set; }
        public int start { get; set; }
        public int total_results { get; set; }
        public int total_suggested { get; set; }
    }

    public class Facets
    {
    }

    public class Attributes
    {
        public Facets facets { get; set; }
        public string[] next { get; set; }
    }

    public class Metadata
    {
    }

    public class Link
    {
        public string[] activeLocales { get; set; }
        public string bucket { get; set; }
        public string container_type { get; set; }
        public string content_type { get; set; }
        public Default_Sku default_sku { get; set; }
        public Gamecontenttypeslist[] gameContentTypesList { get; set; }
        public string game_contentType { get; set; }
        public string id { get; set; }
        public Image[] images { get; set; }
        public bool is_child { get; set; }
        public bool is_suggested { get; set; }
        public Metadata2 metadata { get; set; }
        public string name { get; set; }
        public string[] playable_platform { get; set; }
        public Position[] positions { get; set; }
        public string provider_name { get; set; }
        public DateTime release_date { get; set; }
        public bool restricted { get; set; }
        public int revision { get; set; }
        public string short_name { get; set; }
        public long timestamp { get; set; }
        public string top_category { get; set; }
        public string url { get; set; }
    }

    public class Default_Sku
    {
        public bool amortizeFlag { get; set; }
        public bool bundleExclusiveFlag { get; set; }
        public bool chargeImmediatelyFlag { get; set; }
        public int charge_type_id { get; set; }
        public int credit_card_required_flag { get; set; }
        public bool defaultSku { get; set; }
        public string display_price { get; set; }
        public object[] eligibilities { get; set; }
        public Entitlement[] entitlements { get; set; }
        public string id { get; set; }
        public bool is_original { get; set; }
        public string name { get; set; }
        public int[] platforms { get; set; }
        public int price { get; set; }
        public object[] rewards { get; set; }
        public bool seasonPassExclusiveFlag { get; set; }
        public bool skuAvailabilityOverrideFlag { get; set; }
        public int sku_type { get; set; }
        public string type { get; set; }
    }

    public class Entitlement
    {
        public object description { get; set; }
        public object[] drms { get; set; }
        public int duration { get; set; }
        public object durationOverrideTypeId { get; set; }
        public int exp_after_first_use { get; set; }
        public int feature_type_id { get; set; }
        public string id { get; set; }
        public int license_type { get; set; }
        public Metadata1 metadata { get; set; }
        public string name { get; set; }
        public string packageType { get; set; }
        public Package[] packages { get; set; }
        public bool preorder_placeholder_flag { get; set; }
        public int size { get; set; }
        public int subType { get; set; }
        public string[] subtitle_language_codes { get; set; }
        public int type { get; set; }
        public int use_count { get; set; }
        public string[] voice_language_codes { get; set; }
    }

    public class Metadata1
    {
        public string[] voiceLanguageCode { get; set; }
        public string[] subtitleLanguageCode { get; set; }
    }

    public class Package
    {
        public int platformId { get; set; }
        public string platformName { get; set; }
        public long size { get; set; }
    }

    public class Metadata2
    {
        public Cn_Remoteplay cn_remotePlay { get; set; }
        public Secondary_Classification secondary_classification { get; set; }
        public Game_Subgenre game_subgenre { get; set; }
        public Cn_Numberofnetworkplayerspsplus cn_numberOfNetworkPlayersPSPlus { get; set; }
        public Game_Genre game_genre { get; set; }
        public Playable_Platform playable_platform { get; set; }
        public Cn_Numberofplayers cn_numberOfPlayers { get; set; }
        public Subgenre subgenre { get; set; }
        public Tertiary_Classification tertiary_classification { get; set; }
        public Container_Type container_type { get; set; }
        public PsnGenre genre { get; set; }
        public Cn_Ingamepurchases cn_inGamePurchases { get; set; }
        public Cn_Singstarmicrophone cn_singstarMicrophone { get; set; }
        public Primary_Classification primary_classification { get; set; }
    }

    public class Cn_Remoteplay
    {
        public string name { get; set; }
        public string[] values { get; set; }
    }

    public class Secondary_Classification
    {
        public string name { get; set; }
        public string[] values { get; set; }
    }

    public class Game_Subgenre
    {
        public string name { get; set; }
        public string[] values { get; set; }
    }

    public class Cn_Numberofnetworkplayerspsplus
    {
        public string name { get; set; }
        public string[] values { get; set; }
    }

    public class Game_Genre
    {
        public string name { get; set; }
        public string[] values { get; set; }
    }

    public class Playable_Platform
    {
        public string name { get; set; }
        public string[] values { get; set; }
    }

    public class Cn_Numberofplayers
    {
        public string name { get; set; }
        public string[] values { get; set; }
    }

    public class Subgenre
    {
        public string name { get; set; }
        public string[] values { get; set; }
    }

    public class Tertiary_Classification
    {
        public string name { get; set; }
        public string[] values { get; set; }
    }

    public class Container_Type
    {
        public string name { get; set; }
        public string[] values { get; set; }
    }

    public class PsnGenre
    {
        public string name { get; set; }
        public string[] values { get; set; }
    }

    public class Cn_Ingamepurchases
    {
        public string name { get; set; }
        public string[] values { get; set; }
    }

    public class Cn_Singstarmicrophone
    {
        public string name { get; set; }
        public string[] values { get; set; }
    }

    public class Primary_Classification
    {
        public string name { get; set; }
        public string[] values { get; set; }
    }

    public class Gamecontenttypeslist
    {
        public string name { get; set; }
        public string key { get; set; }
    }

    public class Image
    {
        public int type { get; set; }
        public string url { get; set; }
    }

    public class Position
    {
        public int start { get; set; }
        public int end { get; set; }
    }
}
