using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Utility;

namespace Repository
{
    public class AdminContext : DbContext
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
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 1, Name = "Andorran" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 2, Name = "Emirati" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 3, Name = "Afghan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 4, Name = "Antiguan, Barbudan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 5, Name = "Anguillian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 6, Name = "Albanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 7, Name = "Armenian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 8, Name = "Dutch" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 9, Name = "Angolan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 10, Name = "Antarctican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 11, Name = "Argentinean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 12, Name = "American Samoan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 13, Name = "Austrian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 14, Name = "Australian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 15, Name = "Aruban" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 16, Name = "Swedish" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 17, Name = "Azerbaijani" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 18, Name = "Bosnian, Herzegovinian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 19, Name = "Barbadian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 20, Name = "Bangladeshi" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 21, Name = "Belgian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 22, Name = "Burkinabe" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 23, Name = "Bulgarian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 24, Name = "Bahraini" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 25, Name = "Burundian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 26, Name = "Beninese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 27, Name = "Saint Barthélemy Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 28, Name = "Bermudian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 29, Name = "Bruneian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 30, Name = "Bolivian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 31, Name = "Dutch" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 32, Name = "Brazilian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 33, Name = "Bahamian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 34, Name = "Bhutanese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 35, Name = "Bouvet Island" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 36, Name = "Motswana" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 37, Name = "Belarusian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 38, Name = "Belizean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 39, Name = "Canadian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 40, Name = "Cocos Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 41, Name = "Congolese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 42, Name = "Central African" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 43, Name = "Congolese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 44, Name = "Swiss" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 45, Name = "Ivorian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 46, Name = "Cook Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 47, Name = "Chilean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 48, Name = "Cameroonian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 49, Name = "Chinese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 50, Name = "Colombian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 51, Name = "Costa Rican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 52, Name = "Montenegrins, Serbs" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 53, Name = "Canton and Enderbury Islands" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 54, Name = "Cuban" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 55, Name = "Cape Verdian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 56, Name = "Curaçaoan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 57, Name = "Christmas Island" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 58, Name = "Cypriot" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 59, Name = "Czech" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 60, Name = "German" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 61, Name = "Djibouti" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 62, Name = "Danish" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 63, Name = "Dominican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 64, Name = "Dominican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 65, Name = "Algerian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 66, Name = "Ecuadorean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 67, Name = "Estonian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 68, Name = "Egyptian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 69, Name = "Sahrawi" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 70, Name = "Eritrean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 71, Name = "Spanish" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 72, Name = "Ethiopian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 73, Name = "Finnish" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 74, Name = "Fijian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 75, Name = "Falkland Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 76, Name = "Micronesian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 77, Name = "Faroese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 78, Name = "French Southern and Antarctic Territories" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 79, Name = "French" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 80, Name = "Metropolitan France" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 81, Name = "Gabonese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 82, Name = "British" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 83, Name = "Grenadian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 84, Name = "Georgian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 85, Name = "French Guiana" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 86, Name = "Channel Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 87, Name = "Ghanaian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 88, Name = "Gibraltar" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 89, Name = "Greenlandic" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 90, Name = "Gambian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 91, Name = "Guinean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 92, Name = "Guadeloupian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 93, Name = "Equatorial Guinean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 94, Name = "Greek" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 95, Name = "South Georgia and the South Sandwich Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 96, Name = "Guatemalan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 97, Name = "Guamanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 98, Name = "Guinea-Bissauan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 99, Name = "Guyanese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 100, Name = "Chinese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 101, Name = "Heard and McDonald Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 102, Name = "Honduran" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 103, Name = "Croatian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 104, Name = "Haitian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 105, Name = "Hungarian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 106, Name = "Indonesian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 107, Name = "Irish" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 108, Name = "Israeli" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 109, Name = "Manx" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 110, Name = "Indian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 111, Name = "Indian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 112, Name = "Iraqi" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 113, Name = "Iranian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 114, Name = "Icelander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 115, Name = "Italian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 116, Name = "Channel Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 117, Name = "Jamaican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 118, Name = "Jordanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 119, Name = "Japanese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 120, Name = "Johnston Island" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 121, Name = "Kenyan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 122, Name = "Kirghiz" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 123, Name = "Cambodian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 124, Name = "I-Kiribati" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 125, Name = "Comoran" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 126, Name = "Kittian and Nevisian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 127, Name = "North Korean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 128, Name = "South Korean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 129, Name = "Kuwaiti" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 130, Name = "Caymanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 131, Name = "Kazakhstani" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 132, Name = "Laotian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 133, Name = "Lebanese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 134, Name = "Saint Lucian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 135, Name = "Liechtensteiner" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 136, Name = "Sri Lankan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 137, Name = "Liberian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 138, Name = "Mosotho" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 139, Name = "Lithuanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 140, Name = "Luxembourger" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 141, Name = "Latvian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 142, Name = "Libyan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 143, Name = "Moroccan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 144, Name = "Monegasque" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 145, Name = "Moldovan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 146, Name = "Montenegrin" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 147, Name = "Saint Martin Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 148, Name = "Malagasy" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 149, Name = "Marshallese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 150, Name = "Midway Islands" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 151, Name = "Macedonian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 152, Name = "Malian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 153, Name = "Myanmar" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 154, Name = "Mongolian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 155, Name = "Chinese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 156, Name = "American" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 157, Name = "French" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 158, Name = "Mauritanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 159, Name = "Montserratian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 160, Name = "Maltese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 161, Name = "Mauritian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 162, Name = "Maldivan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 163, Name = "Malawian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 164, Name = "Mexican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 165, Name = "Malaysian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 166, Name = "Mozambican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 167, Name = "Namibian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 168, Name = "New Caledonian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 169, Name = "Nigerian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 170, Name = "Norfolk Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 171, Name = "Nigerian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 172, Name = "Nicaraguan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 173, Name = "Dutch" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 174, Name = "Norwegian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 175, Name = "Nepalese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 176, Name = "Dronning Maud Land" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 177, Name = "Nauruan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 178, Name = "Neutral Zone" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 179, Name = "Niuean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 180, Name = "New Zealander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 181, Name = "Omani" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 182, Name = "Panamanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 183, Name = "Pacific Islands Trust Territory" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 184, Name = "Peruvian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 185, Name = "French Polynesian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 186, Name = "Papua New Guinean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 187, Name = "Filipino" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 188, Name = "Pakistani" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 189, Name = "Polish" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 190, Name = "French" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 191, Name = "Pitcairn Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 192, Name = "Puerto Rican" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 193, Name = "Palestinian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 194, Name = "Portuguese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 195, Name = "U.S. Miscellaneous Pacific Islands" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 196, Name = "Palauan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 197, Name = "Paraguayan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 198, Name = "Panama Canal Zone" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 199, Name = "Qatari" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 200, Name = "French" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 201, Name = "Romanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 202, Name = "Serbian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 203, Name = "Russian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 204, Name = "Rwandan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 205, Name = "Saudi Arabian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 206, Name = "Solomon Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 207, Name = "Seychellois" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 208, Name = "Sudanese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 209, Name = "Swedish" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 210, Name = "Singaporean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 211, Name = "Saint Helenian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 212, Name = "Slovene" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 213, Name = "Norwegian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 214, Name = "Slovak" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 215, Name = "Sierra Leonean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 216, Name = "Sammarinese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 217, Name = "Senegalese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 218, Name = "Somali" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 219, Name = "Surinamer" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 220, Name = "Sao Tomean" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 221, Name = "Union of Soviet Socialist Republics" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 222, Name = "Salvadoran" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 223, Name = "Syrian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 224, Name = "Swazi" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 225, Name = "Turks and Caicos Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 226, Name = "Chadian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 227, Name = "French" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 228, Name = "Togolese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 229, Name = "Thai" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 230, Name = "Tadzhik" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 231, Name = "Tokelauan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 232, Name = "East Timorese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 233, Name = "Turkmen" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 234, Name = "Tunisian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 235, Name = "Tongan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 236, Name = "Tongan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 237, Name = "Trinidadian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 238, Name = "Tuvaluan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 239, Name = "Taiwanese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 240, Name = "Tanzanian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 241, Name = "Ukrainian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 242, Name = "Ugandan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 243, Name = "American" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 244, Name = "American" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 245, Name = "Uruguayan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 246, Name = "Uzbekistani" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 247, Name = "Italian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 248, Name = "Saint Vincentian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 249, Name = "North Vietnam" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 250, Name = "Venezuelan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 251, Name = "Virgin Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 252, Name = "Virgin Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 253, Name = "Vietnamese" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 254, Name = "Ni-Vanuatu" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 255, Name = "Wallis and Futuna Islander" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 256, Name = "Wake Island" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 257, Name = "Samoan" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 258, Name = "Yemeni" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 259, Name = "French" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 260, Name = "South African" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 261, Name = "Zambian" });
            modelBuilder.Entity<Nationality>().HasData(new Nationality { Id = 262, Name = "Zimbabwean" });            

