namespace Treasury.Infrastructure.Seeding
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public interface ITreasuryDataSeeder
    {
        void Seed(DbContext dbContext);
    }

    /// <summary>
    /// Treasury's slice of the legacy InitialData seed (FinancialType, default CashBox, Bank +
    /// BankBranch reference data for EG/SA/AE/KW/QA/BH/OM/SD/LY/TN/DZ/MA/MR) — relocated verbatim,
    /// split by module ownership. See
    /// Administration.Infrastructure.Seeding.AdministrationDataSeeder for the shared rationale.
    /// </summary>
    public sealed class TreasuryDataSeeder : ITreasuryDataSeeder
    {
        public void Seed(DbContext dbContext)
        {
            InitialFinancialType(dbContext);
            InitialCashBox(dbContext);
            InitialBank(dbContext);
            InitialBankBranch(dbContext);
        }

        public void InitialFinancialType(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<FinancialType> list = new List<FinancialType> {
                 new FinancialType { Id = 1, Name = "OpeningBalance", Hide = false, InOut = 1, Icon = "iconsminds-start-2" },
                 new FinancialType { Id = 2, Name = "Receipt", Hide = false, InOut = 1, Icon = "iconsminds-financial" },
                 new FinancialType { Id = 3, Name = "Payment", Hide = false, InOut = -1, Icon = "iconsminds-handshake" },
                 new FinancialType { Id = 4, Name = "TransferIn", Hide = false, InOut = 1, Icon = "simple-icon-shuffle" },
                 new FinancialType { Id = 5, Name = "Deposit", Hide = false, InOut = 1, Icon = "iconsminds-down-1" },
                 new FinancialType { Id = 6, Name = "Withdrawal", Hide = false, InOut = -1, Icon = "iconsminds-up-1" },
                 new FinancialType { Id = 7, Name = "Fee", Hide = false, InOut = -1, Icon = "iconsminds-receipt-4" },
                 new FinancialType { Id = 8, Name = "Interest", Hide = false, InOut = 1, Icon = "iconsminds-line-chart-1" },
                 new FinancialType { Id = 9, Name = "Cheque", Hide = false, InOut = 0, Icon = "iconsminds-check" },
                 new FinancialType { Id = 10, Name = "Adjustment", Hide = false, InOut = 0, Icon = "iconsminds-gear" },
                 new FinancialType { Id = 11, Name = "TransferOut", Hide = false, InOut = -1, Icon = "simple-icon-shuffle" }
            };

            foreach (var ob in list)
            {
                if (!orgContext.Set<FinancialType>().Any(e => e.Id == ob.Id))
                    orgContext.Set<FinancialType>().Add(ob);
                else
                    orgContext.Entry<FinancialType>(orgContext.Set<FinancialType>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialCashBox(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<CashBox> list = new List<CashBox> {
                 new CashBox { Name = "Main Cash Box", Hide = false }
            };

            if (!orgContext.Set<CashBox>().Any())
                orgContext.Set<CashBox>().AddRange(list);
            orgContext.SaveChanges();
        }

        // Bank/BankBranch: same idempotency approach as Country/City/District (business key, not Id),
        // but batched rather than per-row queried — existing Banks/BankBranches are loaded once up front
        // into lookup structures and reconciled in memory, then written with a single AddRange +
        // SaveChanges per method, since this seed is an order of magnitude larger (~500 branch candidates)
        // than the per-row InitialAccount/InitialDistrict pattern comfortably handles as N+1 queries.
        // No SWIFT/branch codes are seeded — none were supplied with confirmed values, and Code sits
        // unused (null) rather than invented, so the fallback natural keys below are always what's used:
        // Bank by (CountryId, Name), BankBranch by (BankId, CityId, DistrictId, Name).
        public void InitialBank(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            var countryIdByCode = orgContext.Set<Country>().ToDictionary(e => e.Code!, e => e.Id);
            var existingBanks = orgContext.Set<Bank>()
                .Where(e => e.CountryId != null)
                .Select(e => new { e.CountryId, e.Name })
                .AsEnumerable()
                .Select(e => (e.CountryId!.Value, e.Name!))
                .ToHashSet();

            var toAdd = new List<Bank>();
            void Collect(string countryCode, string[] names)
            {
                var countryId = countryIdByCode[countryCode];
                foreach (var name in names)
                    if (!existingBanks.Contains((countryId, name)))
                        toAdd.Add(new Bank { CountryId = countryId, Name = name, Hide = false });
            }

            Collect("EG", EgyptBanks());
            Collect("SA", SaudiArabiaBanks());
            Collect("AE", UAEBanks());
            Collect("KW", KuwaitBanks());
            Collect("QA", QatarBanks());
            Collect("BH", BahrainBanks());
            Collect("OM", OmanBanks());
            Collect("SD", SudanBanks());
            Collect("LY", LibyaBanks());
            Collect("TN", TunisiaBanks());
            Collect("DZ", AlgeriaBanks());
            Collect("MA", MoroccoBanks());
            Collect("MR", MauritaniaBanks());

            if (toAdd.Count > 0)
            {
                orgContext.Set<Bank>().AddRange(toAdd);
                orgContext.SaveChanges();
            }
        }

        private static string[] EgyptBanks() => new[]
        {
            // Government / major banks
            "National Bank of Egypt", "Banque Misr", "Banque du Caire", "Agricultural Bank of Egypt",
            // Private / commercial / foreign-subsidiary banks
            "Commercial International Bank (CIB)", "QNB Egypt", "Arab African International Bank", "AlexBank",
            "Credit Agricole Egypt", "HSBC Bank Egypt", "Emirates NBD Egypt", "Abu Dhabi Islamic Bank Egypt (ADIB)",
            "Abu Dhabi Commercial Bank Egypt (ADCB)", "Mashreq Bank Egypt", "First Abu Dhabi Bank Egypt (FABMISR)",
            "Arab International Bank", "Arab Bank Egypt", "Kuwait Finance House Egypt",
            "Housing and Development Bank", "Suez Canal Bank", "Export Development Bank of Egypt",
            "Egyptian Gulf Bank (EG Bank)", "Al Baraka Bank Egypt", "Faisal Islamic Bank of Egypt",
            "MIDBANK", "saib", "Industrial Development Bank", "The United Bank",
        };

        private static string[] SaudiArabiaBanks() => new[]
        {
            "Saudi National Bank (SNB)", "Al Rajhi Bank", "Riyad Bank", "Alinma Bank", "Bank Albilad",
            "Bank AlJazira", "Saudi Awwal Bank (SAB)", "Arab National Bank (ANB)", "Banque Saudi Fransi",
            "Gulf International Bank Saudi Arabia",
        };

        private static string[] UAEBanks() => new[]
        {
            "First Abu Dhabi Bank (FAB)", "Emirates NBD", "Abu Dhabi Commercial Bank (ADCB)",
            "Abu Dhabi Islamic Bank (ADIB)", "Mashreq", "Dubai Islamic Bank", "Emirates Islamic",
            "Commercial Bank of Dubai", "RAKBANK", "Sharjah Islamic Bank", "National Bank of Fujairah",
        };

        private static string[] KuwaitBanks() => new[]
        {
            "National Bank of Kuwait", "Kuwait Finance House", "Gulf Bank", "Commercial Bank of Kuwait",
            "Burgan Bank", "Boubyan Bank", "Warba Bank", "Kuwait International Bank", "Al Ahli Bank of Kuwait",
        };

        private static string[] QatarBanks() => new[]
        {
            "Qatar National Bank (QNB)", "Qatar Islamic Bank (QIB)", "Commercial Bank Qatar", "Doha Bank",
            "Dukhan Bank", "Qatar International Islamic Bank", "Ahlibank Qatar", "Masraf Al Rayan",
        };

        private static string[] BahrainBanks() => new[]
        {
            "National Bank of Bahrain", "Bank of Bahrain and Kuwait", "Al Salam Bank", "Bahrain Islamic Bank",
            "Ahli United Bank", "Gulf International Bank", "ila Bank",
        };

        private static string[] OmanBanks() => new[]
        {
            "Bank Muscat", "Bank Dhofar", "National Bank of Oman", "Sohar International", "Oman Arab Bank",
            "Bank Nizwa", "Ahli Bank Oman",
        };

        private static string[] SudanBanks() => new[]
        {
            "Bank of Khartoum", "Omdurman National Bank", "Faisal Islamic Bank Sudan",
            "Sudanese French Bank", "Blue Nile Mashreg Bank",
        };

        private static string[] LibyaBanks() => new[]
        {
            "Jumhouria Bank", "National Commercial Bank (Libya)", "Sahara Bank", "Wahda Bank",
            "Bank of Commerce and Development",
        };

        private static string[] TunisiaBanks() => new[]
        {
            "Banque Nationale Agricole (BNA)", "Société Tunisienne de Banque (STB)", "Banque de l'Habitat (BH Bank)",
            "Attijari Bank Tunisie", "Banque Internationale Arabe de Tunisie (BIAT)", "Amen Bank",
            "Al Baraka Bank Tunisia",
        };

        private static string[] AlgeriaBanks() => new[]
        {
            "Banque Extérieure d'Algérie (BEA)", "Banque Nationale d'Algérie (BNA)",
            "Crédit Populaire d'Algérie (CPA)", "Banque de Développement Local (BDL)",
            "Société Générale Algérie", "BNP Paribas El Djazair",
        };

        private static string[] MoroccoBanks() => new[]
        {
            "Attijariwafa Bank", "Banque Populaire", "Bank of Africa", "CIH Bank", "Crédit du Maroc",
            "Société Générale Maroc",
        };

        private static string[] MauritaniaBanks() => new[]
        {
            "Banque Nationale de Mauritanie (BNM)", "Générale de Banque de Mauritanie (GBM)", "Chinguitty Bank",
        };

        // Only Egypt/Saudi Arabia/UAE get branches — the only countries with District-level geography
        // seeded (BankBranch.CityId/DistrictId are non-nullable FKs, so a branch literally cannot be
        // created without a real district). Egypt's branch list mirrors the district-level detail asked
        // for; Saudi/UAE get one representative "main branch" location per city instead, matching the
        // shallower request for those countries. Every (bank, city, district) triple below resolves
        // against rows InitialCity/InitialDistrict already created — anything that doesn't resolve is
        // skipped rather than guessed at.
        public void InitialBankBranch(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            var countryIdByCode = orgContext.Set<Country>().ToDictionary(e => e.Code!, e => e.Id);
            var cityIdByCountryAndName = orgContext.Set<City>()
                .Where(e => e.CountryId != null)
                .ToDictionary(e => (e.CountryId!.Value, e.Name!), e => e.Id);
            var districtIdByCityAndName = orgContext.Set<District>()
                .Where(e => e.CityId != null)
                .ToDictionary(e => (e.CityId!.Value, e.Name!), e => e.Id);
            var bankIdByCountryAndName = orgContext.Set<Bank>()
                .Where(e => e.CountryId != null)
                .ToDictionary(e => (e.CountryId!.Value, e.Name!), e => e.Id);
            var existingBranches = orgContext.Set<BankBranch>()
                .Select(e => new { e.BankId, e.CityId, e.DistrictId, e.Name })
                .AsEnumerable()
                .Select(e => (e.BankId, e.CityId, e.DistrictId, e.Name!))
                .ToHashSet();

            var toAdd = new List<BankBranch>();
            void Collect(string countryCode, string[] bankNames, (string City, string District)[] locations)
            {
                if (!countryIdByCode.TryGetValue(countryCode, out var countryId)) return;
                foreach (var bankName in bankNames)
                {
                    if (!bankIdByCountryAndName.TryGetValue((countryId, bankName), out var bankId)) continue;
                    foreach (var loc in locations)
                    {
                        if (!cityIdByCountryAndName.TryGetValue((countryId, loc.City), out var cityId)) continue;
                        if (!districtIdByCityAndName.TryGetValue((cityId, loc.District), out var districtId)) continue;

                        var key = (bankId, cityId, districtId, loc.District);
                        if (!existingBranches.Add(key)) continue; // already in DB, or a duplicate location in this run

                        toAdd.Add(new BankBranch
                        {
                            BankId = bankId,
                            CountryId = countryId,
                            CityId = cityId,
                            DistrictId = districtId,
                            Name = loc.District,
                            Hide = false,
                        });
                    }
                }
            }

            Collect("EG", EgyptBranchBanks(), EgyptBranchLocations());
            Collect("SA", SaudiArabiaBranchBanks(), SaudiArabiaBranchLocations());
            Collect("AE", UAEBranchBanks(), UAEBranchLocations());

            if (toAdd.Count > 0)
            {
                orgContext.Set<BankBranch>().AddRange(toAdd);
                orgContext.SaveChanges();
            }
        }

        // The 5 Egyptian banks with a genuinely nationwide branch network — confident enough to claim
        // presence in every district below. The other 23 Egypt banks (foreign-subsidiary, specialised or
        // smaller commercial banks) are seeded with no branches at all rather than guessing their footprint.
        private static string[] EgyptBranchBanks() => new[]
        {
            "National Bank of Egypt", "Banque Misr", "Banque du Caire",
            "Commercial International Bank (CIB)", "QNB Egypt",
        };

        private static (string City, string District)[] EgyptBranchLocations()
        {
            var cairo = new[]
            {
                "Downtown Cairo", "Nasr City", "Heliopolis", "New Cairo", "Fifth Settlement", "Maadi",
                "Zamalek", "Shubra", "Mokattam", "Helwan", "Badr City", "El Shorouk",
            };
            var giza = new[] { "Dokki", "Mohandessin", "Faisal", "Haram", "Sheikh Zayed", "6th of October" };
            // "Downtown Alexandria" isn't a separate seeded district — Mansheya is Alexandria's actual
            // historic downtown/city-centre district, so it stands in for it rather than inventing a row.
            var alexandria = new[] { "Smouha", "Sidi Gaber", "Roushdy", "Miami", "Montaza", "Mansheya", "Borg El Arab" };
            var others = new (string City, string District)[]
            {
                ("Dakahlia", "Mansoura"), ("Gharbia", "Tanta"), ("Sharqia", "Zagazig"),
                ("Ismailia", "Ismailia City"), ("Suez", "Suez City"), ("Port Said", "Al Manakh"),
                ("Damietta", "Damietta City"), ("Assiut", "Assiut City"), ("Sohag", "Sohag City"),
                ("Minya", "Minya City"), ("Beni Suef", "Beni Suef City"), ("Fayoum", "Fayoum City"),
                ("Luxor", "Luxor City"), ("Aswan", "Aswan City"), ("Red Sea", "Hurghada"),
                ("South Sinai", "Sharm El Sheikh"),
            };

            return Combine("Cairo", cairo)
                .Concat(Combine("Giza", giza))
                .Concat(Combine("Alexandria", alexandria))
                .Concat(others)
                .ToArray();
        }

        // Saudi/UAE: one confirmed, well-known central district per city as the "main branch" location —
        // matching the shallower "أهم الفروع" ask for these countries rather than Egypt's district-by-district
        // detail. Gulf International Bank Saudi Arabia is excluded: it's a wholesale/corporate bank without
        // a retail branch network to place with any confidence.
        private static string[] SaudiArabiaBranchBanks() => new[]
        {
            "Saudi National Bank (SNB)", "Al Rajhi Bank", "Riyad Bank", "Alinma Bank", "Bank Albilad",
            "Bank AlJazira", "Saudi Awwal Bank (SAB)", "Arab National Bank (ANB)", "Banque Saudi Fransi",
        };

        // Jubail, Taif, Abha and Tabuk (also asked for) have no District-level data seeded, so no branch
        // rows can be created there — see the Summary for this gap.
        private static (string City, string District)[] SaudiArabiaBranchLocations() => new[]
        {
            ("Riyadh", "Olaya"), ("Jeddah", "Al Rawdah"), ("Mecca", "Ajyad"),
            ("Medina", "Al Haram"), ("Dammam", "Al Faisaliyah"), ("Khobar", "Corniche"),
        };

        private static string[] UAEBranchBanks() => new[]
        {
            "First Abu Dhabi Bank (FAB)", "Emirates NBD", "Abu Dhabi Commercial Bank (ADCB)",
            "Abu Dhabi Islamic Bank (ADIB)", "Mashreq", "Dubai Islamic Bank", "Emirates Islamic",
            "Commercial Bank of Dubai", "RAKBANK", "Sharjah Islamic Bank", "National Bank of Fujairah",
        };

        // Ajman, Al Ain and Ras Al Khaimah (also asked for) have no District-level data seeded either.
        private static (string City, string District)[] UAEBranchLocations() => new[]
        {
            ("Dubai", "Deira"), ("Abu Dhabi", "Al Khalidiyah"), ("Sharjah", "Al Majaz"),
        };

        // Duplicated from MasterDataDataSeeder's identical helper (originally a single shared
        // private method in the legacy InitialData) — trivial, stateless, and used by both
        // module seeders' branch-location tuples; not worth a cross-module dependency to share.
        private static IEnumerable<(string City, string District)> Combine(string city, string[] districts) =>
            districts.Select(d => (city, d));
    }
}
