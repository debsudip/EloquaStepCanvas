using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using EloquaStepCanvas.EloquaService;
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
            
      
            programServiceProxy = new EloquaProgramService.ExternalActionServiceClient();
            stepId =  Request.QueryString["StepID"];
            company = Request.QueryString["Company"];

            int count = 0;
            string strInstanceName = "CognizantTechnologySolutionsNetherlandsB";
            string strUserID = "Deb.Sudip";
            string strUserPassword = "Welcome1";

            programServiceProxy.ClientCredentials.UserName.UserName = strInstanceName + "\\" + strUserID;
            programServiceProxy.ClientCredentials.UserName.Password = strUserPassword;
            EloquaInstance objInstance = new EloquaInstance(strInstanceName, strUserID, strUserPassword);

            //It counts the total members/contacts available in the current step
            count = objInstance.CountMembersInStepByStatus(Convert.ToInt32(stepId), 0);
            lblTimertime.Text = "Timer refreshed at: " + DateTime.Now.ToLongTimeString();
            lblContact.Text = "Total Contacts : " + count.ToString();  // Contacts.Count.ToString();
            lblCompany.Text = "Instance : " + company;
            lblStepID.Text = "StepID : " + stepId;

            //It retrieves the contacts awaiting in the current step
            Contacts = ListContactsInStep(Convert.ToInt32(stepId), EloquaProgramService.ExternalActionStatus.AwaitingAction, 100);
            lstTotalContact.Text = "Total Contacts received :" + Contacts.Count.ToString();

            Contacts = SetStatusOfContactsInStep(Convert.ToInt32(stepId), EloquaProgramService.ExternalActionStatus.AwaitingAction, EloquaProgramService.ExternalActionStatus.Complete, Contacts);
            lstChangeStatus.Text = "Status change of the number of contacts :" + Contacts.Count.ToString();
 

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

        public List<EloquaContact> SetStatusOfContactsInStep(int intProgramStepID, EloquaStepCanvas.EloquaProgramService.ExternalActionStatus OldStepStatus, EloquaStepCanvas.EloquaProgramService.ExternalActionStatus NewStepStatus, List<EloquaContact> tmpContactsInStep)
        {
            List<EloquaContact> tmpUpdatedContacts = new List<EloquaContact>();
            EloquaContact tmpContact;
            // EloquaProgramService.ExternalActionStatus OldStatus;
            // EloquaProgramService.ExternalActionStatus NewStatus;
            EloquaProgramService.Member[] stepMembers;
            // OldStatus = (EloquaProgramService.ExternalActionStatus)intOldStepStatus;
            // NewStatus = (EloquaProgramService.ExternalActionStatus)intNewStepStatus;
            EloquaProgramService.Member tmpMember;
            stepMembers = new EloquaProgramService.Member[tmpContactsInStep.Count()];

            for (int index = 0; index < tmpContactsInStep.Count(); index++)
            {
                tmpMember = new EloquaProgramService.Member();
                tmpMember.EntityId = tmpContactsInStep.ElementAt(index).ContactID;
                tmpMember.EntityType = (EloquaProgramService.EntityType)1;
                tmpMember.StepId = intProgramStepID;
                tmpMember.Status = OldStepStatus;
                tmpMember.Id = tmpContactsInStep.ElementAt(index).ExternalActionID;
                stepMembers[index] = tmpMember;

                InsertDataCard(tmpContactsInStep.ElementAt(index).EmailAddress);
            }


            var results = programServiceProxy.SetMemberStatus(stepMembers, NewStepStatus);

            foreach (EloquaProgramService.Member tmpUpdatedMember in results)
            {
                tmpContact = new EloquaContact();
                tmpContact.ContactID = tmpUpdatedMember.EntityId;
                tmpContact.ExternalActionID = tmpUpdatedMember.Id;
                tmpUpdatedContacts.Add(tmpContact);
            }
            return tmpUpdatedContacts;
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            //int count = 0;
            //string strInstanceName = "CognizantTechnologySolutionsNetherlandsB";
            //string strUserID = "Deb.Sudip";
            //string strUserPassword = "Welcome1";

            //programServiceProxy.ClientCredentials.UserName.UserName = strInstanceName + "\\" + strUserID;
            //programServiceProxy.ClientCredentials.UserName.Password = strUserPassword;
            //EloquaInstance objInstance = new EloquaInstance(strInstanceName, strUserID, strUserPassword);

            ////It counts the total members/contacts available in the current step
            //count = objInstance.CountMembersInStepByStatus(Convert.ToInt32(stepId), 0);
            //lblTimertime.Text = "Timer refreshed at: " + DateTime.Now.ToLongTimeString();
            //lblContact.Text = "Total Contacts : " + count.ToString();  // Contacts.Count.ToString();
            //lblCompany.Text = "Instance : " + company;
            //lblStepID.Text = "StepID : " + stepId;

            ////It retrieves the contacts awaiting in the current step
            //Contacts = ListContactsInStep(Convert.ToInt32(stepId), EloquaProgramService.ExternalActionStatus.AwaitingAction, 100);
            //lstTotalContact.Text = "Total Contacts received :" + Contacts.Count.ToString();

            //Contacts = SetStatusOfContactsInStep(Convert.ToInt32(stepId), EloquaProgramService.ExternalActionStatus.AwaitingAction, EloquaProgramService.ExternalActionStatus.Complete, Contacts);
            //lstChangeStatus.Text = "Status change of the number of contacts :" + Contacts.Count.ToString();
 
        }

        #region Setting Eloqua Credentials
        /// <summary>
        /// Connect to the instance and create proxy
        /// </summary>
        //private void SetEloquaServices(EloquaServiceClient serviceProxy, EloquaProgramService.ExternalActionServiceClient programServiceProxy)
        //{
        //    //string strInstanceName = "CSStuartOrmistonE10";
        //    //string strUserID = "Prashanth.Govindaiah";
        //    //string strUserPassword = "Infy1234";

        //    string strInstanceName = "CognizantTechnologySolutionsNetherlandsB";
        //    string strUserID = "Deb.Sudip";
        //    string strUserPassword = "Welcome1";

        //    serviceProxy.ClientCredentials.UserName.UserName = strInstanceName + "\\" + strUserID;
        //    serviceProxy.ClientCredentials.UserName.Password = strUserPassword;

        //    programServiceProxy = new EloquaProgramService.ExternalActionServiceClient();
        //    programServiceProxy.ClientCredentials.UserName.UserName = strInstanceName + "\\" + strUserID;
        //    programServiceProxy.ClientCredentials.UserName.Password = strUserPassword;
        //}
        #endregion

        private void InsertDataCard(string email)
        {
            int dataCardId = 0;
            var dataCardIDs = new int[1];
            serviceProxy = new EloquaService.EloquaServiceClient();
            string strInstanceName = "CognizantTechnologySolutionsNetherlandsB";
            string strUserID = "Deb.Sudip";
            string strUserPassword = "Welcome1";

            serviceProxy.ClientCredentials.UserName.UserName = strInstanceName + "\\" + strUserID;
            serviceProxy.ClientCredentials.UserName.Password = strUserPassword;
           // EloquaInstance objInstance = new EloquaInstance(strInstanceName, strUserID, strUserPassword);

            //Perform the authentication
         //   this.SetEloquaServices(serviceProxy, programServiceProxy);

            // Build a DataCardSet Entity Type object - (the ID is the ID of an existing DataCardSet in Eloqua)
            EntityType entityType = new EntityType { ID = 8, Name = "Product_Interested_Info", Type = "DataCardSet" };

            // Create an Array of Dynamic Entities
            DynamicEntity[] dynamicEntities = new DynamicEntity[1];

            // Create a new Dynamic Entity and add it to the Array of Entities
            dynamicEntities[0] = new DynamicEntity();
            dynamicEntities[0].EntityType = entityType;

            // Create a Dynamic Entity's Field Value Collection
            dynamicEntities[0].FieldValueCollection = new DynamicEntityFields();
            dynamicEntities[0].FieldValueCollection.Add("Email_Address1", email);
            dynamicEntities[0].FieldValueCollection.Add("PI11", "<B>" + "P1" + "</B>");
            dynamicEntities[0].FieldValueCollection.Add("PI21", "<B>" + "P2" + "</B>");
            dynamicEntities[0].FieldValueCollection.Add("PI31", "<B>" + "P3" + "</B>");
            dynamicEntities[0].FieldValueCollection.Add("PI1Desc1", "Shaver");
            dynamicEntities[0].FieldValueCollection.Add("PI2Desc1", "Razor");
            dynamicEntities[0].FieldValueCollection.Add("PI3Desc1", "Saving Blade");
            dynamicEntities[0].FieldValueCollection.Add("PI1Image1", "<" + "ImageShaver" + "/>");
            dynamicEntities[0].FieldValueCollection.Add("PI2Image1", "<" + "ImageRazor" + "/>");
            dynamicEntities[0].FieldValueCollection.Add("PI3Image1", "<" + "ImageBlade" + "/>");
            dynamicEntities[0].FieldValueCollection.Add("MappedEntityType", "1");
            dynamicEntities[0].FieldValueCollection.Add("MappedEntityID", "1");

            // Execute the request
            var result = serviceProxy.Create(dynamicEntities);

            // Verify the status of each DataCard Create request in the results
            foreach (CreateResult t in result)
            {
                //// Successfull requests return a positive integer value for ID
                if (t.ID != -1)
                {
                    dataCardId = t.ID;
                }
                else
                {
                    // Extract the Error Message and Error Code for each failed Create request
                    {

                    }
                }
            }
            lstInsertCDO.Text = " Insert into Product_Interested_Info ";
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