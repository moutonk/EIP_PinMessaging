
namespace PinMessaging.Model
{
    public class PMLogInCreateStructureModel
    {
        public enum ActionType
        {
            SignIn,
            Create
        };

        public PMLogInCreateStructureModel(string pageTitle, string pageSubTitle, string buttonTitle, ActionType actionType)
        {
            PageTitle = pageTitle;
            PageSubTitle = pageSubTitle;
            ButtonTitle = buttonTitle;
            ActionTypeVar = actionType;
        }

        public string PageTitle { get; set; }
        public string PageSubTitle { get; set; }
        public string ButtonTitle { get; set; }
        public ActionType ActionTypeVar { get; set; }
    }
}
