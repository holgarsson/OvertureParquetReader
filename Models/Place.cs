using System.Collections.Generic;

namespace OvertureParquetReader.Models
{
    /// <summary>
    /// Represents a place in the Overture Maps schema.
    /// </summary>
    public class Place
    {
        /// <summary>
        /// The unique identifier of the place.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// The addresses associated with this place.
        /// </summary>
        public List<Address> Addresses { get; set; } = new();

        /// <summary>
        /// The primary category of the place.
        /// </summary>
        public string? CategoryPrimary { get; set; }

        /// <summary>
        /// Alternate categories of the place.
        /// </summary>
        public List<string>? CategoryAlternates { get; set; }

        /// <summary>
        /// The confidence of the existence of the place (0-1).
        /// </summary>
        public double? Confidence { get; set; }

        /// <summary>
        /// The websites of the place.
        /// </summary>
        public List<string>? Websites { get; set; }

        /// <summary>
        /// The social media URLs of the place.
        /// </summary>
        public List<string>? Socials { get; set; }

        /// <summary>
        /// The email addresses of the place.
        /// </summary>
        public List<string>? Emails { get; set; }

        /// <summary>
        /// The phone numbers of the place.
        /// </summary>
        public List<string>? Phones { get; set; }

        /// <summary>
        /// The brand name of the place.
        /// </summary>
        public string? BrandName { get; set; }

        /// <summary>
        /// The Wikidata ID of the brand.
        /// </summary>
        public string? WikidataId { get; set; }

        /// <summary>
        /// The primary name of the place.
        /// </summary>
        public string? NamePrimary { get; set; }

        /// <summary>
        /// Common translations of the place name.
        /// </summary>
        public Dictionary<string, string>? NameCommon { get; set; }
    }

    /// <summary>
    /// Represents an address in the Overture Maps schema.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Free-form address that contains street name, house number and other address info.
        /// </summary>
        public string? Freeform { get; set; }

        /// <summary>
        /// Name of the city or neighborhood where the address is located.
        /// </summary>
        public string? Locality { get; set; }

        /// <summary>
        /// Postal code where the address is located.
        /// </summary>
        public string? Postcode { get; set; }

        /// <summary>
        /// ISO 3166-2 principal subdivision code (region).
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// ISO 3166-1 alpha-2 country code.
        /// </summary>
        public string? Country { get; set; }
    }
}