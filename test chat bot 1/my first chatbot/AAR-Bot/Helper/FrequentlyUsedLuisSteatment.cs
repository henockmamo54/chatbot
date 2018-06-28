using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AAR_Bot.Helper
{
    public class FrequentlyUsedLuisSteatment
    {
        public string _couserInfo = "";
        Rootobject obj;
        private Dictionary<string, IntentEntity> englishFrequentStatements = new Dictionary<string, IntentEntity>();

        public FrequentlyUsedLuisSteatment() {
            addAllEnglishSentences();
        }
        
        public Rootobject getIntentOfString(string statement)
        {
            var mydata = englishFrequentStatements.Where(x => x.Key.Contains(statement));

            if (mydata.Count() != 0) {
                obj = new Rootobject();
                var keyvaluepair = mydata.First();
                IntentEntity intentEntity = (IntentEntity) keyvaluepair.Value;
                obj.topScoringIntent = new Topscoringintent();
                obj.topScoringIntent.intent = intentEntity.Intent;

                Entity entity = new Entity();
                entity.entity = intentEntity.Entity;
                entity.type = intentEntity.Entity;
                Entity[] entities={ entity};
                obj.entities = entities;
            }
            
            return obj;
        }

        struct IntentEntity
        {
            public string Intent;
            public string Entity;

            public IntentEntity(string intent, string entity) {
                Intent = intent;
                Entity = entity;
            }
        }


        private void addAllEnglishSentences()
        {
            englishFrequentStatements.Add("course information", new IntentEntity("CourseInformation", ""));
            englishFrequentStatements.Add("opened major", new IntentEntity("CourseInformation", "openedmajor"));
            englishFrequentStatements.Add("syllabus", new IntentEntity("CourseInformation", "syllabus"));
            englishFrequentStatements.Add("mandatory subject", new IntentEntity("CourseInformation", "mandatorysubject"));
            englishFrequentStatements.Add("prerequisite", new IntentEntity("CourseInformation", "prerequisite"));
        }

    }
}