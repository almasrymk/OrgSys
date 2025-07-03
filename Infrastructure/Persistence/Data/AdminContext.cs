namespace Infrastructure.Persistence.Data
{
    using Utility;
    using Domain.Entities;
    using Domain.Abstraction;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class AdminContext : DbContext , IAdminContext
    {
        public AdminContext(DbContextOptions<AdminContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("OrgConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Andorran" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Emirati" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Afghan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Antiguan, Barbudan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Anguillian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Albanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Armenian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Dutch" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Angolan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Antarctican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Argentinean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "American Samoan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Austrian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Australian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Aruban" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Swedish" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Azerbaijani" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Bosnian, Herzegovinian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Barbadian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Bangladeshi" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Belgian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Burkinabe" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Bulgarian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Bahraini" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Burundian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Beninese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Saint Barthélemy Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Bermudian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Bruneian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Bolivian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Dutch" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Brazilian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Bahamian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Bhutanese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Bouvet Island" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Motswana" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Belarusian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Belizean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Canadian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Cocos Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Congolese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Central African" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Congolese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Swiss" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Ivorian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Cook Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Chilean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Cameroonian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Chinese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Colombian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Costa Rican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Montenegrins, Serbs" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Canton and Enderbury Islands" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Cuban" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Cape Verdian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Curaçaoan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Christmas Island" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Cypriot" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Czech" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "German" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Djibouti" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Danish" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Dominican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Dominican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Algerian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Ecuadorean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Estonian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Egyptian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Sahrawi" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Eritrean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Spanish" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Ethiopian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Finnish" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Fijian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Falkland Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Micronesian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Faroese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "French Southern and Antarctic Territories" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "French" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Metropolitan France" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Gabonese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "British" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Grenadian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Georgian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "French Guiana" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Channel Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Ghanaian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Gibraltar" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Greenlandic" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Gambian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Guinean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Guadeloupian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Equatorial Guinean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Greek" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "South Georgia and the South Sandwich Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Guatemalan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Guamanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Guinea-Bissauan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Guyanese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Chinese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Heard and McDonald Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Honduran" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Croatian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Haitian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Hungarian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Indonesian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Irish" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Israeli" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Manx" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Indian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Indian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Iraqi" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Iranian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Icelander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Italian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Channel Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Jamaican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Jordanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Japanese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Johnston Island" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Kenyan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Kirghiz" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Cambodian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "I-Kiribati" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Comoran" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Kittian and Nevisian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "North Korean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "South Korean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Kuwaiti" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Caymanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Kazakhstani" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Laotian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Lebanese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Saint Lucian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Liechtensteiner" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Sri Lankan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Liberian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Mosotho" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Lithuanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Luxembourger" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Latvian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Libyan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Moroccan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Monegasque" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Moldovan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Montenegrin" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Saint Martin Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Malagasy" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Marshallese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Midway Islands" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Macedonian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Malian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Myanmar" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Mongolian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Chinese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "American" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "French" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Mauritanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Montserratian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Maltese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Mauritian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Maldivan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Malawian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Mexican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Malaysian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Mozambican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Namibian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "New Caledonian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Nigerian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Norfolk Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Nigerian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Nicaraguan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Dutch" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Norwegian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Nepalese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Dronning Maud Land" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Nauruan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Neutral Zone" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Niuean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "New Zealander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Omani" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Panamanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Pacific Islands Trust Territory" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Peruvian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "French Polynesian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Papua New Guinean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Filipino" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Pakistani" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Polish" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "French" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Pitcairn Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Puerto Rican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Palestinian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Portuguese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "U.S. Miscellaneous Pacific Islands" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Palauan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Paraguayan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Panama Canal Zone" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Qatari" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "French" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Romanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Serbian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Russian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Rwandan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Saudi Arabian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Solomon Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Seychellois" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Sudanese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Swedish" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Singaporean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Saint Helenian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Slovene" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Norwegian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Slovak" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Sierra Leonean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Sammarinese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Senegalese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Somali" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Surinamer" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Sao Tomean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Union of Soviet Socialist Republics" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Salvadoran" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Syrian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Swazi" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Turks and Caicos Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Chadian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "French" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Togolese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Thai" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Tadzhik" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Tokelauan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "East Timorese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Turkmen" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Tunisian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Tongan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Tongan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Trinidadian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Tuvaluan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Taiwanese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Tanzanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Ukrainian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Ugandan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "American" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "American" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Uruguayan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Uzbekistani" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Italian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Saint Vincentian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "North Vietnam" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Venezuelan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Virgin Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Virgin Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Vietnamese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Ni-Vanuatu" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Wallis and Futuna Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Wake Island" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Samoan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Yemeni" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "French" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "South African" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Zambian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = new Guid(), Name = "Zimbabwean" });

            modelBuilder.Entity<TypeActivity>().HasData(new TypeActivity { Id = new Guid(), Name = "Supermarket" });

            modelBuilder.Entity<PlanType>().HasData(new PlanType { Id = new Guid(), Name = "Always" });
            modelBuilder.Entity<PlanType>().HasData(new PlanType { Id = new Guid(), Name = "Limited in time" });

            modelBuilder.Entity<Plan>().HasData(new Plan { Id = new Guid(), Name = "Trial", PlanTypeId = 1, Price = "5$", Description = "Two-week trial", Offer = "0", PriceAfterOffer = "5$"/*, CodeNumber = 1, Code = "1"*/ });
            modelBuilder.Entity<Plan>().HasData(new Plan { Id = new Guid(), Name = "Basic", PlanTypeId = 1, Price = "400$", Description = "Two-week trial", Offer = "First year discount offer 50%", PriceAfterOffer = "200$"/*, CodeNumber = 1, Code = "1"*/ });

            modelBuilder.Entity<Request>().HasData(new Request { Id = new Guid(), /*CodeNumber = 1, Code = "1",*/ CompanyName = "org", Email = "info@org.com", Phone = "0201111105784", Name = "Mohammed Khaled", ExpireDate = new DateTime(2021, 1, 1), URL = "" });

            modelBuilder.Entity<Client>().HasData(new Client { Id = new Guid(), Name = "Org", /*Code = "1", CodeNumber = 1,*/ DbSchema = "org", Email = "info@org.com", NationalityId = 68, TypeActivityId = 1, SizeOfCompany = 1, Phone = "0201111105784", Mobile = "0201111105784", VersionDb = 1 });

            modelBuilder.Entity<ClientPlan>().HasData(new ClientPlan { Id = new Guid(), ClientId = 1, PlanId = 1, StartDate = new DateTime(2021, 1, 1), EndDate = new DateTime(2022, 1, 1)/*, Code = "1", CodeNumber = 1*/ });

            modelBuilder.Entity<LoginUser>().HasData(new LoginUser { Id = new Guid(), UserName = "Owner", Password = Security.Encrypt("P@ssw0rd"), ClientId = 1/*, Code = "1", CodeNumber = 1*/ });
            modelBuilder.Entity<LoginUser>().HasData(new LoginUser { Id = new Guid(), UserName = "Admin", Password = Security.Encrypt("P@ssw0rd"), ClientId = 1/*, Code = "1", CodeNumber = 1*/ });
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientPlan> ClientPlans { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<PlanElement> PlanElements { get; set; }
        public virtual DbSet<PlanType> PlanTypes { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<LoginUser> LoginUsers { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<TypeActivity> TypeActivities { get; set; }
        public virtual DbSet<GeneralCity> GeneralCities { get; set; }
        public virtual DbSet<GeneralClassification> GeneralClassifications { get; set; }
        public virtual DbSet<GeneralCountry> GeneralCountries { get; set; }
        public virtual DbSet<GeneralDistrict> GeneralDistricts { get; set; }
        public virtual DbSet<GeneralProduct> GeneralProducts { get; set; }
        public virtual DbSet<GeneralProductPropertyElement> GeneralProductPropertyElements { get; set; }
        public virtual DbSet<GeneralProductRecipe> GeneralProductRecipes { get; set; }
        public virtual DbSet<GeneralProductUnit> GeneralProductUnits { get; set; }
        public virtual DbSet<GeneralProperty> GeneralProperties { get; set; }
        public virtual DbSet<GeneralPropertyElement> GeneralPropertyElements { get; set; }
        public virtual DbSet<GeneralUnit> GeneralUnits { get; set; }
    }
}