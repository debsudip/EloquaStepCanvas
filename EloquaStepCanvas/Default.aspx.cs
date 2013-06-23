using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
namespace EloquaStepCanvas
{
    public partial class Default : System.Web.UI.Page
    {
        EloquaService.EloquaServiceClient serviceProxy;
        EloquaProgramService.ExternalActionServiceClient programServiceProxy;
        string stepId;
        string company;
        List<EloquaContact> Contacts;

        protected void Page_Load(object sender, EventArgs e)
        {
            serviceProxy = new EloquaService.EloquaServiceClient();
            programServiceProxy = new EloquaProgramService.ExternalActionServiceClient();
            stepId = Request.QueryString["STEP_ID"];
            company = Request.QueryString["COMPANY"];

        }


        public List<EloquaContact> ListContactsInStep(int intPBStepID, EloquaStepCanvas.EloquaProgramService.ExternalActionStatus intStepStatus, int intBatchSize)
        {
            List<EloquaContact> tmpContactsInStep = new List<EloquaContact>();
            EloquaContact tmpContact;
            int intPageNumber = 0;


            string strInstanceName = "CognizantTechnologySolutionsNetherlandsB";
            string strUserID = "Deb.Sudip";
            string strUserPassword = "Welcome1";

            programServiceProxy.ClientCredentials.UserName.UserName = strInstanceName + "\\" + strUserID;
            programServiceProxy.ClientCredentials.UserName.Password = strUserPassword;

            EloquaProgramService.Member[] result = null;

            result = programServiceProxy.ListMembersInStepByStatus(intPBStepID, intStepStatus, intPageNumber, intBatchSize);


            if (result != null)
            {
                foreach (var eam in result)
                {
                   
                        if (eam != null)
                        {
                            if ((int)eam.EntityType == 1)
                            {
                                tmpContact = new EloquaContact();
                                tmpContact.ContactID = eam.EntityId;
                                tmpContact.ExternalActionID = eam.Id;
                                tmpContactsInStep.Add(tmpContact);
                            }
                        }
                    }
                   
                }
            

            return tmpContactsInStep;
        }

        public List<EloquaContact> SetStatusOfContactsInStep(int intProgramStepID, int intOldStepStatus, int intNewStepStatus, List<EloquaContact> tmpContactsInStep)
        {
            List<EloquaContact> tmpUpdatedContacts = new List<EloquaContact>();
            EloquaContact tmpContact;
            EloquaProgramService.ExternalActionStatus OldStatus;
            EloquaProgramService.ExternalActionStatus NewStatus;
            EloquaProgramService.Member[] stepMembers;
            OldStatus = (EloquaProgramService.ExternalActionStatus)intOldStepStatus;
            NewStatus = (EloquaProgramService.ExternalActionStatus)intNewStepStatus;
            EloquaProgramService.Member tmpMember;

            stepMembers = new EloquaProgramService.Member[tmpContactsInStep.Count()];
            try
            {
                for (int index = 0; index < tmpContactsInStep.Count(); index++)
                {
                    tmpMember = new EloquaProgramService.Member();
                    tmpMember.EntityId = tmpContactsInStep.ElementAt(index).ContactID;
                    tmpMember.EntityType = (EloquaProgramService.EntityType)1;
                    tmpMember.StepId = intProgramStepID;
                    tmpMember.Status = OldStatus;
                    tmpMember.Id = tmpContactsInStep.ElementAt(index).ExternalActionID;
                    stepMembers[index] = tmpMember;
                }

                var results = programServiceProxy.SetMemberStatus(stepMembers, NewStatus);

                foreach (EloquaProgramService.Member tmpUpdatedMember in results)
                {
                    tmpContact = new EloquaContact();
                    tmpContact.ContactID = tmpUpdatedMember.EntityId;
                    tmpContact.ExternalActionID = tmpUpdatedMember.Id;
                    tmpUpdatedContacts.Add(tmpContact);
                }
            }
            catch (Exception ex)
            {
                //Trace.TraceWarning(String.Format("Exception in EloquaInstance:SetStatusOfContactsInStep {0} :: {1} :: Step {2}", ex.Message, ex.InnerException, intProgramStepID.ToString()), "Exception");
                throw;
            }
            return tmpUpdatedContacts;
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {

            Contacts = new List<EloquaContact>();

           Contacts = ListContactsInStep(Convert.ToInt32 (stepId), EloquaStepCanvas.EloquaProgramService.ExternalActionStatus.AwaitingAction, 100);

            lblTimertime.Text = "Timer refreshed at: " + DateTime.Now.ToLongTimeString();
            lblContact.Text = "Total Contacts : " + Contacts.Count.ToString();
            lblCompany.Text = "Instance : " + company;
            lblStepID.Text = "StepID : " + stepId;
        }
    }

    public class EloquaContact
    {

        public int ContactID { get; set; }
        public string EmailAddress { get; set; }
        public int ExternalActionID { get; set; }
        public EloquaContact()
        {
            ContactID = 0;
            EmailAddress = "";
            ExternalActionID = 0;
        }
    }


}