using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace EloquaStepCanvas
{
    public class EloquaInstance
    {

        private EloquaService.EloquaServiceClient serviceProxy;
        private EloquaProgramService.ExternalActionServiceClient programServiceProxy;

        private DateTime dttLastEloquaAPICall;


        public EloquaInstance(string InstanceName, string UserID, string UserPassword)
        {
            string strInstanceName = InstanceName;
            string strUserID = UserID;
            string strUserPassword = UserPassword;

            serviceProxy = new EloquaService.EloquaServiceClient();
            serviceProxy.ClientCredentials.UserName.UserName = strInstanceName + "\\" + strUserID;
            serviceProxy.ClientCredentials.UserName.Password = strUserPassword;


            programServiceProxy = new EloquaProgramService.ExternalActionServiceClient();
            programServiceProxy.ClientCredentials.UserName.UserName = strInstanceName + "\\" + strUserID;
            programServiceProxy.ClientCredentials.UserName.Password = strUserPassword;

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            //without prior knowledge, set the last API call 1s ago to allow new object instances to invoke call
            dttLastEloquaAPICall = DateTime.Now.ToUniversalTime().Subtract(TimeSpan.FromMilliseconds(1000));

        }
    }
}