            modelBuilder.Entity<TypeActivity>().HasData(new TypeActivity { Id = 1, Name = "Supermarket" });

            modelBuilder.Entity<PlanType>().HasData(new PlanType { Id = 1, Name = "Always" });
            modelBuilder.Entity<PlanType>().HasData(new PlanType { Id = 2, Name = "Limited in time" });

            modelBuilder.Entity<Plan>().HasData(new Plan { Id = 1, Name = "Trial" , PlanTypeId = 1 , Price = "5$" , Description = "Two-week trial" , Offer = "0" , PriceAfterOffer = "5$" , CodeNumber = 1 , Code = "1" });
            modelBuilder.Entity<Plan>().HasData(new Plan { Id = 2, Name = "Basic", PlanTypeId = 1, Price = "400$", Description = "Two-week trial", Offer = "First year discount offer 50%", PriceAfterOffer = "200$", CodeNumber = 1, Code = "1" });

            modelBuilder.Entity<Request>().HasData(new Request { Id = 1, CodeNumber = 1, Code = "1", CompanyName = "org", Email = "info@org.com", Phone = "0201111105784", Name = "Mohammed Khaled", ExpireDate = new System.DateTime(2021, 1, 1), URL = "" });

            modelBuilder.Entity<Client>().HasData(new Client { Id = 1, Name = "Org" , Code = "1" , CodeNumber = 1 , DbSchema = "org" , Email = "info@org.com" , NationalityId = 68 , TypeActivityId = 1 , SizeOfCompany = 1 , Phone = "0201111105784" , Mobile = "0201111105784" , VersionDb = 1 });

            modelBuilder.Entity<ClientPlan>().HasData(new ClientPlan { Id = 1, ClientId = 1 , PlanId = 1 , StartDate = new System.DateTime(2021 , 1, 1) , EndDate = new System.DateTime(2022, 1, 1) , Code = "1" , CodeNumber = 1 });

            modelBuilder.Entity<LoginUser>().HasData(new LoginUser { Id = 1, UserName = "Owner" , Password = Security.Encrypt("P@ssw0rd") , ClientId = 1 , Code = "1" , CodeNumber = 1 });

            modelBuilder.Entity<LoginUser>().HasData(new LoginUser { Id = 2, UserName = "Admin", Password = Security.Encrypt("P@ssw0rd") , ClientId = 1, Code = "1", CodeNumber = 1 });
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