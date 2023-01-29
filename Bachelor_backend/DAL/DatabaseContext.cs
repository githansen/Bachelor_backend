using Bachelor_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Bachelor_backend.DAL
{


    public class DatabaseContext : DbContext
    {
        public class TagForText
        {
            public int TagId { get; set; }
            public Tag tag { get; set; }
            public int TextId { get; set; }
            public Text Text { get; set; }
        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Audiofile> Audiofiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
           modelBuilder.Entity<Text>()
                .HasMany(t => t.Tags)
                .WithMany(t => t.Texts)
                .UsingEntity(t => t.ToTable("TagsForTexts"));

           
            var tag1 = new Tag {TagId = 1, TagText = "Narkotika" };
            var tag2 = new Tag { TagId = 2,TagText = "Kniv" };
            var user1 = new User { UserId = 1, AgeGroup = "18-28", Type = "Target", Dialect = null, NativeLanguage = "English" };

            modelBuilder.Entity<User>().HasData(user1);
            var tekst1 = new Text { TextId = 1, TextText = "Jeg dør for mine bros og de dør for meg\r\nFakk alle hater jeg sverger jeg er lei\r\nBitches prøvde å teste de lo men nå\r\nLer jeg\r\nNgrn switcha opp det ække mitt problem det var din feil\r\nJeg sa jeg har det bra\r\nIkke ødeleg ditt liv det var det mamma sa\r\nMen jeg er oppvokst i et helvete det er der jeg er fra\r\nStarta ung som rapper og jeg vet hvor jeg skal\r\nJeg skaffa gode læg med min trapphone\r\nMine niggas vi er fly for jeg satser nå\r\nMamma please don't cry, vi skal nå opp til topp\r\nKommer ikke hjem med hull i klærne lenger nå\r\nHva hadde skjedd om jeg var hvit (mamma)\r\nIkke politisaker om jeg var hvit (mamma)\r\nTil og med skole hadde gått fint (mamma)\r\nDet er derfor de ser fuck politi (fuck dem Haha/mamma)\r\nHaterne snakker luft ja de snakker shit\r\n16 år og allerede opplevd nok i mitt liv\r\nKaren var 10 år første gang karen løp fra politi\r\nMamma gråter i et avhør hun vet ingenting\r\nFuck politi ja fuck politi\r\nDe vet ikke hvem du er men de fakker opp din liv\r\nHud fargen har tydeligvis mye å si\r\nBryr seg 0 om din family 0 om din familly\r\nFikk ikke jobb så jeg valgte å trappe\r\nFakk alle disse haterne jeg gjør mitt beste\r\nJeg shower off en bil og kommentar feltet fullt av den ække din men fuck dem neger for dem hakke vært i mine værdager\r\nJeg hadde flere planer men mange av dem gikk galt\r\nArtist planen er den eneste som går som den skal\r\nJeg mista min bestemor, unkel og min bestefar\r\nAlle tårene gjorde det vanskelig å sove på natta\r\nAlt jeg har gjort jeg har gjort det på\r\nEgenhånd\r\nEneste jeg kunne lene meg på var min unkel bro\r\nPoliti venter i buskene men jeg løpet som bolt\r\nIngen kan ta min familly det beste jg har fått\r\nJeg er flink til å late som ingenting har skjedd\r\nDet er bare mine close ones som jeg snakker med\r\nJeg ække en morder men ikke test meg neger\r\nJeg ække en morder men ikke test meg neger\r\nFuck politi ja fuck politi\r\nDe vet ikke hvem du er men de fakker opp din liv\r\nHud fargen har tydeligvis mye å si\r\nBryr seg 0 om din family 0 om din familly", UserId =1 };
            var tekst2 = new Text { TextId = 2, TextText = "Jeg har Bakke kontakt jeg lar dem fly\r\nDu hakke sett en shit lil boy hold tyst\r\nBle født i et mørkt sted men jeg fant lys\r\nJeg har bitches overalt nå boy i alle byer\r\nFor her i hooden det går fast\r\nVarer går fram og tilbake neger vi gjør task\r\nLeker du big man bitch du får slapp\r\nFakk hva de sier rambow starta fra scratch (hæ)\r\nFor jeg har gjort tusenvis av feil\r\nMen jeg har lært av dem alle jeg holder meg til samme spor\r\nHaterne for snakke for jeg blikke lei\r\nRambow on the track det er fortsatt fire in the booth\r\nJeg vil bare catche green green (green green)\r\nIngen av oss hadde læg tro meg vi levde tungt\r\nJeg vil bare catche ti på ti på ti\r\nMine brødre i gata hakke jobb de catcher floos\r\nOps ved vår blokk de fåkke bli\r\nJeg følger ikke andres sti jeg lager min\r\nFuck haters nigga vi gjør vår ting\r\nDu sier du er best lil boy stop å lyv\r\nDu leker g til du møter på veggen\r\nVi lekte politi og tyv det ække lek lenger\r\nHadde mange før nå har jeg 3 venner\r\nFordi fake ble til real og real ble til fake neger\r\nPappa lærte meg trust får deg drept\r\nVanskelig å lære men tok step by step\r\nFikk fame nå er alle sammen glad i meg igjen\r\nVi ække brødre eller day ones bitch ass hold kjeft\r\nNå vil alle bli bros for real\r\nHold deg unna bitch stop tråkke på min tå\r\nNå har jeg hoes og freaky bitches\r\nJeg ække g men jeg trykker om jeg må\r\nJeg vil bare catche green green (green green)\r\nIngen av oss hadde læg tro meg vi levde tungt\r\nJeg vil bare catche ti på ti på ti\r\nMine brødre i gata hakke jobb de catcher floos" , UserId = 1};
            modelBuilder.Entity<Tag>().HasData(tag1, tag2);
            modelBuilder.Entity<Text>().HasData(tekst1, tekst2);
           
        }
      
    }
}
