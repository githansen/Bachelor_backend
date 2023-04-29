using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Bachelor_backend.Models
{
    //Input validation
    [ValidateTarget]
    public class TargetGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Targetid { get; set; } 
        public virtual List<string>? Genders { get; set; }
        public virtual List<string>? Languages { get; set; }
        public virtual List<string>? Dialects { get; set; }
        public virtual List<string>? AgeGroups { get; set; }
    }


    public class ValidateTarget : ValidationAttribute
    {
        public override bool IsValid(Object targetgroup)
        {
            var target = (TargetGroup)targetgroup;

            if (target?.AgeGroups != null)
            {
                var regexAgeGroup = new Regex(@"^(\d){2}-(\d){2}|(\d){2}\+$");
                foreach (var Agegroup in target.AgeGroups)
                {
                    if (!regexAgeGroup.IsMatch(Agegroup))
                    {
                        return false;
                    }
                }
            }

            if (target?.Languages != null)
            {
                var regexLanguage = new Regex("^([a-zA-ZæøåÆØÅ]{4,16})$");
                foreach (var lang in target.Languages)
                {
                    if (!regexLanguage.IsMatch(lang))
                    {
                        return false;
                    }
                }
            }

            if (target?.Dialects != null)
            {


                var regexDialect = new Regex("^([a-zA-ZæøåÆØÅ]{4,16})$");
                foreach (var dialect in target.Dialects)
                {
                    if (!regexDialect.IsMatch(dialect))
                    {
                        return false;
                    }
                }
            }
            if(target?.Genders != null)
            {
                var regexGenders = new Regex("^(mann|kvinne|annet|Mann|Kvinne|Annet)$");
                foreach(var gender in target.Genders)
                {
                    if (!regexGenders.IsMatch(gender))
                    {
                        return false;
                    }
                }
            }


            return true;
        }

    }
}
