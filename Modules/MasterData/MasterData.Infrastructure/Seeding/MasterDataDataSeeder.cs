namespace MasterData.Infrastructure.Seeding
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public interface IMasterDataDataSeeder
    {
        void Seed(DbContext dbContext);
    }

    /// <summary>
    /// MasterData's slice of the legacy InitialData seed (PaymentType, ReferenceType, Currency,
    /// Country/City/District reference data for EG/SA/AE/KW/QA/BH/OM/SD/LY/TN/DZ/MA/MR) —
    /// relocated verbatim, split by module ownership. See
    /// Administration.Infrastructure.Seeding.AdministrationDataSeeder for the shared rationale.
    /// </summary>
    public sealed class MasterDataDataSeeder : IMasterDataDataSeeder
    {
        public void Seed(DbContext dbContext)
        {
            InitialPaymentType(dbContext);
            InitialReferenceType(dbContext);
            InitialCurrency(dbContext);
            InitialCountry(dbContext);
            InitialCity(dbContext);
            InitialDistrict(dbContext);
        }

        public void InitialPaymentType(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<PaymentType> list = new List<PaymentType> {
                  new PaymentType { Id = 1, Name = "Cash", Hide = false },
                  new PaymentType { Id = 2, Name = "Check", Hide = false }
            };

            foreach (var ob in list)
            {
                if (!orgContext.Set<PaymentType>().Any(e => e.Id == ob.Id))
                    orgContext.Set<PaymentType>().Add(ob);
                else
                    orgContext.Entry<PaymentType>(orgContext.Set<PaymentType>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        // Ids mirror Treasury.Domain.FinancialReferenceType 1:1 so Financial.ReferenceType (still an int
        // enum column) can resolve its display name against this table without a separate mapping.
        public void InitialReferenceType(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<ReferenceType> list = new List<ReferenceType> {
                  new ReferenceType { Id = 0, Name = "Other", Hide = false },
                  new ReferenceType { Id = 1, Name = "Customer", Hide = false },
                  new ReferenceType { Id = 2, Name = "Supplier", Hide = false },
                  new ReferenceType { Id = 3, Name = "Employee", Hide = false },
                  new ReferenceType { Id = 4, Name = "Expense", Hide = false },
                  new ReferenceType { Id = 5, Name = "Income", Hide = false },
                  new ReferenceType { Id = 6, Name = "Invoice", Hide = false },
                  new ReferenceType { Id = 7, Name = "Payment", Hide = false },
                  new ReferenceType { Id = 8, Name = "Loan", Hide = false },
                  new ReferenceType { Id = 9, Name = "Cheque", Hide = false },
                  new ReferenceType { Id = 10, Name = "Payment Gateway", Hide = false },
                  new ReferenceType { Id = 11, Name = "Transfer", Hide = false }
            };

            foreach (var ob in list)
            {
                if (!orgContext.Set<ReferenceType>().Any(e => e.Id == ob.Id))
                    orgContext.Set<ReferenceType>().Add(ob);
                else
                    orgContext.Entry<ReferenceType>(orgContext.Set<ReferenceType>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialCurrency(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<Currency> list = new List<Currency> {
                 new Currency { Name = "Epg", Hide = false }
            };

            if (!orgContext.Set<Currency>().Any())
                orgContext.Set<Currency>().AddRange(list);
            orgContext.SaveChanges();
        }

        // Country/City/District are plain BaseModel lockups (Name + Code only — no NameAr/ISO2/ISO3/
        // PhoneCode columns exist on these entities, so this seed doesn't add any), identity-Id like
        // everything else in this file. Idempotency is by business key, not Id: Country by Code (its
        // ISO2), City by (CountryId, Name), District by (CityId, Name) — matching InitialAccount's
        // Code-based approach, since two different countries/cities can legitimately share a Name.
        public void InitialCountry(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            var rows = new (string Code, string Name)[]
            {
                ("EG", "Egypt"),
                ("SA", "Saudi Arabia"),
                ("AE", "United Arab Emirates"),
                ("KW", "Kuwait"),
                ("QA", "Qatar"),
                ("BH", "Bahrain"),
                ("OM", "Oman"),
                ("SD", "Sudan"),
                ("LY", "Libya"),
                ("TN", "Tunisia"),
                ("DZ", "Algeria"),
                ("MA", "Morocco"),
                ("MR", "Mauritania"),
            };

            foreach (var row in rows)
            {
                var existing = orgContext.Set<Country>().FirstOrDefault(e => e.Code == row.Code);
                if (existing == null)
                    orgContext.Set<Country>().Add(new Country { Code = row.Code, Name = row.Name, Hide = false });
                else
                    existing.Name = row.Name;
            }
            orgContext.SaveChanges();
        }

        public void InitialCity(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            var countryIdByCode = orgContext.Set<Country>().ToDictionary(e => e.Code!, e => e.Id);

            AddCities(orgContext, countryIdByCode["EG"], EgyptCities());
            AddCities(orgContext, countryIdByCode["SA"], SaudiArabiaCities());
            AddCities(orgContext, countryIdByCode["AE"], UAECities());
            AddCities(orgContext, countryIdByCode["KW"], KuwaitCities());
            AddCities(orgContext, countryIdByCode["QA"], QatarCities());
            AddCities(orgContext, countryIdByCode["BH"], BahrainCities());
            AddCities(orgContext, countryIdByCode["OM"], OmanCities());
            AddCities(orgContext, countryIdByCode["SD"], SudanCities());
            AddCities(orgContext, countryIdByCode["LY"], LibyaCities());
            AddCities(orgContext, countryIdByCode["TN"], TunisiaCities());
            AddCities(orgContext, countryIdByCode["DZ"], AlgeriaCities());
            AddCities(orgContext, countryIdByCode["MA"], MoroccoCities());
            AddCities(orgContext, countryIdByCode["MR"], MauritaniaCities());

            orgContext.SaveChanges();
        }

        private static void AddCities(Microsoft.EntityFrameworkCore.DbContext orgContext, long countryId, string[] names)
        {
            foreach (var name in names)
            {
                var existing = orgContext.Set<City>().FirstOrDefault(e => e.CountryId == countryId && e.Name == name);
                if (existing == null)
                    orgContext.Set<City>().Add(new City { CountryId = countryId, Name = name, Hide = false });
            }
        }

        private static string[] EgyptCities() => new[]
        {
            "Cairo", "Giza", "Alexandria", "Dakahlia", "Red Sea", "Beheira", "Fayoum", "Gharbia",
            "Ismailia", "Menofia", "Minya", "Qalyubia", "New Valley", "Suez", "Aswan", "Assiut",
            "Beni Suef", "Port Said", "Damietta", "Sharqia", "South Sinai", "Kafr El Sheikh",
            "Matrouh", "Luxor", "Qena", "North Sinai", "Sohag",
        };

        private static string[] SaudiArabiaCities() => new[]
        {
            "Riyadh", "Jeddah", "Mecca", "Medina", "Dammam", "Khobar", "Dhahran", "Taif", "Tabuk",
            "Abha", "Khamis Mushait", "Buraidah", "Hail", "Jubail", "Yanbu", "Jazan", "Najran", "Al Ahsa",
        };

        private static string[] UAECities() => new[]
        {
            "Abu Dhabi", "Dubai", "Sharjah", "Ajman", "Umm Al Quwain", "Ras Al Khaimah", "Fujairah", "Al Ain",
        };

        private static string[] KuwaitCities() => new[]
        {
            "Kuwait City", "Hawalli", "Salmiya", "Farwaniya", "Jahra", "Ahmadi", "Mubarak Al-Kabeer",
        };

        private static string[] QatarCities() => new[]
        {
            "Doha", "Al Rayyan", "Al Wakrah", "Al Khor", "Umm Salal", "Lusail",
        };

        private static string[] BahrainCities() => new[]
        {
            "Manama", "Muharraq", "Riffa", "Hamad Town", "Isa Town",
        };

        private static string[] OmanCities() => new[]
        {
            "Muscat", "Salalah", "Sohar", "Nizwa", "Sur", "Barka", "Seeb",
        };

        private static string[] SudanCities() => new[]
        {
            "Khartoum", "Omdurman", "Khartoum North", "Port Sudan", "Kassala", "Gedaref",
            "Wad Madani", "El Obeid", "Nyala", "Atbara",
        };

        private static string[] LibyaCities() => new[]
        {
            "Tripoli", "Benghazi", "Misrata", "Sabha", "Sirte", "Zawiya", "Tobruk",
        };

        private static string[] TunisiaCities() => new[]
        {
            "Tunis", "Sfax", "Sousse", "Bizerte", "Kairouan", "Gabes", "Monastir",
        };

        private static string[] AlgeriaCities() => new[]
        {
            "Algiers", "Oran", "Constantine", "Annaba", "Blida", "Setif", "Batna", "Tlemcen",
        };

        private static string[] MoroccoCities() => new[]
        {
            "Casablanca", "Rabat", "Marrakech", "Fez", "Tangier", "Agadir", "Meknes", "Oujda", "Tetouan",
        };

        private static string[] MauritaniaCities() => new[]
        {
            "Nouakchott", "Nouadhibou", "Rosso", "Kaedi", "Atar",
        };

        public void InitialDistrict(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            var countryIdByCode = orgContext.Set<Country>().ToDictionary(e => e.Code!, e => e.Id);
            var cityIdByCountryAndName = orgContext.Set<City>()
                .Where(e => e.CountryId != null)
                .ToDictionary(e => (e.CountryId!.Value, e.Name!), e => e.Id);

            AddDistricts(orgContext, countryIdByCode["EG"], cityIdByCountryAndName, EgyptDistricts());
            AddDistricts(orgContext, countryIdByCode["SA"], cityIdByCountryAndName, SaudiArabiaDistricts());
            AddDistricts(orgContext, countryIdByCode["AE"], cityIdByCountryAndName, UAEDistricts());

            orgContext.SaveChanges();
        }

        private static void AddDistricts(Microsoft.EntityFrameworkCore.DbContext orgContext, long countryId,
            Dictionary<(long CountryId, string CityName), long> cityIdByCountryAndName, (string City, string District)[] rows)
        {
            foreach (var row in rows)
            {
                // Skip silently if the city lookup ever falls out of sync with the district list above —
                // seeding the rest of the batch matters more than failing the whole run over one row.
                if (!cityIdByCountryAndName.TryGetValue((countryId, row.City), out var cityId))
                    continue;

                var existing = orgContext.Set<District>().FirstOrDefault(e => e.CityId == cityId && e.Name == row.District);
                if (existing == null)
                    orgContext.Set<District>().Add(new District { CountryId = countryId, CityId = cityId, Name = row.District, Hide = false });
            }
        }

        private static (string City, string District)[] EgyptDistricts()
        {
            var cairo = new[]
            {
                "Nasr City", "Heliopolis", "New Cairo", "Fifth Settlement", "First Settlement", "Third Settlement",
                "Maadi", "Zahraa El Maadi", "Mokattam", "Dar El Salam", "Basateen", "Sayeda Zeinab",
                "Downtown Cairo", "Abdeen", "Zamalek", "Shubra", "Ain Shams", "Matariya", "Zeitoun",
                "Hadayek El Kobba", "El Marg", "Salam City", "Nozha", "Badr City", "El Shorouk",
                "New Administrative Capital", "Helwan", "15 May City",
            };
            var giza = new[]
            {
                "Dokki", "Mohandessin", "Agouza", "Haram", "Faisal", "Omrania", "Boulaq El Dakrour",
                "Imbaba", "Warraq", "Sheikh Zayed", "6th of October", "Hadayek October", "New Giza",
                "Kerdasa", "Abu Rawash", "Hawamdia", "Badrasheen",
            };
            var alexandria = new[]
            {
                "Montaza", "Miami", "Sidi Bishr", "San Stefano", "Gleem", "Roushdy", "Smouha", "Sidi Gaber",
                "Sporting", "Stanley", "Louran", "Mandara", "Asafra", "Agami", "Borg El Arab",
                "New Borg El Arab", "Moharam Bek", "Mansheya",
            };
            // Remaining 24 governorates: main markaz/towns, not exhaustive — enough to make the District
            // dropdown usable everywhere without trying to model every markaz in the country.
            var rest = new (string City, string[] Districts)[]
            {
                ("Dakahlia", new[] { "Mansoura", "Talkha", "Mit Ghamr", "Aga" }),
                ("Red Sea", new[] { "Hurghada", "Safaga", "Marsa Alam", "Ras Gharib" }),
                ("Beheira", new[] { "Damanhur", "Kafr El Dawwar", "Rashid", "Edku" }),
                ("Fayoum", new[] { "Fayoum City", "Sinnuris", "Tamiya", "Ibsheway" }),
                ("Gharbia", new[] { "Tanta", "Mahalla El Kubra", "Kafr El Zayat", "Zefta" }),
                ("Ismailia", new[] { "Ismailia City", "Fayed", "Qantara", "Tel El Kebir" }),
                ("Menofia", new[] { "Shibin El Kom", "Sadat City", "Menouf", "Ashmoun" }),
                ("Minya", new[] { "Minya City", "Mallawi", "Beni Mazar", "Samalut" }),
                ("Qalyubia", new[] { "Banha", "Shubra El Kheima", "Qalyub", "Khanka", "Obour City" }),
                ("New Valley", new[] { "Kharga", "Dakhla", "Farafra" }),
                ("Suez", new[] { "Suez City", "Ain Sokhna" }),
                ("Aswan", new[] { "Aswan City", "Kom Ombo", "Edfu", "Daraw" }),
                ("Assiut", new[] { "Assiut City", "Dairut", "Manfalut", "Abnub" }),
                ("Beni Suef", new[] { "Beni Suef City", "El Wasta", "Nasser", "Biba" }),
                ("Port Said", new[] { "Port Fouad", "Al Manakh", "Al Zohour" }),
                ("Damietta", new[] { "Damietta City", "New Damietta", "Faraskur", "Ras El Bar" }),
                ("Sharqia", new[] { "Zagazig", "Belbeis", "Abu Kabir", "10th of Ramadan City" }),
                ("South Sinai", new[] { "Sharm El Sheikh", "Dahab", "Nuweiba", "Taba", "Saint Catherine" }),
                ("Kafr El Sheikh", new[] { "Kafr El Sheikh City", "Desouk", "Fuwwah", "Baltim" }),
                ("Matrouh", new[] { "Marsa Matrouh", "Siwa", "El Alamein", "El Dabaa" }),
                ("Luxor", new[] { "Luxor City", "Esna", "Armant" }),
                ("Qena", new[] { "Qena City", "Nag Hammadi", "Qus", "Deshna" }),
                ("North Sinai", new[] { "Arish", "Sheikh Zuweid", "Rafah", "Bir al-Abd" }),
                ("Sohag", new[] { "Sohag City", "Akhmim", "Girga", "Tahta" }),
            };

            return Combine("Cairo", cairo)
                .Concat(Combine("Giza", giza))
                .Concat(Combine("Alexandria", alexandria))
                .Concat(rest.SelectMany(g => Combine(g.City, g.Districts)))
                .ToArray();
        }

        private static (string City, string District)[] SaudiArabiaDistricts()
        {
            var groups = new (string City, string[] Districts)[]
            {
                ("Riyadh", new[] { "Olaya", "Malaz", "Al Naseem", "Al Malqa", "Al Sulimaniyah", "Al Murabba", "Diriyah" }),
                ("Jeddah", new[] { "Al Rawdah", "Al Salamah", "Al Hamra", "Al Zahra", "Al Naeem", "Al Balad", "Obhur" }),
                ("Mecca", new[] { "Al Aziziyah", "Al Shoqiah", "Al Nassim", "Ajyad", "Al Awali" }),
                ("Medina", new[] { "Al Aziziyah", "Quba", "Al Haram", "Al Ranuna", "Al Awali" }),
                ("Dammam", new[] { "Al Faisaliyah", "Al Shati", "Al Rakah", "Al Adamah", "Al Manar" }),
                ("Khobar", new[] { "Al Aqrabiyah", "Al Ulaya", "Al Thuqbah", "Al Yarmouk", "Corniche" }),
            };
            return groups.SelectMany(g => Combine(g.City, g.Districts)).ToArray();
        }

        private static (string City, string District)[] UAEDistricts()
        {
            var groups = new (string City, string[] Districts)[]
            {
                ("Dubai", new[] { "Deira", "Bur Dubai", "Jumeirah", "Downtown Dubai", "Dubai Marina", "Al Barsha", "Business Bay", "Al Qusais" }),
                ("Abu Dhabi", new[] { "Al Khalidiyah", "Al Bateen", "Al Reem Island", "Al Zahiyah", "Mussafah", "Khalifa City", "Al Muroor" }),
                ("Sharjah", new[] { "Al Majaz", "Al Qasimia", "Al Nahda", "Al Taawun", "Al Khan", "Muwaileh" }),
            };
            return groups.SelectMany(g => Combine(g.City, g.Districts)).ToArray();
        }

        private static IEnumerable<(string City, string District)> Combine(string city, string[] districts) =>
            districts.Select(d => (city, d));
    }
}
