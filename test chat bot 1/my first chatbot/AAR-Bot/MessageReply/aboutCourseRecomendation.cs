using AAR_Bot.Dialogs;
using AAR_Bot.Helper.StoredStringValues;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;

namespace AAR_Bot.MessageReply
{
    public static class aboutCourseRecomendation
    {
        static StoredStringValuesMaster _storedvalues;
        static string lang = "";
        public static async Task CourseRecomendationOptionSelected(IDialogContext context)
        {
            lang = context.PrivateConversationData.GetValue<string>("_storedvalues");
            var langtype = new StoredStringValuesMaster();
            if (lang.Equals("StoredValues_en")) _storedvalues = new StoredValues_en();
            else if (lang.Equals("StoredValues_kr")) _storedvalues = new StoredValues_kr();

            var activity = context.MakeMessage();
            activity.Text = _storedvalues._recommendedCourse + RootDialog.studentinfo.getrecommendedCourselist(60131937).Trim().Replace("  ", ",");
            await context.PostAsync(activity);

        }
    }

}